using NOBY.Api.Endpoints.Cases.CreateSalesArrangement.Services.Internals;
using __SA = DomainServices.SalesArrangementService.Contracts;

namespace NOBY.Api.Endpoints.Cases.CreateSalesArrangement.Services;

internal sealed class DrawingBuilder
    : BaseBuilder
{
    public override async Task<__SA.CreateSalesArrangementRequest> UpdateParameters(CancellationToken cancellationToken = default)
    {
        Request.Drawing = new()
        {
            RepaymentAccount = new()
            {
                IsAccountNumberMissing = true
            }
        };

        // Dotažení dat z KonsDB ohledně účtu pro splácení přes getMortgage
        var productService = GetRequiredService<DomainServices.ProductService.Clients.IProductServiceClient>();
        try
        {
            var mortgageInstance = await productService.GetMortgage(Request.CaseId, cancellationToken);

            if (!string.IsNullOrEmpty(mortgageInstance.Mortgage.RepaymentAccount?.Number) && !string.IsNullOrEmpty(mortgageInstance.Mortgage.RepaymentAccount?.BankCode))
            {
                Request.Drawing.RepaymentAccount.IsAccountNumberMissing = false;
                Request.Drawing.RepaymentAccount.Prefix = mortgageInstance.Mortgage.RepaymentAccount.Prefix;
                Request.Drawing.RepaymentAccount.Number = mortgageInstance.Mortgage.RepaymentAccount.Number;
                Request.Drawing.RepaymentAccount.BankCode = mortgageInstance.Mortgage.RepaymentAccount.BankCode;
            }
            else
                GetLogger<DrawingBuilder>().LogInformation("DrawingBuilder: Account is empty");
        }
        catch
        {
            GetLogger<DrawingBuilder>().LogInformation("DrawingBuilder: Account not found in ProductService");
        }

        return Request;
    }

    public DrawingBuilder(BuilderValidatorAggregate aggregate)
        : base(aggregate) { }
}
