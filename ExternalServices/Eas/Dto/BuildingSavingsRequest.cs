using System.Globalization;
using ExternalServices.Eas.V1.EasWrapper;

namespace ExternalServices.Eas.Dto;

public class BuildingSavingsRequest
{
    private const string DateFormat = "dd.MM.yyyy";

    public int? MarketingActionCode { get; set; }

    public decimal TargetAmount { get; set; }

    public decimal MinimumMonthlyDeposit { get; set; }

    public DateOnly ContractStartDate { get; set; }

    public bool SimulateUntilBindingPeriod { get; set; }

    public DateOnly? ContractTerminationDate { get; set; }

    public bool AnnualStatementRequired { get; set; }

    public bool StateSubsidyRequired { get; set; }

    public bool IsClientSVJ { get; set; }

    public bool IsClientJuridicalPerson { get; set; }

    public DateOnly? ClientDateOfBirth { get; set; }

    public ICollection<ExtraDeposit> ExtraDeposits { get; set; } = [];

    internal ESBI_SIMULATION_INPUT_PARAMETERS ToEasRequest()
    {
        return new ESBI_SIMULATION_INPUT_PARAMETERS
        {
            simulationStart = ContractStartDate.ToString(DateFormat, CultureInfo.InvariantCulture),
            SS_Vklad_Den = 25,
            SS_Vklad_Vyska = MinimumMonthlyDeposit,
            SS_Vklad_Perioda = 1,
            SS_Vklad_ZmenaVyskyPriZmeneCC = 1,
            SS_PoplatokZaVedenieDatumDen = 1,
            SS_PoplatokZaVedenieDatumMesiac = 1,
            SS_StatnaPremia_Ziadat = Convert.ToInt32(StateSubsidyRequired),
            SS_StatnaPremia_DatumDen = 18,
            SS_StatnaPremia_DatumMesiac = 4,
            SS_StatnaPremia_DruheKolo_DatumDen = 30,
            SS_StatnaPremia_DruheKolo_DatumMesiac = 9,
            SS_DatumDovrsenia26rokovKlienta = ClientDateOfBirth is not null && ClientDateOfBirth.Value.AddYears(26) > DateOnly.FromDateTime(DateTime.Now)
                ? ClientDateOfBirth.Value.AddYears(26).ToString(DateFormat, CultureInfo.InvariantCulture)
                : null,
            SS_KlientFyzickaPravnicka = Convert.ToInt32(IsClientJuridicalPerson),
            SS_KlientSVJ = Convert.ToInt32(IsClientSVJ),
            SS_KodProduktu = 61,
            SS_DatumZalozenia = ContractStartDate.ToString(DateFormat, CultureInfo.InvariantCulture),
            SS_CielovaSuma = TargetAmount,
            SS_KodAkcie = 0,
            SS_ZadaRocniVypis = Convert.ToInt32(AnnualStatementRequired),
            SS_UkonceniePoUplynutiViazacejLehoty = Convert.ToInt32(SimulateUntilBindingPeriod),
            SS_UkonceniePriNasporeniCC = SimulateUntilBindingPeriod || ContractTerminationDate is not null ? 0 : 1,
            SS_UkoncenieVZadanyDen = ContractTerminationDate is null ? 0 : 1,
            SS_DatumVypovede = ContractTerminationDate?.ToString(DateFormat, CultureInfo.InvariantCulture),
            PU_SimulovatUver = 0,
            USS_SimulovatUver = 0,
            MimoriadneObraty = ExtraDeposits.Select(e => new ESBI_OPERACIA
            {
                TypOperacie = 1000,
                TypUctu = 9,
                DatumValutovania = e.Date.ToString(DateFormat, CultureInfo.InvariantCulture),
                Suma = e.Amount
            }).ToArray()
        };
    }

    public class ExtraDeposit
    {
        public decimal Amount { get; set; }

        public DateOnly Date { get; set; }
    }
}