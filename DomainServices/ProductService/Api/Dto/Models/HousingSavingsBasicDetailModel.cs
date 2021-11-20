namespace DomainServices.ProductService.Api.Dto;

internal class HousingSavingsBasicDetailModel
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public string CisloSmlouvy { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public int CilovaCastka { get; set; }
    public bool ZadaStatniPremii { get; set; }
    public decimal StavUctuCelkem { get; set; }
    public decimal UrokovaSazbaSporeni { get; set; }
    public DateTime DatrumUzavreniSmlouvy { get; set; }

    public static explicit operator Contracts.BasicDetail(HousingSavingsBasicDetailModel value)
        => new()
        {
            ContractDate = value.DatrumUzavreniSmlouvy,
            State = 1,
            BusinessAction = 1,
            StateSubsidy = value.ZadaStatniPremii,
            CurrentAmount = value.StavUctuCelkem,
            ContractNumber = value.CisloSmlouvy,
            GrantedLoanDate = null,
            InterestRate = value.UrokovaSazbaSporeni,
            TargetAmount = value.CilovaCastka,
            Tarif = 1
        };
}
