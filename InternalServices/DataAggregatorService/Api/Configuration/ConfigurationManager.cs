using CIS.Core.Exceptions;
using CIS.InternalServices.DataAggregatorService.Api.Configuration.Data;
using CIS.InternalServices.DataAggregatorService.Api.Configuration.Document;
using CIS.InternalServices.DataAggregatorService.Api.Configuration.EasForm;
using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts.Endpoints.DocumentTemplateVersions;

namespace CIS.InternalServices.DataAggregatorService.Api.Configuration;

[TransientService, AsImplementedInterfacesService]
internal class ConfigurationManager : IConfigurationManager
{
    private readonly ConfigurationRepository _repository;
    private readonly ICodebookServiceClients _codebookService;

    public ConfigurationManager(ConfigurationRepository repository, ICodebookServiceClients codebookService)
    {
        _repository = repository;
        _codebookService = codebookService;
    }

    public async Task<DocumentConfiguration> LoadDocumentConfiguration(DocumentKey documentKey, CancellationToken cancellationToken)
    {
        var documentVersionData = await GetDocumentVersionData(documentKey, cancellationToken);

        var fields = await _repository.LoadDocumentSourceFields(documentKey.TypeId, documentVersionData.documentVersion);
        var tables = await _repository.LoadDocumentTables(documentKey.TypeId, documentVersionData.documentVersion);

        return new DocumentConfiguration
        {
            DocumentTemplateVersionId = documentVersionData.documentVersionId,
            DocumentTemplateVersion = documentVersionData.documentVersion,
            InputConfig = new InputConfig
            {
                DataSources = GetDataSources(UnionFieldAndTableSources()),
                DynamicInputParameters = await _repository.LoadDocumentDynamicInputFields(documentKey.TypeId, documentVersionData.documentVersion)
            },
            SourceFields = fields,
            DynamicStringFormats = await _repository.LoadDocumentDynamicStringFormats(documentKey.TypeId, documentVersionData.documentVersion),
            Tables = tables
        };

        IEnumerable<DataSource> UnionFieldAndTableSources() => 
            fields.Select(f => f.DataSource).Union(tables.Select(t => t.DataSource));
    }

    public async Task<EasFormConfiguration> LoadEasFormConfiguration(EasFormKey easFormKey, CancellationToken cancellationToken)
    {
        var fields = await _repository.LoadEasFormSourceFields(easFormKey.RequestTypeId, cancellationToken);

        return new EasFormConfiguration
        {
            EasFormKey = easFormKey,
            InputConfig = new InputConfig
            {
                DataSources = GetDataSources(fields.Select(f => f.DataSource)),
                DynamicInputParameters = await _repository.LoadEasFormDynamicInputFields(easFormKey.RequestTypeId, cancellationToken)
            },
            SourceFields = fields
        };
    }

    private async Task<(int documentVersionId, string documentVersion)> GetDocumentVersionData(DocumentKey documentKey, CancellationToken cancellationToken)
    {
        var documentVersions = await _codebookService.DocumentTemplateVersions(cancellationToken);

        var selectedVersion = documentKey.VersionId.HasValue ? GetRequestedVersion() : GetLatestVersion();

        if (selectedVersion is null)
            throw new CisValidationException($"Could not find a version for the document type with id {documentKey.TypeId} " +
                                             $"and requested version {(documentKey.VersionId.HasValue ? documentKey.VersionId.Value.ToString() : "N/A")}");

        return (selectedVersion.Id, selectedVersion.DocumentVersion);

        DocumentTemplateVersionItem? GetRequestedVersion() => documentVersions.FirstOrDefault(d => d.Id == documentKey.VersionId && d.DocumentTemplateTypeId == documentKey.TypeId);
        DocumentTemplateVersionItem? GetLatestVersion() => documentVersions.FirstOrDefault(d => d.DocumentTemplateTypeId == documentKey.TypeId && d.IsValid);
    }

    private static IEnumerable<DataSource> GetDataSources(IEnumerable<DataSource> dataSources) =>
        dataSources.Where(d => d != DataSource.General).Distinct();
}