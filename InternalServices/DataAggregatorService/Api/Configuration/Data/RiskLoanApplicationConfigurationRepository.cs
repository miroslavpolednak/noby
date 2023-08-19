using CIS.InternalServices.DataAggregatorService.Api.Configuration.RiskLoanApplication;
using Microsoft.EntityFrameworkCore;

namespace CIS.InternalServices.DataAggregatorService.Api.Configuration.Data;

internal class RiskLoanApplicationConfigurationRepository
{
    private readonly ConfigurationContext _dbContext;

    public RiskLoanApplicationConfigurationRepository(ConfigurationContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<RiskLoanApplicationSourceField>> LoadSourceFields(CancellationToken cancellationToken)
    {
        var sourceFieldsQuery = _dbContext.RiskLoanApplicationDataFields
                                          .AsNoTracking()
                                          .Select(r => new RiskLoanApplicationSourceField
                                          {
                                              DataSource = (DataSource)r.DataField.DataServiceId,
                                              FieldPath = r.DataField.FieldPath,
                                              JsonPropertyName = r.JsonPropertyName,
                                              UseDefaultInsteadOfNull = r.UseDefaultInsteadOfNull
                                          });

        var specialSourceFieldsQuery = _dbContext.RiskLoanApplicationSpecialDataFields
                                                 .AsNoTracking()
                                                 .Select(r => new RiskLoanApplicationSourceField
                                                 {
                                                     DataSource = (DataSource)r.DataServiceId,
                                                     FieldPath = r.FieldPath,
                                                     JsonPropertyName = r.JsonPropertyName,
                                                     UseDefaultInsteadOfNull = r.UseDefaultInsteadOfNull
                                                 });

        return await sourceFieldsQuery.Concat(specialSourceFieldsQuery).ToListAsync(cancellationToken);
    }
}