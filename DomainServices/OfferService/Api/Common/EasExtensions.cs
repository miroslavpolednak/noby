using DomainServices.OfferService.Contracts;
using ExternalServices.Eas;

namespace DomainServices.OfferService.Api;

public static class EasExtensions
{
    public static Eas.EasWrapper.ESBI_SIMULATION_INPUT_PARAMETERS ToContract(this BuildingSavingsInput request)
        => new()
        {
            SS_Vklad_Den = 25,
            SS_Vklad_Vyska = Convert.ToInt32((decimal)request.TargetAmount * 0.005M),
            SS_Vklad_Perioda = 1,
            SS_Vklad_ZmenaVyskyPriZmeneCC = 1,
            SS_PoplatokZaVedenieDatumDen = 1,
            SS_PoplatokZaVedenieDatumMesiac = 1,
            SS_StatnaPremia_Ziadat = request.StateSubsidy ? 1 : 0,
            SS_StatnaPremia_DatumDen = 18,
            SS_StatnaPremia_DatumMesiac = 4,
            SS_StatnaPremia_DruheKolo_DatumDen = 30,
            SS_StatnaPremia_DruheKolo_DatumMesiac = 9,
            SS_KlientFyzickaPravnicka = request.ClientIsNaturalPerson ? 0 : 1,
            SS_KlientSVJ = request.ClientIsSVJ ? 1 : 0,
            SS_KodProduktu = request.ProductCode,
            SS_DatumZalozenia = EasHelpers.CreateEasDate(),
            SS_CielovaSuma = request.TargetAmount,
            SS_KodAkcie = request.ActionCode,
            SS_ZadaRocniVypis = 0,
            SS_UkonceniePoUplynutiViazacejLehoty = 1,
            SS_UkonceniePriNasporeniCC = 0,
            SS_UkoncenieVZadanyDen = 0
        };

    public static LoanData ToLoanData(this Eas.EasWrapper.ESBI_SIMULATION_RESULTS result)
        => new LoanData
        {
            GrantedLoanDate = EasHelpers.CreateDateFromEas(result.USS_datum_prideleni),
            GrantedLoanLastPaymentDate = EasHelpers.CreateDateFromEas(result.USS_datum_posledni_splatky),
            GrantedLoanAmount = result.USS_vyse_uveru,
            GrantedLoanInterestRate = result.PU_urokova_sazba,
            GrantedLoanMonthlyPayment = result.USS_mesicni_splatka,
            GrantedLoanTotalFees = result.USS_poplatky_za_vedeni_uctu_celkem,
            GrantedLoanRPSN = result.RPSN_pu_uss,
            GrantedLoanRPSNPaymentAmount = result.RPSN_pu_uss_suma_plateb,
            GrantedLoanRPSNNumberOfPaymentsForClient = result.RPSN_pu_uss_pocet_plateb
        };

    public static BuildingSavingsData ToBuildingSavingsData(this Eas.EasWrapper.ESBI_SIMULATION_RESULTS result)
        => new()
        {
            SavingPeriod = Convert.ToInt32(result.SS_doba_sporeni_v_mesicich),
            ContractTerminationDate = EasHelpers.CreateDateFromEas(result.SS_datum_ukonceni_smlouvy).GetValueOrDefault(),
            InterestRate = result.SS_urokova_sazba,
            TotalSaved = result.SS_celkem_nasporeno,
            TotalDeposits = result.SS_vklady_celkem,
            TotalFees = result.SS_uhrady_a_poplatky_celkem,
            InterestsAdvantage = result.SS_urokove_zvyhodneni,
            TotalGovernmentIncentives = result.SS_statni_podpora_celkem + result.SS_evidovana_statni_podpora,
            ExpectedGrantedLoanDate = EasHelpers.CreateDateFromEas(result.SS_predpokladany_datum_prideleni),
            DepositBalance = result.SS_zostatok_vkladu,
            BenefitInterests = result.SS_uroky_z_benefitu,
            TaxFromBenefitInterests = result.SS_uroky_z_benefitu_dan
        };

    public static List<ScheduleItem> ToScheduleItems(this Eas.EasWrapper.ESBI_SIMULATION_RESULTS result)
    {
        if (result.Operacie != null && result.Operacie.Any())
            return result.Operacie.Select(t => new ScheduleItem
            {
                Sum = Convert.ToInt32(t.Suma),
                Balance = Convert.ToInt32(t.Zostatok),
                Date = EasHelpers.CreateDateFromEas(t.DatumValutovania).GetValueOrDefault(),
                Info = t.Info,
                Note = t.Poznamka,
                Type = (ScheduleItemTypes)t.TypUctu
            })
                .ToList();
        else
            return new List<ScheduleItem>();
    }
}