﻿using NOBY.Api.Endpoints.SalesArrangement.CreateSalesArrangement.Services.Internals;
using __SA = DomainServices.SalesArrangementService.Contracts;

namespace NOBY.Api.Endpoints.SalesArrangement.CreateSalesArrangement.Services;

internal sealed class HUBNBuilder
    : BaseBuilder
{
    public override async Task<__SA.CreateSalesArrangementRequest> UpdateParameters(CancellationToken cancellationToken = default(CancellationToken))
    {
        _request.HUBN = new __SA.SalesArrangementParametersHUBN
        {
            LoanAmount = new(),
            DrawingDateTo = new(),
            CommentToChangeRequest = new(),
            ExpectedDateOfDrawing = new()
        };

        // Dotažení dat z KonsDB ohledně účtu pro splácení přes getMortgage
        var productService = _httpContextAccessor.HttpContext!.RequestServices.GetRequiredService<DomainServices.ProductService.Clients.IProductServiceClient>();
        try
        {
            var mortgageInstance = await productService.GetMortgage(_request.CaseId, cancellationToken);

            _request.HUBN.LoanAmount.AgreedLoanAmount = (decimal?)mortgageInstance.Mortgage?.LoanAmount ?? 0M;
            _request.HUBN.LoanAmount.AgreedLoanDueDate = (DateTime?)mortgageInstance.Mortgage?.LoanDueDate ?? DateTime.Now;
            _request.HUBN.LoanAmount.AgreedLoanPaymentAmount = (decimal?)mortgageInstance.Mortgage?.LoanPaymentAmount ?? 0M;
            _request.HUBN.ExpectedDateOfDrawing.AgreedExpectedDateOfDrawing = (DateTime?)mortgageInstance.Mortgage?.ExpectedDateOfDrawing ?? DateTime.Now;
            _request.HUBN.DrawingDateTo.AgreedDrawingDateTo = (DateTime?)mortgageInstance.Mortgage?.DrawingDateTo ?? DateTime.Now;
        }
        catch
        {
            _logger.LogInformation("HUBNBuilder: Account not found in ProductService");
        }

        return _request;
    }

    public HUBNBuilder(ILogger<CreateSalesArrangementParametersFactory> logger, __SA.CreateSalesArrangementRequest request, IHttpContextAccessor httpContextAccessor)
        : base(logger, request, httpContextAccessor)
    {
    }
}
