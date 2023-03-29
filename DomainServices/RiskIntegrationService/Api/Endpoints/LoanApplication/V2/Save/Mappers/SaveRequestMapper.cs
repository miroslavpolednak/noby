using _V2 = DomainServices.RiskIntegrationService.Contracts.LoanApplication.V2;
using _C4M = DomainServices.RiskIntegrationService.ExternalServices.LoanApplication.V1.Contracts;
using _RAT = DomainServices.CodebookService.Contracts.Endpoints.RiskApplicationTypes;
using CIS.Core.Security;
using DomainServices.RiskIntegrationService.ExternalServices.LoanApplication.V1.Contracts;
using CIS.Core.Configuration;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.LoanApplication.V2.Save.Mappers;

[CIS.Core.Attributes.ScopedService, CIS.Core.Attributes.SelfService]
internal sealed class SaveRequestMapper
{
    public async Task<_C4M.LoanApplication> MapToC4m(_V2.LoanApplicationSaveRequest request, CancellationToken cancellation)
    {
        // neprislo LTV, zkus ho spocitat
        if (request.Product.Ltv.GetValueOrDefault() == 0 && request.Product.RequiredAmount.GetValueOrDefault() > 0)
        {
            decimal totalAppraised = request.Product.Collaterals?.Select(t => t.AppraisedValue?.Amount ?? 0).Sum() ?? 0;
            if (totalAppraised > 0)
                request.Product.Ltv = (request.Product.RequiredAmount!.Value / totalAppraised) * 100M;
        }
        request.Product.Ltv ??= 0;

        // produkt
        var riskApplicationType = await getRiskApplicationType(request.Product, cancellation) ?? throw new CisValidationException(17009, $"Can't find RiskApplicationType item");

        bool verification = riskApplicationType.MandantId == (int)CIS.Foms.Enums.Mandants.Kb
            && request.Product.RequiredAmount <= 9000000
            && (request.Product.Purposes?.All(p => p.LoanPurposeId == 201 || p.LoanPurposeId == 202) ?? false)
            && request.Households.Count == 1
            && request.Households.All(t => t.Customers?.All(c => c.IdentificationDocument?.IdentificationDocumentTypeId == 1) ?? false)
            && request.Households.All(t => t.Customers?.All(c => c.Income?.EmploymentIncomes?.Count == 1) ?? false)
            && request.Households.All(t => t.Customers?.All(c => c.Income?.OtherIncomes is null || !c.Income.OtherIncomes.Any() || c.Income.OtherIncomes.All(z => z.MonthlyIncomeAmount is null || z.MonthlyIncomeAmount.Amount == 0)) ?? true)
            && request.Households.All(t => t.Customers?.All(c => !c.Income?.OtherIncomes?.Any() ?? true) ?? true)
            && request.Households.All(t => t.Customers?.All(c => c.Income?.EntrepreneurIncome?.AnnualIncomeAmount is null) ?? true)
            && request.Households.All(t => t.Customers?.All(c => c.Income?.RentIncome?.MonthlyIncomeAmount is null) ?? true)
            && request.Households.All(t => t.Customers?.All(c => c.Income?.EmploymentIncomes?.First().EmploymentTypeId == 2) ?? false)
            && request.Households.All(t => t.Customers?.All(c => c.Income?.EmploymentIncomes?.First().Address?.CountryId == 16) ?? false);

        // mappers instances
        var productChildMapper = new ProductChildMapper(_codebookService, riskApplicationType, cancellation);
        var householdMapper = new HouseholdChildMapper(_codebookService, riskApplicationType, cancellation);

        var requestModel = new _C4M.LoanApplication
        {
            Id = _C4M.ResourceIdentifier.CreateId(request.SalesArrangementId.ToEnvironmentId(_cisEnvironment.EnvironmentName!), _configuration.GetItChannelFromServiceUser(_serviceUserAccessor.User!.Name!)),
            AppendixCode = request.AppendixCode,
            DistributionChannelCode = Helpers.GetEnumFromString<_C4M.LoanApplicationDistributionChannelCode>((await _codebookService.Channels(cancellation)).FirstOrDefault(t => t.Id == request.DistributionChannelId)?.Code, LoanApplicationDistributionChannelCode.BR),
            LoanApplicationDataVersion = request.LoanApplicationDataVersion,
            LoanApplicationHousehold = await householdMapper.MapHouseholds(request.Households, verification),
            LoanApplicationProduct = await productChildMapper.MapProduct(request.Product),
            LoanApplicationProductRelation = await productChildMapper.MapProductRelations(request.ProductRelations),
            LoanApplicationDeclaredProductRelation = productChildMapper.MapDeclaredProductRelations(request.DeclaredSecuredProducts)
        };
        if (request.SignatureType != CIS.Foms.Enums.SignatureTypes.Unknown)
            requestModel.SignatureType = request.SignatureType.ToString();

        // human user instance
        if (request.UserIdentity is not null)
        {
            var userInstance = await _xxvConnectionProvider.GetC4mUserInfo(request.UserIdentity, cancellation);
            if (Helpers.IsDealerSchema(userInstance.DealerCompanyId))
                requestModel.LoanApplicationDealer = _C4M.C4mUserInfoDataExtensions.ToC4mDealer(userInstance, request.UserIdentity);
            else
                requestModel.Person = _C4M.C4mUserInfoDataExtensions.ToC4mPerson(userInstance, request.UserIdentity);
        }

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

    private readonly CIS.Core.Data.IConnectionProvider<Data.IXxvDapperConnectionProvider> _xxvConnectionProvider;
    private readonly CodebookService.Clients.ICodebookServiceClients _codebookService;
    private readonly IServiceUserAccessor _serviceUserAccessor;
    private readonly AppConfiguration _configuration;
    private readonly ICisEnvironmentConfiguration _cisEnvironment;

    public SaveRequestMapper(
        AppConfiguration configuration,
        IServiceUserAccessor serviceUserAccessor,
        CIS.Core.Data.IConnectionProvider<Data.IXxvDapperConnectionProvider> xxvConnectionProvider,
        CodebookService.Clients.ICodebookServiceClients codebookService,
        ICisEnvironmentConfiguration cisEnvironment)
    {
        _configuration = configuration;
        _serviceUserAccessor = serviceUserAccessor;
        _codebookService = codebookService;
        _xxvConnectionProvider = xxvConnectionProvider;
        _cisEnvironment = cisEnvironment;
    }
}
