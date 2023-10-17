namespace DomainServices.CodebookService.Api.Configuration;

internal sealed class AppConfiguration
{
    public List<RdmCodebookSettings>? RdmCodebooksToUpdate { get; set; }

    public sealed class RdmCodebookSettings
    {
        public string CodebookName { get; set; } = string.Empty;
    }
}
