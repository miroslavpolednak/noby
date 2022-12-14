namespace NOBY.Api.Endpoints.Customer.GetDetail;

public sealed class GetDetailResponse
{
    public List<CIS.Foms.Types.CustomerIdentity>? Identities { get; set; }
    
    public Dto.NaturalPersonModel? NaturalPerson { get; set; }
    
    public Dto.JuridicalPersonModel? JuridicalPerson { get; set; }
    
    public bool Updatable { get; set; }
    
    public bool IsLegallyIncapable { get; set; }
    
    public DateTime? LegallyIncapableToDate { get; set; }
    
    public List<CIS.Foms.Types.Address>? Addresses { get; set; }
    
    public List<SharedDto.CustomerContact>? Contacts { get; set; }
    
    public SharedDto.IdentificationDocumentFull? IdentificationDocument { get; set; }
}