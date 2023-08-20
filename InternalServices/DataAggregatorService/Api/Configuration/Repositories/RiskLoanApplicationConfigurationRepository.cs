using CIS.Core.Data;
using CIS.Infrastructure.Data;
using CIS.InternalServices.DataAggregatorService.Api.Configuration.RiskLoanApplication;

namespace CIS.InternalServices.DataAggregatorService.Api.Configuration.Repositories;

internal class RiskLoanApplicationConfigurationRepository
{
    private readonly IConnectionProvider _connectionProvider;

    public RiskLoanApplicationConfigurationRepository(IConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    public Task<List<RiskLoanApplicationSourceField>> LoadSourceFields(CancellationToken cancellationToken)
    {
        const string FieldsQuery = "SELECT DataServiceId as DataService, FieldPath, JsonPropertyName, UseDefaultInsteadOfNull FROM vw_RiskLoanApplicationFields";

        return _connectionProvider.ExecuteDapperRawSqlToListAsync<RiskLoanApplicationSourceField>(FieldsQuery, cancellationToken);
    }
}