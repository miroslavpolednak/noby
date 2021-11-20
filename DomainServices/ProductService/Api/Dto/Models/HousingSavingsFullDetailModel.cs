namespace DomainServices.ProductService.Api.Dto;

internal class HousingSavingsFullDetailModel : HousingSavingsBasicDetailModel
{
    public int MesicniSplatka { get; set; }

    public static explicit operator Contracts.FullDetail(HousingSavingsFullDetailModel value)
        => new()
        {
            MonthlyDeposit = value.MesicniSplatka
        };
}
