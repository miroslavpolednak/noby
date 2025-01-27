﻿using NOBY.Api.Endpoints.SalesArrangement.CreateSalesArrangement.Services.Internals;

namespace NOBY.Api.Endpoints.SalesArrangement.CreateSalesArrangement.Services;

internal sealed class GeneralChangeValidator
    : BaseValidator<GeneralChangeBuilder>, ICreateSalesArrangementParametersValidator
{
    public GeneralChangeValidator(BuilderValidatorAggregate aggregate)
        : base(aggregate) { }

    public override async Task<ICreateSalesArrangementParametersBuilder> Validate(CancellationToken cancellationToken = default)
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
