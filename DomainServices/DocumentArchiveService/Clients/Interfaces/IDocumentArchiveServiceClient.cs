namespace DomainServices.DocumentArchiveService.Clients;

public interface IDocumentArchiveServiceClient
{
    /// <summary>
    /// Metoda slouží k vygenerování ID dokumentu které se následně použije pro uložení dokumentu do eArchivu a pro registraci do ESCUDO.
    /// </summary>
    /// <returns><see cref="string"/> DocumentId</returns>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 14009; EnvironmentName hodnota není z enum (DEV, FAT, SIT, UAT, PREPROD, EDU, PROD)</exception>
    /// <exception cref="CIS.Core.Exceptions.CisArgumentException">Code: 14010; EnvironmentIndex není jednociferné číslo</exception>
    /// <exception cref="CIS.Core.Exceptions.CisServiceUnavailableException">Service unavailable</exception>
    Task<IServiceCallResult> GenerateDocumentId(Contracts.EnvironmentNames environmentName, int? environmentIndex = 0, CancellationToken cancellationToken = default(CancellationToken));
}
