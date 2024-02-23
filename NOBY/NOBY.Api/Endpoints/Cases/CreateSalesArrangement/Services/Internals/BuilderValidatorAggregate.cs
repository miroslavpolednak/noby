namespace NOBY.Api.Endpoints.Cases.CreateSalesArrangement.Services.Internals;

internal sealed class BuilderValidatorAggregate
{
    public DomainServices.SalesArrangementService.Contracts.CreateSalesArrangementRequest Request { get; init; }
    public IHttpContextAccessor HttpContextAccessor { get; init; }

    public BuilderValidatorAggregate(DomainServices.SalesArrangementService.Contracts.CreateSalesArrangementRequest request, IHttpContextAccessor httpContextAccessor)
    {
        Request = request;
        HttpContextAccessor = httpContextAccessor;
    }
}
