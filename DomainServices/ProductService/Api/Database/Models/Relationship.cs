namespace DomainServices.ProductService.Api.Database.Models;

internal sealed class Relationship
{
    public long PartnerId { get; set; }
    public int ContractRelationshipTypeId { get; set; }
    public long? KbId { get; set; }
    public bool? Agent { get; set; }
    public bool? Kyc { get; set; }
}