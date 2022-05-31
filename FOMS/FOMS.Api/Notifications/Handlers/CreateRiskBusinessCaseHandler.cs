using DomainServices.SalesArrangementService.Abstraction;
using DomainServices.OfferService.Abstraction;
using DomainServices.CaseService.Abstraction;
using DomainServices.SalesArrangementService.Abstraction;
using _Case = DomainServices.CaseService.Contracts;
using _SA = DomainServices.SalesArrangementService.Contracts;
using _Offer = DomainServices.OfferService.Contracts;
using CIS.Foms.Enums;

namespace FOMS.Api.Notifications.Handlers;

/// <summary>
/// Zalozi RiskBusinessCase pro dany Sales Arrangement
/// </summary>
internal class CreateRiskBusinessCaseHandler
    : INotificationHandler<MainCustomerUpdatedNotification>
{
    public async Task Handle(MainCustomerUpdatedNotification notification, CancellationToken cancellationToken)
    {
        if (!notification.NewMpCustomerId.HasValue) return;

        // case
        var caseInstance = ServiceCallResult.ResolveAndThrowIfError<_Case.Case>(await _caseService.GetCaseDetail(notification.CaseId, cancellationToken));
        //SA
        var saInstance = ServiceCallResult.ResolveAndThrowIfError<_SA.SalesArrangement>(await _salesArrangementService.GetSalesArrangement(notification.SalesArrangementId, cancellationToken));
        // offer
        if (!saInstance.OfferId.HasValue)
            throw new CisNotFoundException(0, "SA does not have Offer bound to it");
        var offerInstance = ServiceCallResult.ResolveAndThrowIfError<_Offer.GetMortgageOfferResponse>(await _offerService.GetMortgageOffer(saInstance.OfferId!.Value, cancellationToken));
        // household
        var households = ServiceCallResult.ResolveAndThrowIfError<List<_SA.Household>>(await _householdService.GetHouseholdList(notification.SalesArrangementId, cancellationToken));

        // ziskat segment
        string riskSegment = await getRiskSegment(
            notification.SalesArrangementId, 
            households.First(t => t.HouseholdTypeId == (int)HouseholdTypes.Main).HouseholdId, 
            caseInstance.Data.ProductTypeId, 
            offerInstance.SimulationInputs.LoanKindId,
            notification.CustomerOnSAId,
            notification.NewMpCustomerId.Value);
        
        // get rbcId
        string riskBusinessId = ServiceCallResult.ResolveAndThrowIfError<string>(await _ripClient.CreateRiskBusinesCase(notification.SalesArrangementId, offerInstance.ResourceProcessId));
        
    }

    async Task<string> getRiskSegment(int salesArrangementId, int householdId, int productTypeId, int loanKindId, int customerOnSAId, int mpId)
    {
        var loanApplicationRequest = new ExternalServices.Rip.V1.RipWrapper.LoanApplicationRequest
        {
            Id = new ExternalServices.Rip.V1.RipWrapper.SystemId
            {
                Id = salesArrangementId.ToString(),
                System = "NOBY"
            },
            LoanApplicationDataVersion = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ"),
            LoanApplicationHousehold = new List<ExternalServices.Rip.V1.RipWrapper.LoanApplicationHousehold2>
            {
                new ExternalServices.Rip.V1.RipWrapper.LoanApplicationHousehold2
                {
                    Id = householdId,
                    CounterParty = new List<ExternalServices.Rip.V1.RipWrapper.LoanApplicationCounterParty2>
                    {
                        new ExternalServices.Rip.V1.RipWrapper.LoanApplicationCounterParty2
                        {
                            Id = customerOnSAId,
                            CustomerId = mpId.ToString(),
                            RoleCodeMp = (int)CustomerRoles.Debtor
                        }
                    }
                },
            },
            LoanApplicationProduct = new ExternalServices.Rip.V1.RipWrapper.LoanApplicationProduct2
            {
                Product = productTypeId,
                LoanType = loanKindId
            }
        };
        return ServiceCallResult.ResolveAndThrowIfError<string>(await _ripClient.CreateLoanApplication(loanApplicationRequest));
    }

    private readonly ExternalServices.Rip.V1.IRipClient _ripClient;
    private readonly IHouseholdServiceAbstraction _householdService;
    private readonly IOfferServiceAbstraction _offerService;
    private readonly ICaseServiceAbstraction _caseService;
    private readonly ISalesArrangementServiceAbstraction _salesArrangementService;

    public CreateRiskBusinessCaseHandler(
        ExternalServices.Rip.V1.IRipClient ripClient,
        IHouseholdServiceAbstraction householdService,
        IOfferServiceAbstraction offerService,
        ICaseServiceAbstraction caseService,
        ISalesArrangementServiceAbstraction salesArrangementService)
    {
        _ripClient = ripClient;
        _householdService = householdService;
        _offerService = offerService;
        _caseService = caseService;
        _salesArrangementService = salesArrangementService;
    }
}
