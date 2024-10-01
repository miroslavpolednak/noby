using DomainServices.SalesArrangementService.Clients;
using DomainServices.SalesArrangementService.Contracts;
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
            .Any(t => t.SalesArrangementTypeId is ((int)SalesArrangementTypes.MortgageRetention or (int)SalesArrangementTypes.MortgageRefixation) && t.IsInState(SalesArrangementHelpers.ActiveSalesArrangementStates)))
        {
            throw new NobyValidationException(90052);
        }

        return await base.Validate(cancellationToken);
    }
}