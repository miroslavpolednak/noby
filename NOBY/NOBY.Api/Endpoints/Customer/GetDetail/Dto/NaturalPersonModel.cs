namespace NOBY.Api.Endpoints.Customer.GetDetail.Dto;

public class NaturalPersonModel
    : SharedDto.BaseNaturalPerson
{
    /// <summary>
    /// Přihlášen k aktualizaci dat ze základních registrů
    /// </summary>
    public bool? IsBrSubscribed { get; set; }
}