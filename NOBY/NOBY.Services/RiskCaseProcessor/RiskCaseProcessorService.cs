using CIS.Core.Security;
using CIS.InternalServices.DataAggregatorService.Contracts;
using DomainServices.OfferService.Clients.v1;
using DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;
using DomainServices.SalesArrangementService.Clients;
using System.Text.Json;
using DomainServices.UserService.Clients;
using CIS.InternalServices.DataAggregatorService.Clients;

namespace NOBY.Services.RiskCaseProcessor;

[TransientService, SelfService]
public sealed class RiskCaseProcessorService
{
    public async Task<(string RiskSegment, string RiskBusinessCaseId, string LoanApplicationDataVersion)> CreateOrUpdateRiskCase(
        int salesArrangementId,
        CancellationToken cancellationToken)
    {
        //SA
        var saInstance = await _salesArrangementService.GetSalesArrangement(salesArrangementId, cancellationToken);
        if (!saInstance.OfferId.HasValue)
        {
            throw new CisValidationException(400002, "CreateRiskBusinessCaseService: SA does not have Offer bound to it");
        }

        // offer
        var offerInstance = await _offerService.ValidateOfferId(saInstance.OfferId!.Value, false, cancellationToken);

        // ziskat segment
        var laResult = await SaveLoanApplication(saInstance.SalesArrangementId, saInstance.CaseId, saInstance.OfferId.Value, cancellationToken);

        // get rbcId
        if (string.IsNullOrWhiteSpace(saInstance.RiskBusinessCaseId))
        {
            // set RBCID
            saInstance.RiskBusinessCaseId = (await _riskBusinessCaseService.CreateCase(salesArrangementId, offerInstance.Data.ResourceProcessId, cancellationToken)).RiskBusinessCaseId;
        }

        // set risk segment
        saInstance.RiskSegment = laResult.RiskSegment;

        return (saInstance.RiskSegment, saInstance.RiskBusinessCaseId, laResult.LoanApplicationDataVersion);
    }

    public async Task<(string RiskSegment, string LoanApplicationDataVersion)> SaveLoanApplication(int salesArrangementId, long caseId, int offerId, CancellationToken cancellationToken)
    {
        var dataRequest = new GetRiskLoanApplicationDataRequest
        {
            SalesArrangementId = salesArrangementId,
            CaseId = caseId,
            OfferId = offerId
        };
        var response = await _dataAggregatorService.GetRiskLoanApplicationData(dataRequest, cancellationToken);

        var loanApplicationSaveRequest = JsonSerializer.Deserialize<LoanApplicationSaveRequest>(response.Json)!;

        var user = await _userService.GetUser(_currentUserAccessor.User!.Id, cancellationToken);
        loanApplicationSaveRequest.UserIdentity = new DomainServices.RiskIntegrationService.Contracts.Shared.Identity
        {
            IdentityId = user.UserIdentifiers[0].Identity,
            IdentityScheme = user.UserIdentifiers[0].IdentityScheme.ToString()
        };

        // set risk segment
        var riskSegment = await _loanApplicationService.Save(loanApplicationSaveRequest, cancellationToken);

        return (riskSegment, loanApplicationSaveRequest.LoanApplicationDataVersion);
    }

    private readonly IDataAggregatorServiceClient _dataAggregatorService;
    private readonly ICurrentUserAccessor _currentUserAccessor;
    private readonly IUserServiceClient _userService;
    private readonly IOfferServiceClient _offerService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly DomainServices.RiskIntegrationService.Clients.LoanApplication.V2.ILoanApplicationServiceClient _loanApplicationService;
    private readonly DomainServices.RiskIntegrationService.Clients.RiskBusinessCase.V2.IRiskBusinessCaseServiceClient _riskBusinessCaseService;

    public RiskCaseProcessorService(
        DomainServices.RiskIntegrationService.Clients.LoanApplication.V2.ILoanApplicationServiceClient loanApplicationService,
        DomainServices.RiskIntegrationService.Clients.RiskBusinessCase.V2.IRiskBusinessCaseServiceClient riskBusinessCaseService,
        IDataAggregatorServiceClient dataAggregatorService,
        ICurrentUserAccessor currentUserAccessor,
        IUserServiceClient userService,
        IOfferServiceClient offerService,
        ISalesArrangementServiceClient salesArrangementService)
    {
        _dataAggregatorService = dataAggregatorService;
        _currentUserAccessor = currentUserAccessor;
        _userService = userService;
        _loanApplicationService = loanApplicationService;
        _riskBusinessCaseService = riskBusinessCaseService;
        _offerService = offerService;
        _salesArrangementService = salesArrangementService;
    }
}
