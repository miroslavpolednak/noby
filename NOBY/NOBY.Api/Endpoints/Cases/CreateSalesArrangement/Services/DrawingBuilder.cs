using NOBY.Api.Endpoints.Cases.CreateSalesArrangement.Services.Internals;
using __SA = DomainServices.SalesArrangementService.Contracts;

namespace NOBY.Api.Endpoints.Cases.CreateSalesArrangement.Services;

internal sealed class DrawingBuilder
    : BaseBuilder
{
    public override async Task<__SA.CreateSalesArrangementRequest> UpdateParameters(CancellationToken cancellationToken = default(CancellationToken))
    {
        _request.Drawing = new()
        {
            RepaymentAccount = new()
            {
                IsAccountNumberMissing = true
            }
        };

        // Dotažení dat z KonsDB ohledně účtu pro splácení přes getMortgage
        var productService = _httpContextAccessor.HttpContext!.RequestServices.GetRequiredService<DomainServices.ProductService.Clients.IProductServiceClient>();
        try
        {
            var mortgageInstance = await productService.GetMortgage(_request.CaseId, cancellationToken);

            if (!string.IsNullOrEmpty(mortgageInstance.Mortgage.RepaymentAccount?.Number) && !string.IsNullOrEmpty(mortgageInstance.Mortgage.RepaymentAccount?.BankCode))
            {
                _request.Drawing.RepaymentAccount.IsAccountNumberMissing = false;
                _request.Drawing.RepaymentAccount.Prefix = mortgageInstance.Mortgage.RepaymentAccount.Prefix;
                _request.Drawing.RepaymentAccount.Number = mortgageInstance.Mortgage.RepaymentAccount.Number;
                _request.Drawing.RepaymentAccount.BankCode = mortgageInstance.Mortgage.RepaymentAccount.BankCode;
            }
            else
                _logger.LogInformation("DrawingBuilder: Account is empty");
        }
        catch
        {
            _logger.LogInformation("DrawingBuilder: Account not found in ProductService");
        }

        return _request;
    }

    public DrawingBuilder(ILogger<CreateSalesArrangementParametersFactory> logger, __SA.CreateSalesArrangementRequest request, IHttpContextAccessor httpContextAccessor)
        : base(logger, request, httpContextAccessor)
    {
    }
}
