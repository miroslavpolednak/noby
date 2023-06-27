namespace CIS.InternalServices.DataAggregatorService.Api.Configuration.Data;

internal class RiskLoanApplicationConfigurationRepository
{
    private readonly ConfigurationContext _dbContext;

    public RiskLoanApplicationConfigurationRepository(ConfigurationContext dbContext)
    {
        _dbContext = dbContext;
    }
}