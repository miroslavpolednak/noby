namespace FOMS.Api.Endpoints.Savings.Offer.Dto;

internal record LoanData(
    DateTime? GrantedLoanDate,
    DateTime? GrantedLoanLastPaymentDate,
    decimal GrantedLoanAmount,
    decimal GrantedLoanInterestRate,
    decimal GrantedLoanMonthlyPayment,
    decimal GrantedLoanTotalFees,
    decimal GrantedLoanRPSN,
    decimal GrantedLoanRPSNPaymentAmount,
    int GrantedLoanRPSNNumberOfPaymentsForClient
)
{
    //public static implicit operator LoanData(DomainServices.OfferService.Contracts.LoanData data)
    //    => new LoanData(
    //        data.GrantedLoanDate,
    //        data.GrantedLoanLastPaymentDate,
    //        data.GrantedLoanAmount,
    //        data.GrantedLoanInterestRate,
    //        data.GrantedLoanMonthlyPayment,
    //        data.GrantedLoanTotalFees,
    //        data.GrantedLoanRPSN,
    //        data.GrantedLoanRPSNPaymentAmount,
    //        data.GrantedLoanRPSNNumberOfPaymentsForClient
    //    );
}
