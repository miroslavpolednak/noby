using NOBY.Api.Endpoints.SalesArrangement.CreateSalesArrangement.Services.Internals;

namespace NOBY.Api.Endpoints.SalesArrangement.CreateSalesArrangement.Services;

internal sealed class CustomerChange3602AValidator
    : BaseValidator<CustomerChange3602ABuilder>, ICreateSalesArrangementParametersValidator
{
    public CustomerChange3602AValidator(BuilderValidatorAggregate aggregate)
        : base(aggregate) { }

    public override async Task<ICreateSalesArrangementParametersBuilder> Validate(CancellationToken cancellationToken = default)
    {
        ValidateUserPermissions(UserPermissions.CHANGE_REQUESTS_Access);

        var productService = GetRequiredService<DomainServices.ProductService.Clients.IProductServiceClient>();
        var salesArrangementService = GetRequiredService<DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClient>();

        // instance hypo
        var productInstance = await productService.GetMortgage(Request.CaseId, cancellationToken);

        if (productInstance.Mortgage?.ContractSignedDate is null)
        {
            throw new NobyValidationException(90014);
        }

        // neexistuje SAType=9 v Case
        var salesArrangementsForCase = await salesArrangementService.GetSalesArrangementList(Request.CaseId, cancellationToken);
        if (!salesArrangementsForCase.SalesArrangements.Any(t => t.SalesArrangementTypeId == (int)SalesArrangementTypes.CustomerChange))
        {
            throw new NobyValidationException(90015);
        }

        return await base.Validate(cancellationToken);
    }
}
