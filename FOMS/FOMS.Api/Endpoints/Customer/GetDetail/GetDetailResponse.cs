namespace FOMS.Api.Endpoints.Customer.GetDetail;

public class GetDetailResponse
{
    public List<CIS.Foms.Types.CustomerIdentity>? Identities { get; set; }
    public Dto.NaturalPersonModel? NaturalPerson { get; set; }
    public Dto.JuridicalPersonModel? JuridicalPerson { get; set; }
    
    public bool Updatable { get; set; }
    
    public bool IsLegallyIncapable { get; set; }
    
    public DateTime? LegallyIncapableToDate { get; set; }
    
    public List<Dto.AddressModel>? Addresses { get; set; }
    
    public List<Dto.ContactModel>? Contacts { get; set; }
    
    public Dto.IdentificationDocumentModel? IdentificationDocument { get; set; }
}