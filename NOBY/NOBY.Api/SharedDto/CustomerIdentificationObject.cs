namespace NOBY.Api.SharedDto;

public sealed class CustomerIdentificationObject
{
    public int? CustomerIdentification { get; set; }

    public string? CustomerName { get; set; }

    public DateTime? IdentificationDate { get; set; }
}