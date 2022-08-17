using _V2 = DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;
using _C4M = DomainServices.RiskIntegrationService.Api.Clients.LoanApplication.V1.Contracts;
using _RAT = DomainServices.CodebookService.Contracts.Endpoints.RiskApplicationTypes;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.LoanApplication.V2.Save;

internal sealed class SaveHandler
    : IRequestHandler<_V2.LoanApplicationSaveRequest, _V2.LoanApplicationSaveResponse>
{
    public async Task<_V2.LoanApplicationSaveResponse> Handle(_V2.LoanApplicationSaveRequest request, CancellationToken cancellation)
    {
        // distr channel
        var distrChannel = (await _codebookService.Channels(cancellation)).FirstOrDefault(t => t.Id == request.DistributionChannelId)?.Code ?? "BR";
        if (FastEnum.TryParse(distrChannel, out _C4M.LoanApplicationDistributionChannelCode distrChannelEnumValue))
            throw new CisValidationException(0, $"Can't cast DistributionChannelId '{request.DistributionChannelId}' to C4M enum");

        // produkt
        var riskApplicationType = await getRiskApplicationType(request.Product, cancellation) ?? throw new CisValidationException(0, $"Can't find RiskApplicationType item");

        var requestModel = new _C4M.LoanApplication
        {
            Id = _C4M.ResourceIdentifier.CreateId(request.SalesArrangementId, _configuration.GetItChannelFromServiceUser(_serviceUserAccessor.User!.Name)),
            AppendixCode = request.AppendixCode,
            DistributionChannelCode = distrChannelEnumValue,
            SignatureType = request.SignatureType.ToString(),
            LoanApplicationDataVersion = request.LoanApplicationDataVersion,
            LoanApplicationHousehold = null,
            LoanApplicationProduct = await request.Product?.ToC4m(riskApplicationType, _codebookService, cancellation) ?? throw new CisValidationException(0, "Unable to create LoanApplicationProduct"),
            LoanApplicationProductRelation = await request.ProductRelations?.ToC4m(riskApplicationType, _codebookService, cancellation),
            LoanApplicationDeclaredProductRelation = null
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
            //LoanApplicationId = response.Id,//TODO ResourceIdentifier
            LoanApplicationDataVersion = response.LoanApplicationDataVersion,
            RiskSegment = responseVerPriority is null ? _V2.LoanApplicationRiskSegments.B : (responseVerPriority.All(t => t.GetValueOrDefault()) ? _V2.LoanApplicationRiskSegments.A : _V2.LoanApplicationRiskSegments.B)
        };
    }

    // najit odpovidajici produkt
    private async Task<_RAT.RiskApplicationTypeItem?> getRiskApplicationType(_V2.LoanApplicationProduct product, CancellationToken cancellation)
    {
        // product 
        var products = (await _codebookService.RiskApplicationTypes(cancellation))
            .Where(t =>
                // Dle produktu - vždy vyplněn
                t.ProductTypeId is not null && t.ProductTypeId.Contains(product.ProductTypeId)
                // LTV
                && (product.Ltv <= (t.LtvTo ?? int.MaxValue) && product.Ltv >= (t.LtvFrom ?? 0))
            )
            .ToList();

        // MA
        bool requestContainsMa = product.MarketingActions?.Any() ?? false;
        var productsFromMA = products
            .Where(t =>
                // v req neni MA, hledam jen v items bez MA
                (!requestContainsMa && !(t.MarketingActions?.Any() ?? false))
                || (requestContainsMa && (t.MarketingActions?.Any(x => product.MarketingActions!.Contains(x)) ?? false))
            )
            .ToList();
        if (requestContainsMa && !productsFromMA.Any())
            productsFromMA = products.Where(t => !(t.MarketingActions?.Any() ?? false)).ToList();

        // Druh uveru
        var productsFromLoanKind = productsFromMA.Where(t => t.LoanKindId == product.LoanKindId).ToList();
        if (!productsFromLoanKind.Any())
            productsFromLoanKind = productsFromMA.Where(t => !t.LoanKindId.HasValue).ToList();

        //TODO tady vzit jen prvni?
        return productsFromLoanKind.FirstOrDefault();
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
