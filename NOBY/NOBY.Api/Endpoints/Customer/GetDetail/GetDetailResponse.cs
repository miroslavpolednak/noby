namespace NOBY.Api.Endpoints.Customer.GetDetail;

public sealed class GetDetailResponse
{
    public List<CIS.Foms.Types.CustomerIdentity>? Identities { get; set; }
    
    public Dto.NaturalPersonModel? NaturalPerson { get; set; }
    
    public Shared.JuridicalPerson? JuridicalPerson { get; set; }
    
    public bool Updatable { get; set; }
    
    public Shared.LegalCapacityItem? LegalCapacity { get; set; }
    
    public List<CIS.Foms.Types.Address>? Addresses { get; set; }
    
    public SharedDto.ContactsConfirmedDto? Contacts { get; set; }
    
    public SharedDto.IdentificationDocumentFull? IdentificationDocument { get; set; }
}