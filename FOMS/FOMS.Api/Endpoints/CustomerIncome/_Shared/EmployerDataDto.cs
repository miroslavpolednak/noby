namespace FOMS.Api.Endpoints.CustomerIncome.Dto;

public class EmployerDataDto
{
    public string? Name { get; set; }
    public string? BirthNumber { get; set; }
    public string? Cin { get; set; }
    public CIS.Foms.Types.Address? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public string? ClassificationOfEconomicActivities { get; set; }
    public int? WorkSectorId { get; set; }
    public bool SensitiveSector { get; set; }
}
