using DomainServices.SalesArrangementService.Clients;
using NOBY.Api.Endpoints.SalesArrangement.CreateSalesArrangement.Services.Internals;

namespace NOBY.Api.Endpoints.SalesArrangement.CreateSalesArrangement.Services;

internal sealed class ExtraPaymentValidator(BuilderValidatorAggregate aggregate) : BaseValidator<ExtraPaymentBuilder>(aggregate)
{
    public override async Task<ICreateSalesArrangementParametersBuilder> Validate(CancellationToken cancellationToken = default)
    {
        ValidateUserPermissions(UserPermissions.CHANGE_REQUESTS_RefinancingAccess);

        var salesArrangementService = GetRequiredService<ISalesArrangementServiceClient>();

        var salesArrangements = (await salesArrangementService.GetSalesArrangementList(Request.CaseId, cancellationToken)).SalesArrangements;

        if (salesArrangements.Any(sa => sa.SalesArrangementTypeId == Request.SalesArrangementTypeId && sa.State is not (int)SalesArrangementStates.Finished and not (int)SalesArrangementStates.Cancelled))
        {
            throw new NobyValidationException(90052);
        }

        return await base.Validate(cancellationToken);
    }
}