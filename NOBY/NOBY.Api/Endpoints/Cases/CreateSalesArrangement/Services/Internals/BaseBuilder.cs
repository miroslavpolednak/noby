using __SA = DomainServices.SalesArrangementService.Contracts;

namespace NOBY.Api.Endpoints.Cases.CreateSalesArrangement.Services.Internals;

internal abstract class BaseBuilder
    : ICreateSalesArrangementParametersBuilder
{
    protected DomainServices.SalesArrangementService.Contracts.CreateSalesArrangementRequest Request => _aggregate.Request;
    
    private readonly BuilderValidatorAggregate _aggregate;

    public BaseBuilder(BuilderValidatorAggregate aggregate)
    {
        _aggregate = aggregate;
    }

    public ILogger<T> GetLogger<T>()
        where T : class
        => _aggregate.HttpContextAccessor.HttpContext!.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger<T>();

    protected TService GetRequiredService<TService>()
        where TService : class
        => _aggregate.HttpContextAccessor.HttpContext!.RequestServices.GetRequiredService<TService>();

    public virtual Task<__SA.CreateSalesArrangementRequest> UpdateParameters(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(Request);
    }

    public virtual Task PostCreateProcessing(int salesArrangementId, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
