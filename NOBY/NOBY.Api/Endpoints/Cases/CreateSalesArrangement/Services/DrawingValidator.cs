﻿using NOBY.Api.Endpoints.Cases.CreateSalesArrangement.Services.Internals;

namespace NOBY.Api.Endpoints.Cases.CreateSalesArrangement.Services;

internal sealed class DrawingValidator
    : BaseValidator, ICreateSalesArrangementParametersValidator
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DrawingValidator(ILogger<CreateSalesArrangementParametersFactory> logger, DomainServices.SalesArrangementService.Contracts.CreateSalesArrangementRequest request, IHttpContextAccessor httpContextAccessor)
        : base(logger, request)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    /// <remarks>
    /// https://wiki.kb.cz/pages/viewpage.action?pageId=424119307#id-%C4%8Cerp%C3%A1ni%C5%BD%C3%A1dosto%C4%8Derp%C3%A1n%C3%AD-Kontrolyp%C5%99edformul%C3%A1%C5%99em%C4%8Derp%C3%A1n%C3%AD
    /// </remarks>
    public async Task<ICreateSalesArrangementParametersBuilder> Validate(CancellationToken cancellationToken = default(CancellationToken))
    {
        var productService = _httpContextAccessor.HttpContext!.RequestServices.GetRequiredService<DomainServices.ProductService.Clients.IProductServiceClient>();
        // instance hypo
        var productInstance = await productService.GetMortgage(_request.CaseId, cancellationToken);

        if (productInstance.Mortgage.AvailableForDrawing <= 0M)
        {
            throw new NobyValidationException(90011);
        }

        if (productInstance.Mortgage.FirstAnnuityInstallmentDate != null && DateTime.Now >= productInstance.Mortgage.FirstAnnuityInstallmentDate)
        {
            throw new NobyValidationException(90012);
        }

        if (string.IsNullOrEmpty(productInstance.Mortgage.PaymentAccount?.Number))
        {
            throw new NobyValidationException(90013);
        }

        return new DrawingBuilder(_logger, _request, _httpContextAccessor);
    }
}
