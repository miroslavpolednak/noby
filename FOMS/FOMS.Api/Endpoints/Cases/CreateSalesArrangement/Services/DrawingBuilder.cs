using _SA = DomainServices.SalesArrangementService.Contracts;
using _Pr = DomainServices.ProductService.Contracts;

namespace FOMS.Api.Endpoints.Cases.CreateSalesArrangement.Services;

internal class DrawingBuilder
        : BaseBuilder, ICreateSalesArrangementParametersBuilder
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DrawingBuilder(ILogger<CreateSalesArrangementParametersFactory> logger, long caseId, IHttpContextAccessor httpContextAccessor)
        : base(logger, caseId)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<_SA.UpdateSalesArrangementParametersRequest> CreateParameters(int salesArrangementId, CancellationToken cancellationToken = default(CancellationToken))
    {
        var data = new _SA.UpdateSalesArrangementParametersRequest()
        {
            SalesArrangementId = salesArrangementId,
            Drawing = new _SA.SalesArrangementParametersDrawing
            {
                RepaymentAccount = new _SA.SalesArrangementParametersDrawing.Types.SalesArrangementParametersDrawingRepaymentAccount
                {
                    IsAccountNumberMissing = true
                }
            }
        };

        // Dotažení dat z KonsDB ohledně účtu pro splácení přes getMortgage
        var productService = _httpContextAccessor.HttpContext!.RequestServices.GetRequiredService<DomainServices.ProductService.Abstraction.IProductServiceAbstraction>();
        try
        {
            var mortgageInstance = ServiceCallResult.ResolveAndThrowIfError<_Pr.GetMortgageResponse>(await productService.GetMortgage(_caseId, cancellationToken));
            if (!string.IsNullOrEmpty(mortgageInstance.Mortgage.PaymentAccount?.Number) && !string.IsNullOrEmpty(mortgageInstance.Mortgage.PaymentAccount?.BankCode))
            {
                data.Drawing.RepaymentAccount.IsAccountNumberMissing = false;
                data.Drawing.RepaymentAccount.Prefix = mortgageInstance.Mortgage.PaymentAccount.Prefix;
                data.Drawing.RepaymentAccount.Number = mortgageInstance.Mortgage.PaymentAccount.Number;
                data.Drawing.RepaymentAccount.BankCode = mortgageInstance.Mortgage.PaymentAccount.BankCode;
            }
            else
                _logger.LogInformation("DrawingBuilder: Account is empty");
        }
        catch
        {
            _logger.LogInformation("DrawingBuilder: Account not found in ProductService");
        }

        return data;
    }
}
