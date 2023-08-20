using CIS.Core.Data;

namespace CIS.InternalServices.DataAggregatorService.Api.Configuration.Repositories;

[TransientService, SelfService]
internal class ConfigurationRepositoryFactory
{
    private readonly IConnectionProvider _connectionProvider;

    public ConfigurationRepositoryFactory(IConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    public DocumentConfigurationRepository CreateDocumentRepository() => new(_connectionProvider);

    public EasFormConfigurationRepository CreateEasFormConfigurationRepository() => new(_connectionProvider);

    public RiskLoanApplicationConfigurationRepository CreateRiskLoanApplicationRepository() => new(_connectionProvider);
}