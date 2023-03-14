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
    private readonly ConfigurationRepositoryFactory _repositoryFactory;
    private readonly ICodebookServiceClients _codebookService;

    public ConfigurationManager(ConfigurationRepositoryFactory repositoryFactory, ICodebookServiceClients codebookService)
    {
        _repositoryFactory = repositoryFactory;
        _codebookService = codebookService;
    }

    public async Task<DocumentConfiguration> LoadDocumentConfiguration(DocumentKey documentKey, CancellationToken cancellationToken)
    {
        var repository = _repositoryFactory.CreateDocumentRepository();

        var documentVersionData = await GetDocumentVersionData(documentKey, cancellationToken);
        var documentVariant = await GetDocumentVariantIfNeeded(documentVersionData.documentVersionId, documentKey.VariantId, cancellationToken);

        var fields = await repository.LoadDocumentSourceFields(documentKey.TypeId, documentVersionData.documentVersion, documentVariant, cancellationToken);
        var tables = await repository.LoadDocumentTables(documentKey.TypeId, documentVersionData.documentVersion, cancellationToken);

        return new DocumentConfiguration
        {
            DocumentTemplateVersionId = documentVersionData.documentVersionId,
            DocumentTemplateVersion = documentVersionData.documentVersion,
            DocumentTemplateVariantId = documentKey.VariantId,
            DocumentTemplateVariant = documentVariant,
            InputConfig = new InputConfig
            {
                DataSources = GetDataSources(UnionFieldAndTableSources()),
                DynamicInputParameters = await repository.LoadDocumentDynamicInputFields(documentKey.TypeId, documentVersionData.documentVersion, documentVariant, cancellationToken)
            },
            SourceFields = fields,
            DynamicStringFormats = await repository.LoadDocumentDynamicStringFormats(documentKey.TypeId, documentVersionData.documentVersion, cancellationToken),
            Tables = tables
        };

        IEnumerable<DataSource> UnionFieldAndTableSources() => 
            fields.Select(f => f.DataSource).Union(tables.Select(t => t.DataSource));
    }

    public async Task<EasFormConfiguration> LoadEasFormConfiguration(EasFormKey easFormKey, CancellationToken cancellationToken)
    {
        var repository = _repositoryFactory.CreateEasFormConfigurationRepository();

        var fields = await repository.LoadEasFormSourceFields(easFormKey.RequestTypeId, cancellationToken);

        return new EasFormConfiguration
        {
            EasFormRequestType = (EasFormRequestType)easFormKey.RequestTypeId,
            InputConfig = new InputConfig
            {
                DataSources = GetDataSources(fields.Select(f => f.DataSource)),
                DynamicInputParameters = await repository.LoadEasFormDynamicInputFields(easFormKey.RequestTypeId, cancellationToken)
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

        DocumentTemplateVersionItem? GetRequestedVersion() => documentVersions.FirstOrDefault(d => d.Id == documentKey.VersionId && d.DocumentTypeId == documentKey.TypeId);
        DocumentTemplateVersionItem? GetLatestVersion() => documentVersions.FirstOrDefault(d => d.DocumentTypeId == documentKey.TypeId && d.IsValid);
    }

    private async Task<string?> GetDocumentVariantIfNeeded(int documentVersionId, int? documentVariantId, CancellationToken cancellationToken)
    {
        var documentVariants = await _codebookService.DocumentTemplateVariants(cancellationToken);

        if (documentVariants.Any(v => v.DocumentTemplateVersionId == documentVersionId) && !documentVariantId.HasValue)
            throw new CisValidationException($"Template Variant is not specified for template version {documentVersionId}");

        var variant = documentVariants.FirstOrDefault(v => v.DocumentTemplateVersionId == documentVersionId && v.Id == documentVariantId);

        if (documentVariantId.HasValue && variant is null)
            throw new CisValidationException($"Template Variant {documentVariantId} does not exist for template version {documentVersionId}");

        return variant?.DocumentVariant;
    }

    private static IEnumerable<DataSource> GetDataSources(IEnumerable<DataSource> dataSources) =>
        dataSources.Where(d => d != DataSource.General).Distinct();
}