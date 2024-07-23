using DomainServices.CaseService.Clients.v1;
using DomainServices.ProductService.Clients;
using NOBY.Api.Endpoints.SalesArrangement.CreateSalesArrangement.Services.Internals;

namespace NOBY.Api.Endpoints.SalesArrangement.CreateSalesArrangement.Services;

internal sealed class CustomerChange3602BValidator
    : BaseValidator<CustomerChange3602BBuilder>, ICreateSalesArrangementParametersValidator
{
    public CustomerChange3602BValidator(BuilderValidatorAggregate aggregate)
        : base(aggregate) { }

    public override async Task<ICreateSalesArrangementParametersBuilder> Validate(CancellationToken cancellationToken = default)
    {
        ValidateUserPermissions(UserPermissions.CHANGE_REQUESTS_Access);

        var caseService = GetRequiredService<ICaseServiceClient>();
        var productService = GetRequiredService<IProductServiceClient>();

        var caseInstance = await caseService.GetCaseDetail(Request.CaseId, cancellationToken);
        if (caseInstance.State == (int)EnumCaseStates.InProgress)
        {
            throw new NobyValidationException(90040);
        }

        // instance hypo
        var productInstance = await productService.GetMortgage(Request.CaseId, cancellationToken);
        if (productInstance.Mortgage?.ContractSignedDate is not null)
        {
            throw new NobyValidationException(90041);
        }

        return await base.Validate(cancellationToken);
    }
}
