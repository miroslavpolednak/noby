using DomainServices.SalesArrangementService.Abstraction;
using DomainServices.OfferService.Abstraction;
using DomainServices.CaseService.Abstraction;
using _Case = DomainServices.CaseService.Contracts;
using _SA = DomainServices.SalesArrangementService.Contracts;
using _HO = DomainServices.HouseholdService.Contracts;
using _Offer = DomainServices.OfferService.Contracts;
using CIS.Foms.Enums;
using DomainServices.HouseholdService.Clients;

namespace FOMS.Api.Notifications.Handlers;

/// <summary>
/// Zalozi RiskBusinessCase pro dany Sales Arrangement
/// </summary>
internal class CreateRiskBusinessCaseHandler
    : INotificationHandler<MainCustomerUpdatedNotification>
{
    public async Task Handle(MainCustomerUpdatedNotification notification, CancellationToken cancellationToken)
    {
        long? mpId = notification.CustomerIdentifiers?.FirstOrDefault(t => t.IdentityScheme == CIS.Infrastructure.gRPC.CisTypes.Identity.Types.IdentitySchemes.Mp)?.IdentityId;
        long? kbId = notification.CustomerIdentifiers?.FirstOrDefault(t => t.IdentityScheme == CIS.Infrastructure.gRPC.CisTypes.Identity.Types.IdentitySchemes.Kb)?.IdentityId;
        if (!mpId.HasValue || !kbId.HasValue)
        {
            _logger.LogInformation($"CreateProductHandler for CaseId #{notification.CaseId} not proceeding / missing MP ID");
            return; // nema modre ID, nezajima me
        }

        //SA
        var saInstance = ServiceCallResult.ResolveAndThrowIfError<_SA.SalesArrangement>(await _salesArrangementService.GetSalesArrangement(notification.SalesArrangementId, cancellationToken));
        if (!string.IsNullOrEmpty(saInstance.RiskBusinessCaseId)) // RBCID je jiz zalozene, ukonci flow
        {
            _logger.LogInformation($"SalesArrangement #{notification.SalesArrangementId} already contains RiskBusinessCaseId");
            return;
        }

        // case
        var caseInstance = ServiceCallResult.ResolveAndThrowIfError<_Case.Case>(await _caseService.GetCaseDetail(notification.CaseId, cancellationToken));
        // offer
        if (!saInstance.OfferId.HasValue)
            throw new CisNotFoundException(0, "SA does not have Offer bound to it");
        var offerInstance = ServiceCallResult.ResolveAndThrowIfError<_Offer.GetMortgageOfferResponse>(await _offerService.GetMortgageOffer(saInstance.OfferId!.Value, cancellationToken));
        // household
        var households = ServiceCallResult.ResolveAndThrowIfError<List<_HO.Household>>(await _householdService.GetHouseholdList(notification.SalesArrangementId, cancellationToken));
        if (!households.Any())
            throw new CisValidationException("CreateRiskBusinessCase: household does not exist");

        // ziskat segment
        string riskSegment = await getRiskSegment();

        bool updated1 = ServiceCallResult.Resolve(await _salesArrangementService.UpdateLoanAssessmentParameters(notification.SalesArrangementId, null, riskSegment, null, saInstance.RiskBusinessCaseExpirationDate, cancellationToken));

        // get rbcId
        var createRBCResponse = ServiceCallResult.ResolveAndThrowIfError<DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2.RiskBusinessCaseCreateResponse>(await _riskBusinessCaseService.CreateCase(notification.SalesArrangementId, offerInstance.ResourceProcessId, cancellationToken));

        // ulozit na SA
        bool updated2 = ServiceCallResult.Resolve(await _salesArrangementService.UpdateSalesArrangement(notification.SalesArrangementId, null, createRBCResponse.RiskBusinessCaseId, null, cancellationToken));

        #region local fce
        async Task<string> getRiskSegment()
        {
            var loanApplicationRequest = new DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2.LoanApplicationSaveRequest
            {
                SalesArrangementId = notification.SalesArrangementId,
                LoanApplicationDataVersion = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ", System.Globalization.CultureInfo.InvariantCulture),
                Households = new()
            {
                new()
                {
                    HouseholdId = households.First(t => t.HouseholdTypeId == (int)HouseholdTypes.Main).HouseholdId,
                    Customers = new()
                    {
                        new DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2.LoanApplicationCustomer
                        {
                            InternalCustomerId = notification.CustomerOnSAId,
                            PrimaryCustomerId = kbId.Value.ToString(System.Globalization.CultureInfo.InvariantCulture),
                            CustomerRoleId = (int)CustomerRoles.Debtor
                        }
                    }
                },
            },
                Product = new()
                {
                    ProductTypeId = caseInstance.Data.ProductTypeId,
                    LoanKindId = offerInstance.SimulationInputs.LoanKindId
                }
            };

            return ServiceCallResult.ResolveAndThrowIfError<string>(await _loanApplicationService.Save(loanApplicationRequest, cancellationToken));
        }
        #endregion local fce
    }

    private readonly IHouseholdServiceClient _householdService;
    private readonly IOfferServiceAbstraction _offerService;
    private readonly ICaseServiceAbstraction _caseService;
    private readonly ISalesArrangementServiceAbstraction _salesArrangementService;
    private readonly DomainServices.RiskIntegrationService.Clients.LoanApplication.V2.ILoanApplicationServiceClient _loanApplicationService;
    private readonly DomainServices.RiskIntegrationService.Clients.RiskBusinessCase.V2.IRiskBusinessCaseServiceClient _riskBusinessCaseService;
    private readonly ILogger<CreateRiskBusinessCaseHandler> _logger;

    public CreateRiskBusinessCaseHandler(
        ILogger<CreateRiskBusinessCaseHandler> logger,
        DomainServices.RiskIntegrationService.Clients.LoanApplication.V2.ILoanApplicationServiceClient loanApplicationService,
        DomainServices.RiskIntegrationService.Clients.RiskBusinessCase.V2.IRiskBusinessCaseServiceClient riskBusinessCaseService,
        IHouseholdServiceClient householdService,
        IOfferServiceAbstraction offerService,
        ICaseServiceAbstraction caseService,
        ISalesArrangementServiceAbstraction salesArrangementService)
    {
        _logger = logger;
        _loanApplicationService = loanApplicationService;
        _riskBusinessCaseService = riskBusinessCaseService;
        _householdService = householdService;
        _offerService = offerService;
        _caseService = caseService;
        _salesArrangementService = salesArrangementService;
    }
}
