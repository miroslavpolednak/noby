using _SA = DomainServices.SalesArrangementService.Contracts;
using _Pr = DomainServices.ProductService.Contracts;

namespace FOMS.Api.Endpoints.Cases.CreateSalesArrangement.Services;

internal class DrawingBuilder
        : BaseBuilder, ICreateSalesArrangementParametersBuilder
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DrawingBuilder(ILogger<CreateSalesArrangementParametersFactory> logger, _SA.CreateSalesArrangementRequest request, IHttpContextAccessor httpContextAccessor)
        : base(logger, request)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<_SA.CreateSalesArrangementRequest> UpdateParameters(CancellationToken cancellationToken = default(CancellationToken))
    {
        _request.Drawing = new _SA.SalesArrangementParametersDrawing
        {
            RepaymentAccount = new _SA.SalesArrangementParametersDrawing.Types.SalesArrangementParametersDrawingRepaymentAccount
            {
                IsAccountNumberMissing = true
            }
        };

        // Dotažení dat z KonsDB ohledně účtu pro splácení přes getMortgage
        var productService = _httpContextAccessor.HttpContext!.RequestServices.GetRequiredService<DomainServices.ProductService.Abstraction.IProductServiceAbstraction>();
        try
        {
            var mortgageInstance = ServiceCallResult.ResolveAndThrowIfError<_Pr.GetMortgageResponse>(await productService.GetMortgage(_request.CaseId, cancellationToken));

            if (!string.IsNullOrEmpty(mortgageInstance.Mortgage.PaymentAccount?.Number) && !string.IsNullOrEmpty(mortgageInstance.Mortgage.PaymentAccount?.BankCode))
            {
                _request.Drawing.RepaymentAccount.IsAccountNumberMissing = false;
                _request.Drawing.RepaymentAccount.Prefix = mortgageInstance.Mortgage.PaymentAccount.Prefix;
                _request.Drawing.RepaymentAccount.Number = mortgageInstance.Mortgage.PaymentAccount.Number;
                _request.Drawing.RepaymentAccount.BankCode = mortgageInstance.Mortgage.PaymentAccount.BankCode;
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
}
