using NOBY.Api.Endpoints.SalesArrangement.CreateSalesArrangement.Services.Internals;
using __SA = DomainServices.SalesArrangementService.Contracts;

namespace NOBY.Api.Endpoints.SalesArrangement.CreateSalesArrangement.Services;

internal sealed class RefixationBuilder
    : BaseBuilder
{
    public override async Task<__SA.CreateSalesArrangementRequest> UpdateParameters(CancellationToken cancellationToken = default)
    {
        var baseBuilder = new RetentionBuilder(_aggregate);
        await baseBuilder.UpdateParameters(cancellationToken);

        return Request;
    }

    public RefixationBuilder(BuilderValidatorAggregate aggregate)
        : base(aggregate) { }
}
