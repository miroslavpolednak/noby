using DomainServices.SalesArrangementService.Clients;
using NOBY.Api.Endpoints.SalesArrangement.CreateSalesArrangement.Services.Internals;

namespace NOBY.Api.Endpoints.SalesArrangement.CreateSalesArrangement.Services;

internal sealed class RetentionValidator
    : BaseValidator<RetentionBuilder>, ICreateSalesArrangementParametersValidator
{
    public RetentionValidator(BuilderValidatorAggregate aggregate)
        : base(aggregate) { }

    public override async Task<ICreateSalesArrangementParametersBuilder> Validate(CancellationToken cancellationToken = default)
    {
        ValidateUserPermissions(UserPermissions.CHANGE_REQUESTS_RefinancingAccess);

        var saService = GetRequiredService<ISalesArrangementServiceClient>();

        var salesArrangements = await saService.GetSalesArrangementList(Request.CaseId, cancellationToken);
        if (salesArrangements
            .SalesArrangements
            .Any(t => t.SalesArrangementTypeId == Request.SalesArrangementTypeId && t.State is not ((int)SalesArrangementStates.Finished) or (int)SalesArrangementStates.Cancelled))
        {
            throw new NobyValidationException(90052);
        }

        return await base.Validate(cancellationToken);
    }
}
