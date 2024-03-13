using NOBY.Api.Endpoints.SalesArrangement.CreateSalesArrangement.Services.Internals;
using __SA = DomainServices.SalesArrangementService.Contracts;

namespace NOBY.Api.Endpoints.SalesArrangement.CreateSalesArrangement.Services;

internal sealed class GeneralChangeBuilder
    : BaseBuilder
{
    public override async Task<__SA.CreateSalesArrangementRequest> UpdateParameters(CancellationToken cancellationToken = default)
    {
        Request.GeneralChange = new __SA.SalesArrangementParametersGeneralChange
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
        var productService = GetRequiredService<DomainServices.ProductService.Clients.IProductServiceClient>();
        try
        {
            var mortgageInstance = await productService.GetMortgage(Request.CaseId, cancellationToken);

            Request.GeneralChange.PaymentDay.AgreedPaymentDay = mortgageInstance.Mortgage?.PaymentDay ?? 0;
            Request.GeneralChange.DrawingDateTo.AgreedDrawingDateTo = (DateTime?)mortgageInstance.Mortgage?.DrawingDateTo ?? DateTime.Now;
            if (mortgageInstance.Mortgage?.RepaymentAccount != null)
            {
                Request.GeneralChange.RepaymentAccount.AgreedPrefix = mortgageInstance.Mortgage.RepaymentAccount.Prefix;
                Request.GeneralChange.RepaymentAccount.AgreedNumber = mortgageInstance.Mortgage.RepaymentAccount.Number;
                Request.GeneralChange.RepaymentAccount.AgreedBankCode = mortgageInstance.Mortgage.RepaymentAccount.BankCode;
            }
            Request.GeneralChange.LoanPaymentAmount.ActualLoanPaymentAmount = (decimal?)mortgageInstance.Mortgage?.LoanPaymentAmount ?? 0M;
            Request.GeneralChange.DueDate.ActualLoanDueDate = (DateTime?)mortgageInstance.Mortgage?.LoanDueDate ?? DateTime.Now;

            // real estates
            if (mortgageInstance.Mortgage!.LoanRealEstates is not null && mortgageInstance.Mortgage.LoanRealEstates.Count != 0)
            {
                Request.GeneralChange.LoanRealEstate = new __SA.SalesArrangementParametersGeneralChange.Types.LoanRealEstateObject();
                Request.GeneralChange.LoanRealEstate.LoanRealEstates.AddRange(mortgageInstance.Mortgage.LoanRealEstates.Select(t => new __SA.SalesArrangementParametersGeneralChange.Types.LoanRealEstatesItem
                {
                    RealEstatePurchaseTypeId = t.RealEstatePurchaseTypeId,
                    RealEstateTypeId = t.RealEstateTypeId
                }));
            }
        }
        catch
        {
            GetLogger<GeneralChangeBuilder>().LogInformation("GeneralChangeBuilder: Account not found in ProductService");
        }

        return Request;
    }

    public GeneralChangeBuilder(BuilderValidatorAggregate aggregate)
    : base(aggregate) { }
}
