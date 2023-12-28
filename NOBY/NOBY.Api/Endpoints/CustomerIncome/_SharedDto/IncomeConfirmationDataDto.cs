namespace NOBY.Api.Endpoints.CustomerIncome.SharedDto;

public class IncomeConfirmationDataDto
{
    public bool IsIssuedByExternalAccountant { get; set; }
    public DateTime? ConfirmationDate { get; set; }
    public string? ConfirmationPerson { get; set; }
    public NOBY.Dto.PhoneNumberDto? ConfirmationContact { get; set; }
}
