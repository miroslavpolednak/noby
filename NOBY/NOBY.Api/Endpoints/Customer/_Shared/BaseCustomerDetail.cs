using NOBY.Api.SharedDto;

namespace NOBY.Api.Endpoints.Customer.Shared;

public abstract class BaseCustomerDetail
{
    public NaturalPerson? NaturalPerson { get; set; }

    public JuridicalPerson? JuridicalPerson { get; set; }

    public IdentificationDocumentFull? IdentificationDocument { get; set; }

    public List<CIS.Foms.Types.Address>? Addresses { get; set; }

    public CustomerIdentificationMethod? CustomerIdentification { get; set; }

    /// <summary>
    /// Objekt právního omezení	
    /// </summary>
    public LegalCapacityItem? LegalCapacity { get; set; }

    /// <summary>
    /// Přihlášen k aktualizaci dat ze základních registrů
    /// </summary>
    public bool? IsBrSubscribed { get; set; }

    /// <summary>
    /// Zvláštní vztah ke Komerční bance
    /// </summary>
    public bool? HasRelationshipWithKB { get; set; }

    /// <summary>
    /// Osoba blízká zaměstnanci KB
    /// </summary>
    public bool? HasRelationshipWithKBEmployee { get; set; }

    /// <summary>
    /// Propojení s obchodní korporací
    /// </summary>
    public bool? HasRelationshipWithCorporate { get; set; }

    /// <summary>
    /// Politicky exponovaná osoba
    /// </summary>
    public bool? IsPoliticallyExposed { get; set; }

    /// <summary>
    /// Americký občan (FATCA)
    /// </summary>
    public bool? IsUSPerson { get; set; }
}

internal interface ICustomerDetailConfirmedContacts
{
    PhoneNumberConfirmedDto? MobilePhone { get; set; }
    EmailAddressConfirmedDto? EmailAddress { get; set; }
}

internal interface ICustomerDetailContacts
{
    PhoneNumberDto? MobilePhone { get; set; }
    EmailAddressDto? EmailAddress { get; set; }
}