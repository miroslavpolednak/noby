namespace FOMS.Api.Endpoints.Customer.GetDetail.Dto;

public class IdentificationDocumentModel
{
    public int? IdentificationDocumentTypeId { get; set; }
    
    public DateTime? ValidTo  { get; set; }
    
    public string? Number  { get; set; }
    
    public string? IssuedBy  { get; set; }
    
    public int? IssuingCountryId  { get; set; }
    
    public DateTime? IssuedOn { get; set; }
    
    public string? RegisterPlace { get; set; }
}