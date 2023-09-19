namespace DomainServices.ProductService.Api.Database.Models;

internal class CovenantPhase
{
    public long CaseId { get; set; }
    public string? Name { get; set; }
    public short Order { get; set; }
    public string? OrderLetter { get; set; }
}