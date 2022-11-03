using _Pr = DomainServices.ProductService.Contracts;

namespace NOBY.Api.Endpoints.Cases.CreateSalesArrangement.Services;

internal class DrawingValidator
    : BaseValidator, ICreateSalesArrangementParametersValidator
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DrawingValidator(ILogger<CreateSalesArrangementParametersFactory> logger, DomainServices.SalesArrangementService.Contracts.CreateSalesArrangementRequest request, IHttpContextAccessor httpContextAccessor)
        : base(logger, request)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <remarks>
    /// https://wiki.kb.cz/pages/viewpage.action?pageId=424119307#id-%C4%8Cerp%C3%A1ni%C5%BD%C3%A1dosto%C4%8Derp%C3%A1n%C3%AD-Kontrolyp%C5%99edformul%C3%A1%C5%99em%C4%8Derp%C3%A1n%C3%AD
    /// </remarks>
    public async Task<ICreateSalesArrangementParametersBuilder> Validate(CancellationToken cancellationToken = default(CancellationToken))
    {
        var productService = _httpContextAccessor.HttpContext!.RequestServices.GetRequiredService<DomainServices.ProductService.Abstraction.IProductServiceAbstraction>();
        // instance hypo
        var productInstance = ServiceCallResult.ResolveAndThrowIfError<_Pr.GetMortgageResponse>(await productService.GetMortgage(_request.CaseId, cancellationToken));

        if (productInstance.Mortgage.AvailableForDrawing <= 0M)
            throw new CisValidationException("Zůstatek pro čerpání je menší nebo rovný nule. Formulář nelze vytvořit");
        if (productInstance.Mortgage.FirstAnnuityPaymentDate != null && DateTime.Now >= productInstance.Mortgage.FirstAnnuityPaymentDate)
            throw new CisValidationException("Aktuální datum překračuje datum první anuitní splátky. Formulář nelze vytvořit");
        if (string.IsNullOrEmpty(productInstance.Mortgage.PaymentAccount?.Number))
            throw new CisValidationException("Neexistuje úvěrový účet. Formulář nelze vytvořit");

        return new DrawingBuilder(_logger, _request, _httpContextAccessor);
    }
}
