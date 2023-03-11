﻿using __SA = DomainServices.SalesArrangementService.Contracts;

namespace NOBY.Api.Endpoints.Cases.CreateSalesArrangement.Services;

internal sealed class CustomerChangeBuilder
    : BaseBuilder, ICreateSalesArrangementParametersBuilder
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CustomerChangeBuilder(ILogger<CreateSalesArrangementParametersFactory> logger, __SA.CreateSalesArrangementRequest request, IHttpContextAccessor httpContextAccessor)
        : base(logger, request)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<__SA.CreateSalesArrangementRequest> UpdateParameters(CancellationToken cancellationToken = default(CancellationToken))
    {
        // Dotažení dat z KonsDB ohledně účtu pro splácení přes getMortgage
        var productService = _httpContextAccessor.HttpContext!.RequestServices.GetRequiredService<DomainServices.ProductService.Clients.IProductServiceClient>();
        try
        {
            var mortgageInstance = await productService.GetMortgage(_request.CaseId, cancellationToken);

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
