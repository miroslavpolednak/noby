using NOBY.Api.Endpoints.SalesArrangement.CreateSalesArrangement.Services.Internals;

namespace NOBY.Api.Endpoints.SalesArrangement.CreateSalesArrangement.Services;

internal sealed class RefixationValidator
    : BaseValidator<RetentionBuilder>, ICreateSalesArrangementParametersValidator
{
    public RefixationValidator(BuilderValidatorAggregate aggregate)
        : base(aggregate) { }

    public override async Task<ICreateSalesArrangementParametersBuilder> Validate(CancellationToken cancellationToken = default)
    {
        var baseValidator = new RetentionValidator(_aggregate);
        return await baseValidator.Validate(cancellationToken);
    }
}
