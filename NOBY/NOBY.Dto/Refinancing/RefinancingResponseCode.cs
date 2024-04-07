namespace NOBY.Dto.Refinancing;

public sealed class RefinancingResponseCode
{
    public int Id { get; set; }

    public int ResponseCodeTypeId { get; set; }

    public DateTime? DataDateTime { get; set; }

    public string? DataBankCode { get; set; }

    public string? DataString { get; set; }
}
