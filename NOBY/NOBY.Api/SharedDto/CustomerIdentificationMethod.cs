namespace NOBY.Api.SharedDto;

public sealed class CustomerIdentificationMethod
{
    public int? IdentificationMethodId { get; set; }

    public DateTime? IdentificationDate { get; set; }

    public string? CzechIdentificationNumber { get; set; }
}
