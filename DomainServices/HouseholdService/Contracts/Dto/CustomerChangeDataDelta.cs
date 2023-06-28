using CIS.Foms.Types;

namespace DomainServices.HouseholdService.Contracts.Dto;

/// <summary>
/// Vim ze je to takto blbe na dvou mistech, ale nechce se mi sdilet ty objekty mezi DS a FE API. Museli by se v tom zaroven resit OpenApi popisky, nekter subdto se na FE API pouzivaji na vice mistech... byl by to bordel.
/// </summary>
public class CustomerChangeDataDelta
{
    public NaturalPersonDelta? NaturalPerson { get; set; }

    public IdentificationDocumentDelta? IdentificationDocument { get; set; }

    public List<Address>? Addresses { get; set; }

    public MobilePhoneDelta? MobilePhone { get; set; }

    public EmailAddressDelta? EmailAddress { get; set; }
}