using DomainServices.SalesArrangementService.Clients;
using NOBY.Api.Endpoints.Cases.CreateSalesArrangement.Services.Internals;

namespace NOBY.Api.Endpoints.Cases.CreateSalesArrangement.Services;

internal sealed class RetentionValidator
    : BaseValidator<RetentionBuilder>, ICreateSalesArrangementParametersValidator
{
    public RetentionValidator(BuilderValidatorAggregate aggregate)
        : base(aggregate) { }

    public override async Task<ICreateSalesArrangementParametersBuilder> Validate(CancellationToken cancellationToken = default(CancellationToken))
    {
        ValidateUserPermissions(UserPermissions.CHANGE_REQUESTS_RefinancingAccess);

        var saService = GetRequiredService<ISalesArrangementServiceClient>();
        
        var salesArrangements = await saService.GetSalesArrangementList(Request.CaseId, cancellationToken);
        if (salesArrangements.SalesArrangements.Any(t => t.SalesArrangementTypeId == Request.SalesArrangementTypeId && t.State != (int)SalesArrangementStates.Finished))
        {
            throw new NobyValidationException(90032, "Found another unfinished SA with the same type");
        }

        return await base.Validate(cancellationToken);
    }
}
