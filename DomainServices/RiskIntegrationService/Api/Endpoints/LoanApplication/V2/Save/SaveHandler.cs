using _V2 = DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;
using _C4M = DomainServices.RiskIntegrationService.Api.Clients.LoanApplication.V1.Contracts;
using _Contracts = DomainServices.RiskIntegrationService.Api.Clients.LoanApplication.V1.Contracts;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.LoanApplication.V2.Save;

internal sealed class SaveHandler
    : IRequestHandler<_V2.LoanApplicationSaveRequest, _V2.LoanApplicationSaveResponse>
{
    public async Task<_V2.LoanApplicationSaveResponse> Handle(_V2.LoanApplicationSaveRequest request, CancellationToken cancellation)
    {
        // distr channel
        var distrChannel = (await _codebookService.Channels(cancellation)).FirstOrDefault(t => t.Id == request.DistributionChannelId)?.Code ?? "BR";
        if (FastEnum.TryParse(distrChannel, out _Contracts.LoanApplicationDistributionChannelCode distrChannelEnumValue))
            throw new CisValidationException(0, $"Can't cast DistributionChannelId '{request.DistributionChannelId}' to C4M enum");
        
        var requestModel = new _C4M.LoanApplication
        {
            Id = _C4M.ResourceIdentifier.CreateId(request.CaseId, _configuration.GetItChannelFromServiceUser(_serviceUserAccessor.User!.Name)),
            DistributionChannelCode = distrChannelEnumValue,
            SignatureType = request.SignatureType.ToString(),
            LoanApplicationDataVersion = request.LoanApplicationDataVersion,
            LoanApplicationHousehold = null,
            LoanApplicationProduct = await request.Product?.ToC4m(_codebookService, cancellation) ?? throw new CisValidationException(0, "Unable to create LoanApplicationProduct"),
            //LoanApplicationProductRelation = request.ProductRelations.ToC4m()
        };

        // human user instance
        var userInstance = await _xxvConnectionProvider.GetC4mUserInfo(request.UserIdentity, cancellation);
        if (Helpers.IsDealerSchema(request.UserIdentity!.IdentityScheme))
            requestModel.LoanApplicationDealer = _C4M.C4mUserInfoDataExtensions.ToC4mDealer(userInstance, request.UserIdentity);
        else
            requestModel.Person = _C4M.C4mUserInfoDataExtensions.ToC4mPerson(userInstance, request.UserIdentity);

        // volani c4m
        var response = await _client.Save(requestModel, cancellation);

        var responseVerPriority = requestModel.LoanApplicationHousehold?.SelectMany(t => t.CounterParty.SelectMany(x => x.Income?.EmploymentIncome?.Select(y => y.VerificationPriority))).ToList();
        return new _V2.LoanApplicationSaveResponse
        {
            RiskSegment = responseVerPriority is null ? "B" : (responseVerPriority.All(t => t.GetValueOrDefault()) ? "A" : "B")
        };
    }

    private readonly CIS.Core.Data.IConnectionProvider<IXxvDapperConnectionProvider> _xxvConnectionProvider;
    private readonly Clients.LoanApplication.V1.ILoanApplicationClient _client;
    private readonly CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;
    private readonly AppConfiguration _configuration;
    private readonly CIS.Core.Security.IServiceUserAccessor _serviceUserAccessor;

    public SaveHandler(
        AppConfiguration configuration,
        CIS.Core.Security.IServiceUserAccessor serviceUserAccessor,
        Clients.LoanApplication.V1.ILoanApplicationClient client,
        CIS.Core.Data.IConnectionProvider<IXxvDapperConnectionProvider> xxvConnectionProvider,
        CodebookService.Abstraction.ICodebookServiceAbstraction codebookService)
    {
        _serviceUserAccessor = serviceUserAccessor;
        _configuration = configuration;
        _codebookService = codebookService;
        _xxvConnectionProvider = xxvConnectionProvider;
        _client = client;
    }
}
