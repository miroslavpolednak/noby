using System.Globalization;
using CIS.Core.Exceptions;
using CIS.InternalServices.DataAggregatorService.Api.Configuration.Document;
using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts.v1;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.Documents.VersionData;

[TransientService, AsImplementedInterfacesService]
internal class CommonVersionDataProvider : IDocumentVersionDataProvider
{
    private readonly ICodebookServiceClient _codebookService;

    public CommonVersionDataProvider(ICodebookServiceClient codebookService)
    {
        _codebookService = codebookService;
    }

    public async Task<DocumentVersionData> GetDocumentVersionData(GetDocumentDataRequest request, CancellationToken cancellationToken)
    {
        var version = request.DocumentTemplateVersionId.HasValue
            ? await GetRequestedVersion(request.DocumentTypeId, request.DocumentTemplateVersionId.Value, cancellationToken)
            : await GetLatestVersion(request.DocumentTypeId, cancellationToken);

        if (version is null)
            throw new CisValidationException($"Could not find a version for the document type with id {request.DocumentTypeId} " +
                                             $"and requested version {(request.DocumentTemplateVersionId.HasValue ? request.DocumentTemplateVersionId.Value.ToString(CultureInfo.InvariantCulture) : "N/A")}");

        return new DocumentVersionData
        {
            VersionId = version.Id,
            VersionName = version.DocumentVersion
        };
    }

    private async Task<DocumentTemplateVersionsResponse.Types.DocumentTemplateVersionItem?> GetRequestedVersion(int documentTypeId, int versionId, CancellationToken cancellationToken)
    {
        var documentVersions = await _codebookService.DocumentTemplateVersions(cancellationToken);

        return documentVersions.FirstOrDefault(d => d.Id == versionId && d.DocumentTypeId == documentTypeId);
    }

    private async Task<DocumentTemplateVersionsResponse.Types.DocumentTemplateVersionItem?> GetLatestVersion(int documentTypeId, CancellationToken cancellationToken)
    {
        var documentVersions = await _codebookService.DocumentTemplateVersions(cancellationToken);

        return documentVersions.FirstOrDefault(d => d.DocumentTypeId == documentTypeId && d.IsValid);;
    }
}