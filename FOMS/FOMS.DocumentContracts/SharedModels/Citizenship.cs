namespace FOMS.DocumentContracts.SharedModels;

public class Citizenship
{
    public bool HasCzechCitizenship { get; set; }

    public int? CitizenshipCountryId { get; set; }

    public int? ResidencyPermitType { get; set; }

    public DateOnly? ValidFrom { get; set; }

    public DateOnly? ValidTo { get; set; }
}
