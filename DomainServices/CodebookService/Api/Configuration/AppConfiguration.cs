namespace DomainServices.CodebookService.Api.Configuration;

internal sealed class AppConfiguration
{
    public List<RdmCodebookSettings>? RdmCodebooksToUpdate { get; set; }

    public sealed class RdmCodebookSettings
    {
        /// <summary>
        /// Kod ciselniku v KB
        /// </summary>
        public string CodebookName { get; set; } = string.Empty;
        
        /// <summary>
        /// True pokud se jedna o mapping, nikoliv ciselnik
        /// </summary>
        public bool IsMapping { get; set; }

        /// <summary>
        /// Pokud je nastaveno, budou se k nam do DB ukladat pouze pole zminena zde (pouze pro codebooks, pro mappings to nema smysl)
        /// </summary>
        public string[]? Fields { get; set; }
    }
}
