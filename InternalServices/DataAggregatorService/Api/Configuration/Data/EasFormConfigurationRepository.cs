using CIS.InternalServices.DataAggregatorService.Api.Configuration.EasForm;
using Microsoft.EntityFrameworkCore;

namespace CIS.InternalServices.DataAggregatorService.Api.Configuration.Data;

internal class EasFormConfigurationRepository
{
    private readonly ConfigurationContext _dbContext;

    public EasFormConfigurationRepository(ConfigurationContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<List<DynamicInputParameter>> LoadEasFormDynamicInputFields(int requestTypeId, CancellationToken cancellationToken) =>
        _dbContext.EasFormDynamicInputParameters
                  .AsNoTracking()
                  .Where(e => e.EasRequestTypeId == requestTypeId)
                  .Select(e => new DynamicInputParameter
                  {
                      InputParameterName = e.InputParameter.InputParameterName,
                      TargetDataSource = (DataSource)e.TargetDataService.DataServiceId,
                      SourceDataSource = (DataSource)e.SourceDataField.DataServiceId,
                      SourceFieldPath = e.SourceDataField.FieldPath
                  })
                  .ToListAsync(cancellationToken);

    public Task<List<EasFormSourceField>> LoadEasFormSourceFields(int requestTypeId, CancellationToken cancellationToken) => 
        GetSourceFields(requestTypeId).Concat(GetSpecialSourceFields(requestTypeId)).ToListAsync(cancellationToken);

    private IQueryable<EasFormSourceField> GetSourceFields(int requestTypeId) =>
        _dbContext.EasFormDataFields
                  .AsNoTracking()
                  .Where(e => e.EasRequestTypeId == requestTypeId &&
                              e.EasFormType.ValidFrom < DateTime.Now &&
                              e.EasFormType.ValidTo > DateTime.Now)
                  .Select(e => new EasFormSourceField
                  {
                      SourceFieldId = e.EasFormTypeId,
                      DataSource = (DataSource)e.DataField.DataServiceId,
                      FormTypeString = e.EasFormType.EasFormTypeName,
                      FieldPath = e.DataField.FieldPath,
                      JsonPropertyName = e.JsonPropertyName
                  });

    private IQueryable<EasFormSourceField> GetSpecialSourceFields(int requestTypeId) =>
        _dbContext.EasFormSpecialDataFields
                  .AsNoTracking()
                  .Where(e => e.EasRequestTypeId == requestTypeId &&
                              e.EasFormType.ValidFrom < DateTime.Now &&
                              e.EasFormType.ValidTo > DateTime.Now)
                  .Select(e => new EasFormSourceField
                  {
                      SourceFieldId = default,
                      DataSource = (DataSource)e.DataServiceId,
                      FormTypeString = e.EasFormType.EasFormTypeName,
                      FieldPath = e.FieldPath,
                      JsonPropertyName = e.JsonPropertyName
                  });
}