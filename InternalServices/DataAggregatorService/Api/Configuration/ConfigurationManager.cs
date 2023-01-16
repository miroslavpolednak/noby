using CIS.Core.Exceptions;
using CIS.InternalServices.DataAggregatorService.Api.Configuration.Data;
using CIS.InternalServices.DataAggregatorService.Api.Configuration.Document;
using CIS.InternalServices.DataAggregatorService.Api.Configuration.EasForm;
using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts.Endpoints.DocumentTemplateVersions;

namespace CIS.InternalServices.DataAggregatorService.Api.Configuration;

[TransientService, SelfService]
internal class ConfigurationManager
{
    private readonly ConfigurationRepository _repository;
    private readonly ICodebookServiceClients _codebookService;

    public ConfigurationManager(ConfigurationRepository repository, ICodebookServiceClients codebookService)
    {
        _repository = repository;
        _codebookService = codebookService;
    }

    public async Task<DocumentConfiguration> LoadDocumentConfiguration(int documentId, int? documentVersionId, CancellationToken cancellationToken)
    {
        var documentVersionData = await GetDocumentVersionData(documentId, documentVersionId, cancellationToken);

        var fields = await _repository.LoadDocumentSourceFields(documentId, documentVersionData.documentVersion);
        var tables = await _repository.LoadDocumentTables(documentId, documentVersionData.documentVersion);

        return new DocumentConfiguration
        {
            DocumentTemplateVersionId = documentVersionData.documentVersionId,
            DocumentTemplateVersion = documentVersionData.documentVersion,
            InputConfig = new InputConfig
            {
                DataSources = GetDataSources(UnionFieldAndTableSources()),
                DynamicInputParameters = await _repository.LoadDocumentDynamicInputFields(documentId, documentVersionData.documentVersion)
            },
            SourceFields = fields,
            DynamicStringFormats = await _repository.LoadDocumentDynamicStringFormats(documentId, documentVersionData.documentVersion),
            Tables = tables
        };

        IEnumerable<DataSource> UnionFieldAndTableSources() => 
            fields.Select(f => f.DataSource).Union(tables.Select(t => t.DataSource));
    }

    public async Task<EasFormConfiguration> LoadEasFormConfiguration(int easFormRequestType)
    {
        var fields = await _repository.LoadEasFormSourceFields(easFormRequestType);

        return new EasFormConfiguration
        {
            InputConfig = new InputConfig
            {
                DataSources = GetDataSources(fields.Select(f => f.DataSource)),
                DynamicInputParameters = await _repository.LoadEasFormDynamicInputFields(easFormRequestType)
            },
            SourceFields = fields
        };
    }

    private async Task<(int documentVersionId, string documentVersion)> GetDocumentVersionData(int documentId, int? documentVersionId, CancellationToken cancellationToken)
    {
        var documentVersions = await _codebookService.DocumentTemplateVersions(cancellationToken);

        var selectedVersion = documentVersionId.HasValue ? GetRequestedVersion() : GetLatestVersion();

        if (selectedVersion is null)
            throw new CisValidationException($"Could not find a version for the document type with id {documentId} " +
                                             $"and requested version {(documentVersionId.HasValue ? documentVersionId.Value.ToString() : "N/A")}");

        return (selectedVersion.Id, selectedVersion.DocumentVersion);

        DocumentTemplateVersionItem? GetRequestedVersion() => documentVersions.FirstOrDefault(d => d.Id == documentVersionId && d.DocumentTemplateTypeId == documentId);
        DocumentTemplateVersionItem? GetLatestVersion() => documentVersions.FirstOrDefault(d => d.DocumentTemplateTypeId == documentId && d.IsValid);
    }

    private static IEnumerable<DataSource> GetDataSources(IEnumerable<DataSource> dataSources) =>
        dataSources.Where(d => d != DataSource.General).Distinct();
}