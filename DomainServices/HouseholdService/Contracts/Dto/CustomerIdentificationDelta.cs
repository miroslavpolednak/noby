namespace DomainServices.HouseholdService.Contracts.Dto;

public class CustomerIdentificationDelta
{
    public int? IdentificationMethodId { get; set; }

    public DateTime? IdentificationDate { get; set; }

    public string? CzechIdentificationNumber { get; set; }
}