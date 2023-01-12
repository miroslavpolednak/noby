﻿using __SA = DomainServices.SalesArrangementService.Contracts;

namespace NOBY.Api.Endpoints.Cases.CreateSalesArrangement.Services;

internal sealed class GeneralChangeBuilder
    : BaseBuilder, ICreateSalesArrangementParametersBuilder
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GeneralChangeBuilder(ILogger<CreateSalesArrangementParametersFactory> logger, __SA.CreateSalesArrangementRequest request, IHttpContextAccessor httpContextAccessor)
        : base(logger, request)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<__SA.CreateSalesArrangementRequest> UpdateParameters(CancellationToken cancellationToken = default(CancellationToken))
    {
        _request.GeneralChange = new __SA.SalesArrangementParametersGeneralChange
        {
            Collateral = new(),
            PaymentDay = new(),
            DrawingDateTo = new(),
            PaymentAccount = new(),
            LoanPaymentAmount = new(),
            DueDate = new(),
            LoanRealEstate = new(),
            LoanPurpose = new(),
            DrawingAndOtherConditions = new(),
            CommentToChangeRequest = new()
        };

        // Dotažení dat z KonsDB ohledně účtu pro splácení přes getMortgage
        var productService = _httpContextAccessor.HttpContext!.RequestServices.GetRequiredService<DomainServices.ProductService.Clients.IProductServiceClient>();
        try
        {
            var mortgageInstance = await productService.GetMortgage(_request.CaseId, cancellationToken);

            _request.GeneralChange.PaymentDay.AgreedPaymentDay = mortgageInstance.Mortgage?.PaymentDay ?? 0;
            _request.GeneralChange.DrawingDateTo.AgreedDrawingDateTo = (DateTime?)mortgageInstance.Mortgage?.DrawingDateTo ?? DateTime.Now;
            if (mortgageInstance.Mortgage?.PaymentAccount != null)
            {
                _request.GeneralChange.PaymentAccount.AgreedPrefix = mortgageInstance.Mortgage.PaymentAccount.Prefix;
                _request.GeneralChange.PaymentAccount.AgreedNumber = mortgageInstance.Mortgage.PaymentAccount.Number;
                _request.GeneralChange.PaymentAccount.AgreedBankCode = mortgageInstance.Mortgage.PaymentAccount.BankCode;
            }
            _request.GeneralChange.LoanPaymentAmount.ActualLoanPaymentAmount = (decimal?)mortgageInstance.Mortgage?.LoanPaymentAmount ?? 0M;
            _request.GeneralChange.DueDate.ActualLoanDueDate = (DateTime?)mortgageInstance.Mortgage?.LoanDueDate ?? DateTime.Now;
        }
        catch
        {
            _logger.LogInformation("GeneralChangeBuilder: Account not found in ProductService");
        }

        return _request;
    }
}