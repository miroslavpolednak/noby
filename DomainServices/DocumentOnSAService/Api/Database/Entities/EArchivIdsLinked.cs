namespace DomainServices.DocumentOnSAService.Api.Database.Entities;

public class EArchivIdsLinked
{
    public int Id { get; set; }

    public int DocumentOnSAId { get; set; }

    public DocumentOnSa DocumentOnSa { get; set; } = null!;

    public string EArchivId { get; set; } = null!;
}
