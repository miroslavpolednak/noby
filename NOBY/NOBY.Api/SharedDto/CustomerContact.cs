namespace NOBY.Api.SharedDto;

public class CustomerContact
{
    public bool? Confirmed { get; set; }

    public bool? IsPrimary { get; set; }

    public int? ContactTypeId { get; set; }

    public string? Value { get; set; }
}