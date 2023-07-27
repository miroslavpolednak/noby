using CIS.Foms.Types.Enums;
using NOBY.Api.Endpoints.Cases.CreateSalesArrangement.Services.Internals;

namespace NOBY.Api.Endpoints.Cases.CreateSalesArrangement.Services;

internal sealed class CustomerChange3602CValidator
    : BaseValidator, ICreateSalesArrangementParametersValidator
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CustomerChange3602CValidator(ILogger<CreateSalesArrangementParametersFactory> logger, DomainServices.SalesArrangementService.Contracts.CreateSalesArrangementRequest request, IHttpContextAccessor httpContextAccessor)
        : base(logger, request)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ICreateSalesArrangementParametersBuilder> Validate(CancellationToken cancellationToken = default(CancellationToken))
    {
        var productService = _httpContextAccessor.HttpContext!.RequestServices.GetRequiredService<DomainServices.ProductService.Clients.IProductServiceClient>();
        var salesArrangementService = _httpContextAccessor.HttpContext!.RequestServices.GetRequiredService<DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClient>();

        // instance hypo
        var productInstance = await productService.GetMortgage(_request.CaseId, cancellationToken);

        if (productInstance.Mortgage?.ContractSignedDate is null)
        {
            throw new NobyValidationException(90014);
        }

        // neexistuje SAType=9 v Case
        var salesArrangementsForCase = await salesArrangementService.GetSalesArrangementList(_request.CaseId, cancellationToken);
        if (!salesArrangementsForCase.SalesArrangements.Any(t => t.SalesArrangementTypeId == (int)SalesArrangementTypes.CustomerChange))
        {
            throw new NobyValidationException(90015);
        }

        return new CustomerChange3602CBuilder(_logger, _request, _httpContextAccessor);
    }
}
