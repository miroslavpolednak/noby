using _V2 = DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;
using _C4M = DomainServices.RiskIntegrationService.Api.Clients.LoanApplication.V1.Contracts;
using _RAT = DomainServices.CodebookService.Contracts.Endpoints.RiskApplicationTypes;
using Azure.Core;
using CIS.Core.Security;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.LoanApplication.V2.Save.Mappers;

internal sealed class SaveRequestMapper
{
    public async Task<_C4M.LoanApplication> MapToC4m(_V2.LoanApplicationSaveRequest request, CancellationToken cancellation)
    {
        // distr channel
        var distrChannel = (await _codebookService.Channels(cancellation)).FirstOrDefault(t => t.Id == request.DistributionChannelId)?.Code ?? "BR";
        if (FastEnum.TryParse(distrChannel, out _C4M.LoanApplicationDistributionChannelCode distrChannelEnumValue))
            throw new CisValidationException(0, $"Can't cast DistributionChannelId '{request.DistributionChannelId}' to C4M enum");

        // produkt
        var riskApplicationType = await getRiskApplicationType(request.Product, cancellation) ?? throw new CisValidationException(0, $"Can't find RiskApplicationType item");

        // mappers instances
        var productChildMapper = new ProductChildMapper(_codebookService, riskApplicationType, cancellation);

        var requestModel = new _C4M.LoanApplication
        {
            Id = _C4M.ResourceIdentifier.CreateId(request.SalesArrangementId, _configuration.GetItChannelFromServiceUser(_serviceUserAccessor.User!.Name)),
            AppendixCode = request.AppendixCode,
            DistributionChannelCode = distrChannelEnumValue,
            SignatureType = request.SignatureType.ToString(),
            LoanApplicationDataVersion = request.LoanApplicationDataVersion,
            LoanApplicationHousehold = await request.Households?.ToC4m(riskApplicationType, _codebookService, cancellation),
            LoanApplicationProduct = await productChildMapper.MapProduct(request.Product),
            LoanApplicationProductRelation = await productChildMapper.MapProductRelations(request.ProductRelations),
            LoanApplicationDeclaredProductRelation = productChildMapper.MapDeclaredProductRelations(request.DeclaredSecuredProducts)
        };
        
        // human user instance
        var userInstance = await _xxvConnectionProvider.GetC4mUserInfo(request.UserIdentity, cancellation);
        if (Helpers.IsDealerSchema(request.UserIdentity!.IdentityScheme))
            requestModel.LoanApplicationDealer = _C4M.C4mUserInfoDataExtensions.ToC4mDealer(userInstance, request.UserIdentity);
        else
            requestModel.Person = _C4M.C4mUserInfoDataExtensions.ToC4mPerson(userInstance, request.UserIdentity);

        return requestModel;
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
                && product.Ltv <= (t.LtvTo ?? int.MaxValue) && product.Ltv >= (t.LtvFrom ?? 0)
            )
            .ToList();

        // MA
        bool requestContainsMa = product.MarketingActions?.Any() ?? false;
        var productsFromMA = products
            .Where(t =>
                // v req neni MA, hledam jen v items bez MA
                !requestContainsMa && !(t.MarketingActions?.Any() ?? false)
                || requestContainsMa && (t.MarketingActions?.Any(x => product.MarketingActions!.Contains(x)) ?? false)
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
    private readonly CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;
    private readonly IServiceUserAccessor _serviceUserAccessor;
    private readonly AppConfiguration _configuration;

    public SaveRequestMapper(
        AppConfiguration configuration,
        IServiceUserAccessor serviceUserAccessor,
        CIS.Core.Data.IConnectionProvider<IXxvDapperConnectionProvider> xxvConnectionProvider,
        CodebookService.Abstraction.ICodebookServiceAbstraction codebookService)
    {
        _configuration = configuration;
        _serviceUserAccessor = serviceUserAccessor;
        _codebookService = codebookService;
        _xxvConnectionProvider = xxvConnectionProvider;
    }
}
