namespace DomainServices.ProductService.Api.Database.Models;

internal sealed class Covenant
{
    public long CaseId { get; set; }
    public int Order { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Text { get; set; }
    public short? IsFulFilled { get; set; }
    public DateTime? FulfillDate { get; set; }
    public string? OrderLetter { get; set; }
    public int? CovenantTypeId { get; set; }
    public short? PhaseOrder { get; set; }
}