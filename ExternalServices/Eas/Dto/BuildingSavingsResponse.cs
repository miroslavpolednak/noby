using ExternalServices.Eas.V1.EasWrapper;

namespace ExternalServices.Eas.Dto;

public class BuildingSavingsResponse
{
    private BuildingSavingsResponse()
    {
    }

    internal static BuildingSavingsResponse CreateInstance(ESBI_SIMULATION_RESULTS result)
    {
        return new BuildingSavingsResponse
        {
            SavingsLengthInMonths = (int)result.SS_doba_sporeni_v_mesicich,
            InterestRate = result.SS_urokova_sazba,
            SavingsSum = result.SS_celkem_nasporeno,
            DepositsSum = result.SS_vklady_celkem,
            InterestsSum = result.SS_uroky_celkem,
            FeesSum = result.SS_uhrady_a_poplatky_celkem,
            BonusInterestRate = result.SS_urokove_zvyhodneni,
            StateSubsidySum = result.SS_statni_podpora_celkem + result.SS_evidovana_statni_podpora,
            InterestBenefitAmount = result.SS_uroky_z_benefitu,
            InterestBenefitTax = result.SS_uroky_z_benefitu_dan
        };
    }

    public int SavingsLengthInMonths { get; set; }

    public decimal InterestRate { get; set; }

    public decimal SavingsSum { get; set; }

    public decimal DepositsSum { get; set; }

    public decimal InterestsSum { get; set; }

    public decimal FeesSum { get; set; }

    public decimal BonusInterestRate { get; set; }

    public decimal StateSubsidySum { get; set; }

    public decimal InterestBenefitAmount { get; set; }

    public decimal InterestBenefitTax { get; set; }
}