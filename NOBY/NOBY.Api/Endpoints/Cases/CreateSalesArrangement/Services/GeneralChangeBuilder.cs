using __SA = DomainServices.SalesArrangementService.Contracts;

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
            RepaymentAccount = new(),
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
                _request.GeneralChange.RepaymentAccount.AgreedPrefix = mortgageInstance.Mortgage.RepaymentAccount.Prefix;
                _request.GeneralChange.RepaymentAccount.AgreedNumber = mortgageInstance.Mortgage.RepaymentAccount.Number;
                _request.GeneralChange.RepaymentAccount.AgreedBankCode = mortgageInstance.Mortgage.RepaymentAccount.BankCode;
            }
            _request.GeneralChange.LoanPaymentAmount.ActualLoanPaymentAmount = (decimal?)mortgageInstance.Mortgage?.LoanPaymentAmount ?? 0M;
            _request.GeneralChange.DueDate.ActualLoanDueDate = (DateTime?)mortgageInstance.Mortgage?.LoanDueDate ?? DateTime.Now;

            // real estates
            if (mortgageInstance.Mortgage!.LoanRealEstates is not null && mortgageInstance.Mortgage.LoanRealEstates.Any())
            {
                _request.GeneralChange.LoanRealEstate = new __SA.SalesArrangementParametersGeneralChange.Types.LoanRealEstateObject();
                _request.GeneralChange.LoanRealEstate.LoanRealEstates.AddRange(mortgageInstance.Mortgage.LoanRealEstates.Select(t => new __SA.SalesArrangementParametersGeneralChange.Types.LoanRealEstatesItem
                {
                    RealEstatePurchaseTypeId = t.RealEstatePurchaseTypeId,
                    RealEstateTypeId = t.RealEstateTypeId
                }));
            }
        }
        catch
        {
            _logger.LogInformation("GeneralChangeBuilder: Account not found in ProductService");
        }

        return _request;
    }
}
