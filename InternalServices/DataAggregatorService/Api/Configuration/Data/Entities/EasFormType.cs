namespace CIS.InternalServices.DataAggregatorService.Api.Configuration.Data.Entities;

internal class EasFormType
{
    public int EasFormTypeId { get; set; }

    public string EasFormTypeName { get; set; } = null!;

    public int Version { get; set; }

    public DateTime ValidFrom { get; set; }

    public DateTime ValidTo { get; set; }
}