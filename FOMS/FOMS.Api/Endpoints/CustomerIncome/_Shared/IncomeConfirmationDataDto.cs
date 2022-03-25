namespace FOMS.Api.Endpoints.CustomerIncome.Dto;

public class IncomeConfirmationDataDto
{
    public bool ConfirmationByCompany { get; set; }
    public string? ConfirmationPlace { get; set; }
    public DateTime? ConfirmationDate { get; set; }
    public string? ConfirmationPerson { get; set; }
    public string? ConfirmationContact { get; set; }
}
