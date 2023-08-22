using CIS.Core.Data;
using CIS.Infrastructure.Data;
using CIS.InternalServices.DataAggregatorService.Api.Configuration.EasForm;

namespace CIS.InternalServices.DataAggregatorService.Api.Configuration.Repositories;

internal class EasFormConfigurationRepository
{
    private readonly IConnectionProvider _connectionProvider;

    public EasFormConfigurationRepository(IConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    public Task<List<EasFormSourceField>> LoadEasFormSourceFields(EasFormKey easFormKey, CancellationToken cancellationToken)
    {
        const string FieldsQuery =
            """
            SELECT EasFormTypeId as EasFormType, JsonPropertyName, DataServiceId as DataService, FieldPath
            FROM vw_EasFormFields WHERE EasRequestTypeId = @requestTypeId AND EasFormTypeId IN @easFormTypeIds
            ORDER BY JsonPropertyName
            """
        ;

        return _connectionProvider.ExecuteDapperRawSqlToListAsync<EasFormSourceField>(FieldsQuery, easFormKey.CreateSqlParams(), cancellationToken);
    }

    public Task<List<DynamicInputParameter>> LoadEasFormDynamicInputParameters(EasFormKey easFormKey, CancellationToken cancellationToken)
    {
        const string DynamicInputParametersQuery =
            """
            SELECT InputParameter, TargetDataServiceId as TargetDataService, SourceDataServiceId as SourceDataService, SourceFieldPath
            FROM vw_EasFormDynamicInputParameters WHERE EasRequestTypeId = @requestTypeId AND EasFormTypeId IN @easFormTypeIds
            """;

        return _connectionProvider.ExecuteDapperRawSqlToListAsync<DynamicInputParameter>(DynamicInputParametersQuery, easFormKey.CreateSqlParams(), cancellationToken);
    }
}