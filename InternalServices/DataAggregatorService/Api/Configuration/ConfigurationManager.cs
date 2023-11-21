using CIS.InternalServices.DataAggregatorService.Api.Configuration.Repositories;

namespace CIS.InternalServices.DataAggregatorService.Api.Configuration;

[TransientService, AsImplementedInterfacesService]
internal class ConfigurationManager : IServiceConfigurationManager
{
    private readonly ConfigurationRepositoryFactory _repositoryFactory;

    public ConfigurationManager(ConfigurationRepositoryFactory repositoryFactory)
    {
        _repositoryFactory = repositoryFactory;
    }

    public async Task<Document.DocumentConfiguration> LoadDocumentConfiguration(Document.DocumentKey documentKey, CancellationToken cancellationToken)
    {
        var repository = _repositoryFactory.CreateDocumentRepository();

        return new Document.DocumentConfiguration
        {
            DocumentTemplateVersionId = documentKey.VersionData.VersionId,
            DocumentTemplateVariantId = documentKey.VersionData.VariantId,
            SourceFields = await repository.LoadDocumentSourceFields(documentKey, cancellationToken),
            DynamicInputParameters = await repository.LoadDocumentDynamicInputParameters(documentKey, cancellationToken),
            DynamicStringFormats = await repository.LoadDocumentDynamicStringFormats(documentKey, cancellationToken),
            Tables = await repository.LoadDocumentTable(documentKey, cancellationToken)
        };
    }

    public async Task<EasForm.EasFormConfiguration> LoadEasFormConfiguration(EasForm.EasFormKey easFormKey, CancellationToken cancellationToken)
    {
        var repository = _repositoryFactory.CreateEasFormConfigurationRepository();

        return new EasForm.EasFormConfiguration
        {
            EasFormKey = easFormKey,
            SourceFields = await repository.LoadEasFormSourceFields(easFormKey, cancellationToken),
            DynamicInputParameters = await repository.LoadEasFormDynamicInputParameters(easFormKey, cancellationToken),
        };
    }

    public async Task<ConfigurationBase<RiskLoanApplication.RiskLoanApplicationSourceField>> LoadRiskLoanApplicationConfiguration(CancellationToken cancellationToken)
    {
        var repository = _repositoryFactory.CreateRiskLoanApplicationRepository();

        return new ConfigurationBase<RiskLoanApplication.RiskLoanApplicationSourceField>
        {
            SourceFields = await repository.LoadSourceFields(cancellationToken),
            DynamicInputParameters = new List<DynamicInputParameter>()
        };
    }
}