using NOBY.Api.Endpoints.Cases.CreateSalesArrangement.Services.Internals;
using __SA = DomainServices.SalesArrangementService.Contracts;

namespace NOBY.Api.Endpoints.Cases.CreateSalesArrangement.Services;

internal sealed class HUBNBuilder
    : BaseBuilder
{
    public override async Task<__SA.CreateSalesArrangementRequest> UpdateParameters(CancellationToken cancellationToken = default)
    {
        Request.HUBN = new __SA.SalesArrangementParametersHUBN
        {
            LoanAmount = new(),
            DrawingDateTo = new(),
            CommentToChangeRequest = new(),
            ExpectedDateOfDrawing = new()
        };

        // Dotažení dat z KonsDB ohledně účtu pro splácení přes getMortgage
        var productService = GetRequiredService<DomainServices.ProductService.Clients.IProductServiceClient>();
        try
        {
            var mortgageInstance = await productService.GetMortgage(Request.CaseId, cancellationToken);

            Request.HUBN.LoanAmount.AgreedLoanAmount = (decimal?)mortgageInstance.Mortgage?.LoanAmount ?? 0M;
            Request.HUBN.LoanAmount.AgreedLoanDueDate = (DateTime?)mortgageInstance.Mortgage?.LoanDueDate ?? DateTime.Now;
            Request.HUBN.LoanAmount.AgreedLoanPaymentAmount = (decimal?)mortgageInstance.Mortgage?.LoanPaymentAmount ?? 0M;
            Request.HUBN.ExpectedDateOfDrawing.AgreedExpectedDateOfDrawing = (DateTime?)mortgageInstance.Mortgage?.ExpectedDateOfDrawing ?? DateTime.Now;
            Request.HUBN.DrawingDateTo.AgreedDrawingDateTo = (DateTime?)mortgageInstance.Mortgage?.DrawingDateTo ?? DateTime.Now;
        }
        catch
        {
            GetLogger<HUBNBuilder>().LogInformation("HUBNBuilder: Account not found in ProductService");
        }

        return Request;
    }

    public HUBNBuilder(BuilderValidatorAggregate aggregate)
        : base(aggregate) { }
}
