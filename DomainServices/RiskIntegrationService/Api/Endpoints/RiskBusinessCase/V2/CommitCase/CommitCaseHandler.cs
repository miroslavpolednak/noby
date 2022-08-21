using _V2 = DomainServices.RiskIntegrationService.Contracts.RiskBusinessCase.V2;
using _C4M = DomainServices.RiskIntegrationService.Api.Clients.RiskBusinessCase.V0_2.Contracts;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.RiskBusinessCase.V2.CommitCase;

internal sealed class CommitCaseHandler
    : IRequestHandler<_V2.RiskBusinessCaseCommitCaseRequest, _V2.RiskBusinessCaseCommitCaseResponse>
{
    public async Task<_V2.RiskBusinessCaseCommitCaseResponse> Handle(_V2.RiskBusinessCaseCommitCaseRequest request, CancellationToken cancellationToken)
    {
        string chanel = _configuration.GetItChannelFromServiceUser(_serviceUserAccessor.User!.Name);
        var riskApplicationType = Helpers.GetRiskApplicationType(await _codebookService.RiskApplicationTypes(cancellationToken), request.ProductTypeId);

        var requestModel = new _C4M.CommitRequest
        {
            LoanApplicationId = _C4M.ResourceIdentifier.CreateLoanApplication(request.SalesArrangementId, chanel),
            ItChannel = FastEnum.Parse<_C4M.CommitRequestItChannel>(chanel, true),
            LoanApplicationProduct = new _C4M.LoanApplicationProduct()
            {
                ProductClusterCode = riskApplicationType!.C4mAplCode
            },
            LoanSoldProduct = new _C4M.LoanSoldProduct
            {
                Id = request.SoldProduct != null ? _C4M.ResourceIdentifier.CreateLoanSoldProduct(request.SoldProduct.Id, request.SoldProduct.Company, chanel) : null
            },
            ApprovalDate = request.ApprovalDate,
            CollateralAgreements = request.CollateralAgreementsId?.Select(t => new _C4M.CollateralAgreement
            {
                Id = _C4M.ResourceIdentifier.CreateCollateralAgreement($"{t}")
            }).ToList()
        };

        // human user instance
        var userInstance = await _xxvConnectionProvider.GetC4mUserInfo(request.UserIdentity, cancellationToken);
        if (Helpers.IsDealerSchema(request.UserIdentity!.IdentityScheme))
            requestModel.LoanApplicationDealer = _C4M.C4mUserInfoDataExtensions.ToC4mDealer(userInstance, request.UserIdentity);
        else
            requestModel.Creator = _C4M.C4mUserInfoDataExtensions.ToC4mPerson(userInstance, request.UserIdentity);

        // approver
        if (request.Approver != null)
        {
            var approverInstance = await _xxvConnectionProvider.GetC4mUserInfo(request.Approver, cancellationToken);
            if (Helpers.IsDealerSchema(request.UserIdentity!.IdentityScheme))
                throw new CisValidationException(0, $"Approver can't be dealer.");
            else
                requestModel.Approver = _C4M.C4mUserInfoDataExtensions.ToC4mPerson(userInstance, request.UserIdentity);
        }

        //C4M
        var response = await _client.CommitCase(request.RiskBusinessCaseId, requestModel, cancellationToken);

        return new _V2.RiskBusinessCaseCommitCaseResponse
        {
            RiskBusinessCaseId = response.RiskBusinessCaseId.Id,
            Timestamp = response.Timestamp!.DateTime,
            OperationResult = response.OperationResult,
            ResultReasons = response.ResultReasons?.Select(t => new Contracts.Shared.ResultReasonDetail
            {
                Code = t.Code,
                Description = t.Desc
            }).ToList()
        };
    }

    private readonly Clients.RiskBusinessCase.V0_2.IRiskBusinessCaseClient _client;
    private readonly AppConfiguration _configuration;
    private readonly CIS.Core.Security.IServiceUserAccessor _serviceUserAccessor;
    private readonly CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;
    private readonly CIS.Core.Data.IConnectionProvider<IXxvDapperConnectionProvider> _xxvConnectionProvider;

    public CommitCaseHandler(
        AppConfiguration configuration,
        CIS.Core.Security.IServiceUserAccessor serviceUserAccessor,
        Clients.RiskBusinessCase.V0_2.IRiskBusinessCaseClient client,
        CodebookService.Abstraction.ICodebookServiceAbstraction codebookService,
        CIS.Core.Data.IConnectionProvider<IXxvDapperConnectionProvider> xxvConnectionProvider)
    {
        _serviceUserAccessor = serviceUserAccessor;
        _configuration = configuration;
        _client = client;
        _codebookService = codebookService;
        _xxvConnectionProvider = xxvConnectionProvider;
    }
}
