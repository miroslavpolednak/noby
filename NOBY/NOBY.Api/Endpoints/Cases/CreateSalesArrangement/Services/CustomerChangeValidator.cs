using NOBY.Api.Endpoints.Cases.CreateSalesArrangement.Services.Internals;

namespace NOBY.Api.Endpoints.Cases.CreateSalesArrangement.Services;

internal sealed class CustomerChangeValidator
    : BaseValidator<CustomerChangeBuilder>, ICreateSalesArrangementParametersValidator
{
    public CustomerChangeValidator(BuilderValidatorAggregate aggregate)
        : base(aggregate) { }

    public override async Task<ICreateSalesArrangementParametersBuilder> Validate(CancellationToken cancellationToken = default(CancellationToken))
    {
        ValidateUserPermissions(UserPermissions.CHANGE_REQUESTS_Access);

        var productService = GetRequiredService<DomainServices.ProductService.Clients.IProductServiceClient>();
        // instance hypo
        var productInstance = await productService.GetMortgage(Request.CaseId, cancellationToken);

        if (productInstance.Mortgage?.ContractSignedDate is null)
        {
            throw new NobyValidationException(90014);
        }

        return await base.Validate(cancellationToken);
    }
}
