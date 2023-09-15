using __SA = DomainServices.SalesArrangementService.Contracts;

namespace NOBY.Api.Endpoints.Cases.CreateSalesArrangement.Services.Internals;

internal abstract class BaseBuilder
    : ICreateSalesArrangementParametersBuilder
{
    protected readonly IHttpContextAccessor _httpContextAccessor;
    protected readonly __SA.CreateSalesArrangementRequest _request;
    protected readonly ILogger<CreateSalesArrangementParametersFactory> _logger;

    public BaseBuilder(ILogger<CreateSalesArrangementParametersFactory> logger, __SA.CreateSalesArrangementRequest request, IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
        _request = request;
    }

    public virtual Task<__SA.CreateSalesArrangementRequest> UpdateParameters(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_request);
    }

    public virtual Task PostCreateProcessing(int salesArrangementId, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
