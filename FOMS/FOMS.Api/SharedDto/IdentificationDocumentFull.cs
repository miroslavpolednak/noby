namespace FOMS.Api.SharedDto;

public class IdentificationDocumentFull
    : IdentificationDocumentWithIssuedBy
{
    public DateTime? ValidTo { get; set; }

    public DateTime? IssuedOn { get; set; }

    public string? RegisterPlace { get; set; }
}
