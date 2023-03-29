namespace DomainServices.DocumentOnSAService.Api.Database.Entities;

public class GeneratedFormId
{
    public long Id { get; set; }

    public int? HouseholdId { get; set; }

    public short Version { get; set; }

    public bool IsFormIdFinal { get; set; }

    public string TargetSystem { get; set; } = null!;
}
