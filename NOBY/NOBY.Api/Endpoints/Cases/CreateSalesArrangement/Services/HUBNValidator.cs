using NOBY.Api.Endpoints.Cases.CreateSalesArrangement.Services.Internals;

namespace NOBY.Api.Endpoints.Cases.CreateSalesArrangement.Services;

internal sealed class HUBNValidator
    : BaseValidator, ICreateSalesArrangementParametersValidator
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HUBNValidator(ILogger<CreateSalesArrangementParametersFactory> logger, DomainServices.SalesArrangementService.Contracts.CreateSalesArrangementRequest request, IHttpContextAccessor httpContextAccessor)
        : base(logger, request)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ICreateSalesArrangementParametersBuilder> Validate(CancellationToken cancellationToken = default(CancellationToken))
    {
        var productService = _httpContextAccessor.HttpContext!.RequestServices.GetRequiredService<DomainServices.ProductService.Clients.IProductServiceClient>();
        // instance hypo
        var productInstance = await productService.GetMortgage(_request.CaseId, cancellationToken);

        if (productInstance.Mortgage?.ContractSignedDate is null)
        {
            throw new NobyValidationException(90014);
        }

        return new HUBNBuilder(_logger, _request, _httpContextAccessor);
    }
}
