namespace DomainServices.HouseholdService.Contracts.Dto;

public class IdentificationDocumentDelta
{
    public int IdentificationDocumentTypeId { get; set; }
    public int? IssuingCountryId { get; set; }
    public string IssuedBy { get; set; } = string.Empty;
    public DateTime? ValidTo { get; set; }
    public DateTime? IssuedOn { get; set; }
    public string? RegisterPlace { get; set; }
    public string Number { get; set; } = string.Empty;
}