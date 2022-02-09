using DomainServices.OfferService.Contracts;
using ExternalServices.Eas;

namespace DomainServices.OfferService.Api;

public static class EasExtensions
{

    //public static LoanData ToLoanData(this Eas.EasWrapper.ESBI_SIMULATION_RESULTS result)
    //    => new LoanData
    //    {
    //        GrantedLoanDate = EasHelpers.CreateDateFromEas(result.USS_datum_prideleni),
    //        GrantedLoanLastPaymentDate = EasHelpers.CreateDateFromEas(result.USS_datum_posledni_splatky),
    //        GrantedLoanAmount = result.USS_vyse_uveru,
    //        GrantedLoanInterestRate = result.PU_urokova_sazba,
    //        GrantedLoanMonthlyPayment = result.USS_mesicni_splatka,
    //        GrantedLoanTotalFees = result.USS_poplatky_za_vedeni_uctu_celkem,
    //        GrantedLoanRPSN = result.RPSN_pu_uss,
    //        GrantedLoanRPSNPaymentAmount = result.RPSN_pu_uss_suma_plateb,
    //        GrantedLoanRPSNNumberOfPaymentsForClient = result.RPSN_pu_uss_pocet_plateb,
    //        GrantedLoanTotalInterests = result.USS_uroky_celkem,
    //        GrantedLoanOverpayment = result.ZAJISTENI_minimalni_kategoria
    //    };

}