namespace NOBY.Api.Endpoints.Customer.GetCustomerDetail.Dto;

public class NaturalPersonModel
    : NOBY.Dto.BaseNaturalPerson
{
    /// <summary>
    /// Přihlášen k aktualizaci dat ze základních registrů
    /// </summary>
    public bool? IsBrSubscribed { get; set; }
}