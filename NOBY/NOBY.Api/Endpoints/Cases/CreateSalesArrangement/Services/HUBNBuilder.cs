using __SA = DomainServices.SalesArrangementService.Contracts;

namespace NOBY.Api.Endpoints.Cases.CreateSalesArrangement.Services;

internal sealed class HUBNBuilder
    : BaseBuilder, ICreateSalesArrangementParametersBuilder
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HUBNBuilder(ILogger<CreateSalesArrangementParametersFactory> logger, __SA.CreateSalesArrangementRequest request, IHttpContextAccessor httpContextAccessor)
        : base(logger, request)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<__SA.CreateSalesArrangementRequest> UpdateParameters(CancellationToken cancellationToken = default(CancellationToken))
    {
        _request.HUBN = new __SA.SalesArrangementParametersHUBN
        {
            DrawingDateTo = new(),
            CommentToChangeRequest = new()
        };

        // Dotažení dat z KonsDB ohledně účtu pro splácení přes getMortgage
        var productService = _httpContextAccessor.HttpContext!.RequestServices.GetRequiredService<DomainServices.ProductService.Clients.IProductServiceClient>();
        try
        {
            var mortgageInstance = await productService.GetMortgage(_request.CaseId, cancellationToken);

            _request.HUBN.LoanAmount.AgreedLoanAmount = (decimal?)mortgageInstance.Mortgage?.LoanAmount ?? 0M;
            _request.HUBN.LoanAmount.AgreedLoanDueDate = (DateTime?)mortgageInstance.Mortgage?.LoanDueDate ?? DateTime.Now;
            _request.HUBN.LoanAmount.AgreedLoanPaymentAmount = (decimal?)mortgageInstance.Mortgage?.LoanPaymentAmount ?? 0M;
            if (mortgageInstance.Mortgage?.LoanPurposes != null)
            {
                _request.HUBN.LoanPurposes.AddRange(mortgageInstance.Mortgage.LoanPurposes.Select(t => new __SA.SalesArrangementParametersHUBN.Types.LoanPurposeItem
                {
                    Sum = t.Sum,
                    LoanPurposeId = t.LoanPurposeId
                }));
            }
            _request.HUBN.ExpectedDateOfDrawing.AgreedExpectedDateOfDrawing = (DateTime?)mortgageInstance.Mortgage?.ExpectedDateOfDrawing ?? DateTime.Now;
            _request.HUBN.DrawingDateTo.AgreedDrawingDateTo = (DateTime?)mortgageInstance.Mortgage?.DrawingDateTo ?? DateTime.Now;
        }
        catch
        {
            _logger.LogInformation("HUBNBuilder: Account not found in ProductService");
        }

        return _request;
    }
}
