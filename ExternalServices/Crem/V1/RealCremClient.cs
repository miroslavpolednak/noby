using System.Net;
using CIS.Infrastructure.ExternalServicesHelpers;
using ExternalServices.Crem.V1.Contracts;

namespace ExternalServices.Crem.V1;

internal sealed class RealCremClient(HttpClient _httpClient)
    : ICremClient
{
    public async Task<ResponseGetFlatsForAddressDTO> GetFlatsForAddress(long addressPointId, CancellationToken cancellationToken = default)
    {
        return await (await _httpClient
            .GetAsync(_httpClient.BaseAddress + $"/deed-of-ownership-document/addresses/{addressPointId}/flats", cancellationToken)
            .ConfigureAwait(false))
            .EnsureSuccessStatusAndReadJson<Contracts.ResponseGetFlatsForAddressDTO>(StartupExtensions.ServiceName, cancellationToken);
    }

    public async Task<ICollection<DeedOfOwnershipDocument>> GetDocuments(int? katuzId, int? deedOfOwnershipNumber, long? deedOfOwnershipId, CancellationToken cancellationToken = default)
    {
        var result = await (await _httpClient
            .GetAsync(_httpClient.BaseAddress + $"/deed-of-ownership-document?territoryNumber={katuzId}&deedOfOwnershipNumber={deedOfOwnershipNumber}&deedOfOwnershipIsknId={deedOfOwnershipId}", cancellationToken)
            .ConfigureAwait(false))
            .EnsureSuccessStatusAndReadJson<Contracts.ResponseSearchDeedOfOwnershipDocuments>(StartupExtensions.ServiceName, cancellationToken);
        return result.Items;
    }

    public async Task<(long CremDeedOfOwnershipDocumentId, int DeedOfOwnershipNumber)> RequestNewDocumentId(int? katuzId, int? deedOfOwnershipNumber, long? deedOfOwnershipId, CancellationToken cancellationToken = default)
    {
        var request1 = new RequestDownloadDeedOfOwnership
        {
            DeedOfOwnershipNumber = deedOfOwnershipNumber,
            IsknDeedOfOwnershipId = deedOfOwnershipId,
            TerritoryNumber = katuzId
        };

        var result1 = await (await _httpClient
            .PostAsJsonAsync(_httpClient.BaseAddress + "/deed-of-ownership-document", request1, cancellationToken)
            .ConfigureAwait(false))
            .EnsureSuccessStatusAndReadJson<DeedOfOwnershipDocument>(StartupExtensions.ServiceName, customErrorCodes: new Dictionary<HttpStatusCode, int> { { HttpStatusCode.NotFound, 404 } }, cancellationToken);

        return (result1.DocumentId, result1.DeedOfOwnershipNumber);
    }

    public async Task<bool> TryToConfirmDownload(long documentId, CancellationToken cancellationToken)
    {
        for (int i = 1; i <= _requestNewDocumentIterations; i++)
        {
            var status = await getStatus(documentId, cancellationToken);

            // pokud je dokument v pozadovanem statusu, muzeme pokracovat dale
            if (status == DocumentStatusStatusCode.WAITING_FOR_PRICE_CONFIRMATION)
            {
                break;
            }
            // jestlize ani v posledni iteraci neprisel pozadovany status, je to maler
            else if (i == _requestNewDocumentIterations)
            {
                return false;
            }
            else
            {
                Thread.Sleep(_requestNewDocumentTimeout * i);
            }
        }

        await confirmDownload(documentId, cancellationToken);

        return true;
    }

    public async Task<ICollection<DeedOfOwnershipOwnerDTO>> GetOwners(long documentId, CancellationToken cancellationToken = default)
    {
        var result = await (await _httpClient
            .GetAsync(_httpClient.BaseAddress + $"/deed-of-ownership-document/{documentId}/owners", cancellationToken)
            .ConfigureAwait(false))
            .EnsureSuccessStatusAndReadJson<Contracts.ResponseGetOwners>(StartupExtensions.ServiceName, cancellationToken);
        return result.Items;
    }

    public async Task<ICollection<DeedOfOwnershipLegalRelation>> GetLegalRelations(long documentId, CancellationToken cancellationToken = default)
    {
        var result = await (await _httpClient
            .GetAsync(_httpClient.BaseAddress + $"/deed-of-ownership-document/{documentId}/legal-relations", cancellationToken)
            .ConfigureAwait(false))
            .EnsureSuccessStatusAndReadJson<Contracts.ResponseGetLegalRelations>(StartupExtensions.ServiceName, cancellationToken);
        return result.Items;
    }

    public async Task<ICollection<DeedOfOwnershipRealEstate>> GetRealEstates(long documentId, CancellationToken cancellationToken = default)
    {
        var result = await (await _httpClient
            .GetAsync(_httpClient.BaseAddress + $"/deed-of-ownership-document/{documentId}/real-estates", cancellationToken)
            .ConfigureAwait(false))
            .EnsureSuccessStatusAndReadJson<Contracts.ResponseGetRealEstates>(StartupExtensions.ServiceName, cancellationToken);
        return result.Items;
    }

    private async Task<DocumentStatusStatusCode> getStatus(long documentId, CancellationToken cancellationToken)
    {
        var result2 = await (await _httpClient
            .GetAsync(_httpClient.BaseAddress + $"/deed-of-ownership-document/{documentId}/status", cancellationToken)
            .ConfigureAwait(false))
            .EnsureSuccessStatusAndReadJson<DocumentStatus>(StartupExtensions.ServiceName, cancellationToken);
        return result2.StatusCode;
    }

    private async Task confirmDownload(long documentId, CancellationToken cancellationToken)
    {
        await (await _httpClient
           .PutAsync(_httpClient.BaseAddress + $"/deed-of-ownership-document/{documentId}", null, cancellationToken)
           .ConfigureAwait(false))
           .EnsureSuccessStatusCode(StartupExtensions.ServiceName, cancellationToken);
    }

    private const int _requestNewDocumentIterations = 10;
    private const int _requestNewDocumentTimeout = 1000;
}
