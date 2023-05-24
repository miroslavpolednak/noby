using DomainServices.CaseService.Clients;
using DomainServices.ProductService.Clients;
using NOBY.Api.Endpoints.Cases.CreateSalesArrangement.Services.Internals;

namespace NOBY.Api.Endpoints.Cases.CreateSalesArrangement.Services;

internal sealed class CustomerChange3602BValidator
    : BaseValidator, ICreateSalesArrangementParametersValidator
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CustomerChange3602BValidator(ILogger<CreateSalesArrangementParametersFactory> logger, DomainServices.SalesArrangementService.Contracts.CreateSalesArrangementRequest request, IHttpContextAccessor httpContextAccessor)
        : base(logger, request)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ICreateSalesArrangementParametersBuilder> Validate(CancellationToken cancellationToken = default(CancellationToken))
    {
        var caseService = _httpContextAccessor.HttpContext!.RequestServices.GetRequiredService<ICaseServiceClient>();
        var productService = _httpContextAccessor.HttpContext!.RequestServices.GetRequiredService<IProductServiceClient>();

        var caseInstance = await caseService.GetCaseDetail(_request.CaseId, cancellationToken);
        if (caseInstance.State == 1)
        {
            throw new NobyValidationException("Case state < 2");
        }

        // instance hypo
        var productInstance = await productService.GetMortgage(_request.CaseId, cancellationToken);
        if (productInstance.Mortgage?.ContractSignedDate is null)
        {
            throw new NobyValidationException(90014);
        }

        return new CustomerChange3602BBuilder(_logger, _request, _httpContextAccessor);
    }
}
