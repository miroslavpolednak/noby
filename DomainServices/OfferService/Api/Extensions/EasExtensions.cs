using DomainServices.OfferService.Contracts;
using ExternalServices.Eas.R21.EasWrapper;

namespace DomainServices.OfferService.Api;

internal static class EasExtensions
{

    /// <summary>
    /// Converts OFFER object [SimulationInputs] to EAS object [ESBI_SIMULATION_INPUT_PARAMETERS].
    /// </summary>

    public static ESBI_SIMULATION_INPUT_PARAMETERS ToEasSimulationInputParameters(this SimulationInputs inputs)
    {
        return new ESBI_SIMULATION_INPUT_PARAMETERS
        {


        };
    }

    /// <summary>
    /// Converts EAS object [ESBI_SIMULATION_INPUT_PARAMETERS] to OFFER object [SimulationResults].
    /// </summary>
    public static SimulationResults ToSimulationResults(this ESBI_SIMULATION_RESULTS easSimulationResults)
    {
        return new SimulationResults
        {


        };
    }
}


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