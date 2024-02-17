using NOBY.Api.Endpoints.Cases.CreateSalesArrangement.Services.Internals;

namespace NOBY.Api.Endpoints.Cases.CreateSalesArrangement.Services;

internal sealed class DrawingValidator
    : BaseValidator<DrawingBuilder>, ICreateSalesArrangementParametersValidator
{
    public DrawingValidator(BuilderValidatorAggregate aggregate)
        : base(aggregate) { }

    /// <remarks>
    /// https://wiki.kb.cz/pages/viewpage.action?pageId=424119307#id-%C4%8Cerp%C3%A1ni%C5%BD%C3%A1dosto%C4%8Derp%C3%A1n%C3%AD-Kontrolyp%C5%99edformul%C3%A1%C5%99em%C4%8Derp%C3%A1n%C3%AD
    /// </remarks>
    public override async Task<ICreateSalesArrangementParametersBuilder> Validate(CancellationToken cancellationToken = default)
    {
        ValidateUserPermissions(UserPermissions.CHANGE_REQUESTS_Access);

        var productService = GetRequiredService<DomainServices.ProductService.Clients.IProductServiceClient>();
        // instance hypo
        var productInstance = await productService.GetMortgage(Request.CaseId, cancellationToken);

        if (productInstance.Mortgage.AvailableForDrawing <= 0M)
        {
            throw new NobyValidationException(90011);
        }

        if (productInstance.Mortgage.FirstAnnuityPaymentDate != null && DateTime.Now >= productInstance.Mortgage.FirstAnnuityPaymentDate)
        {
            throw new NobyValidationException(90012);
        }

        if (string.IsNullOrEmpty(productInstance.Mortgage.PaymentAccount?.Number))
        {
            throw new NobyValidationException(90013);
        }

        return await base.Validate(cancellationToken);
    }
}
