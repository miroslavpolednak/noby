using DomainServices.SalesArrangementService.Abstraction;
using DomainServices.OfferService.Abstraction;
using DomainServices.CaseService.Abstraction;
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
        if (!households.Any())
            throw new CisValidationException("CreateRiskBusinessCase: household does not exist");

        // ziskat segment
        string riskSegment = await getRiskSegment(
            notification.SalesArrangementId, 
            households.First(t => t.HouseholdTypeId == (int)HouseholdTypes.Main).HouseholdId, 
            caseInstance.Data.ProductTypeId, 
            offerInstance.SimulationInputs.LoanKindId,
            notification.CustomerOnSAId,
            notification.NewMpCustomerId.Value,
            cancellationToken);

        bool updated1 = ServiceCallResult.Resolve(await _salesArrangementService.UpdateLoanAssessmentParameters(notification.SalesArrangementId, null, riskSegment, null, cancellationToken));

        // get rbcId
        var createRBCResponse = ServiceCallResult.ResolveAndThrowIfError<DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2.RiskBusinessCaseCreateResponse>(await _riskBusinessCaseService.CreateCase(notification.SalesArrangementId, offerInstance.ResourceProcessId, cancellationToken));
        
        // ulozit na SA
        bool updated2 = ServiceCallResult.Resolve(await _salesArrangementService.UpdateSalesArrangement(notification.SalesArrangementId, null, createRBCResponse.RiskBusinessCaseId, null, cancellationToken));
    }

    async Task<string> getRiskSegment(int salesArrangementId, int householdId, int productTypeId, int loanKindId, int customerOnSAId, long mpId, CancellationToken cancellationToken)
    {
        var loanApplicationRequest = new DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2.LoanApplicationSaveRequest
        {
            SalesArrangementId = salesArrangementId,
            LoanApplicationDataVersion = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ"),
            Households = new()
            {
                new()
                {
                    HouseholdId = householdId,
                    Customers = new()
                    {
                        new DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2.LoanApplicationCustomer
                        {
                            InternalCustomerId = customerOnSAId,
                            PrimaryCustomerId = mpId.ToString(),
                            CustomerRoleId = (int)CustomerRoles.Debtor
                        }
                    }
                },
            },
            Product = new()
            {
                ProductTypeId = productTypeId,
                LoanKindId = loanKindId
            }
        };
        
        return ServiceCallResult.ResolveAndThrowIfError<string>(await _loanApplicationService.Save(loanApplicationRequest, cancellationToken));
    }

    private readonly IHouseholdServiceAbstraction _householdService;
    private readonly IOfferServiceAbstraction _offerService;
    private readonly ICaseServiceAbstraction _caseService;
    private readonly ISalesArrangementServiceAbstraction _salesArrangementService;
    private readonly DomainServices.RiskIntegrationService.Abstraction.LoanApplication.V2.ILoanApplicationServiceAbstraction _loanApplicationService;
    private readonly DomainServices.RiskIntegrationService.Abstraction.RiskBusinessCase.V2.IRiskBusinessCaseServiceAbstraction _riskBusinessCaseService;

    public CreateRiskBusinessCaseHandler(
        DomainServices.RiskIntegrationService.Abstraction.LoanApplication.V2.ILoanApplicationServiceAbstraction loanApplicationService,
        DomainServices.RiskIntegrationService.Abstraction.RiskBusinessCase.V2.IRiskBusinessCaseServiceAbstraction riskBusinessCaseService,
        IHouseholdServiceAbstraction householdService,
        IOfferServiceAbstraction offerService,
        ICaseServiceAbstraction caseService,
        ISalesArrangementServiceAbstraction salesArrangementService)
    {
        _loanApplicationService = loanApplicationService;
        _riskBusinessCaseService = riskBusinessCaseService;
        _householdService = householdService;
        _offerService = offerService;
        _caseService = caseService;
        _salesArrangementService = salesArrangementService;
    }
}
