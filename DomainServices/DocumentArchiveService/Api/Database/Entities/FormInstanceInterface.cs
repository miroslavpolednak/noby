namespace DomainServices.DocumentArchiveService.Api.Database.Entities;

public class FormInstanceInterface
{
    public string DocumentId { get; set; } = null!;

    public string? FormType { get; set; }

    public short? Status { get; set; }

    public string? FormKind { get; set; }

    public string? Cpm { get; set; }

    public string? Icp { get; set; }

    public DateTime? CreatedAt { get; set; }

    public short? Storno { get; set; }

    public short? DataType { get; set; }

    public string? JsonDataClob { get; set; }

    public DocumentInterface? Document { get; set; }
}
