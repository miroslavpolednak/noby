namespace CIS.InternalServices.DataAggregatorService.Api.Configuration.Data;

[TransientService, SelfService]
internal class ConfigurationRepositoryFactory
{
    private readonly ConfigurationContext _dbContext;

    public ConfigurationRepositoryFactory(ConfigurationContext dbContext)
    {
        _dbContext = dbContext;
    }

    public DocumentConfigurationRepository CreateDocumentRepository() => new(_dbContext);

    public EasFormConfigurationRepository CreateEasFormConfigurationRepository() => new(_dbContext);

    public RiskLoanApplicationConfigurationRepository CreateRiskLoanApplicationRepository() => new(_dbContext);
}