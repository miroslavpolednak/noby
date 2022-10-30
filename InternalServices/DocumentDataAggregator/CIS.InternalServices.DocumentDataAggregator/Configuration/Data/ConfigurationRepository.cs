using CIS.InternalServices.DocumentDataAggregator.Configuration.Model;
using Microsoft.EntityFrameworkCore;

namespace CIS.InternalServices.DocumentDataAggregator.Configuration.Data;

[TransientService, SelfService]
internal class ConfigurationRepository
{
    private readonly ConfigurationContext _dbContext;

    public ConfigurationRepository(ConfigurationContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<List<DynamicInputParameter>> LoadDynamicInputFields(int documentId, int documentVersion)
    {
        return _dbContext.DocumentDynamicInputParameters
                         .AsNoTracking()
                         .Where(d => d.DocumentId == documentId && d.DocumentVersion == documentVersion)
                         .Select(d => new DynamicInputParameter
                         {
                             InputParameterName = d.InputParameter.InputParameterName,
                             TargetDataSource = (DataSource)d.TargetDataServiceId,
                             SourceDataSource = (DataSource)d.SourceDataField.DataServiceId,
                             SourceFieldPath = d.SourceDataField.FieldPath
                         })
                         .ToListAsync();
    }

    public Task<List<SourceField>> LoadSourceFields(int documentId, int documentVersion)
    {
        return GetSourceFieldsQuery().Union(GetSpecialSourceFields()).ToListAsync();

        IQueryable<SourceField> GetSourceFieldsQuery()
        {
            return _dbContext.DocumentDataFields
                             .AsNoTracking()
                             .Where(f => f.DocumentId == documentId && f.DocumentVersion == documentVersion)
                             .Select(f => new SourceField
                             {
                                 SourceFieldId = f.DataFieldId,
                                 DataSource = (DataSource)f.DataField.DataServiceId,
                                 FieldPath = f.DataField.FieldPath,
                                 TemplateFieldName = f.TemplateFieldName,
                                 StringFormat = f.StringFormat ?? f.DataField.DefaultStringFormat
                             });
        }

        IQueryable<SourceField> GetSpecialSourceFields()
        {
            return _dbContext.DocumentSpecialDataFields
                             .AsNoTracking()
                             .Where(f => f.DocumentId == documentId)
                             .Select(f => new SourceField
                             {
                                 SourceFieldId = default,
                                 DataSource = (DataSource)f.DataServiceId,
                                 FieldPath = Convert.ToString(f.FieldPath),
                                 TemplateFieldName = f.TemplateFieldName,
                                 StringFormat = default
                             });
        }
    }

    public async Task<ILookup<int, DynamicStringFormat>> LoadDocumentDynamicStringFormats()
    {
        var data = await _dbContext.DynamicStringFormats
                                   .AsNoTracking()
                                   .Where(x => x.DocumentId == 1 && x.DocumentVersion == 1)
                                   .Include(x => x.DynamicStringFormatConditions)
                                   .ThenInclude(x => x.DynamicStringFormatDataField)
                                   .AsSplitQuery()
                                   .Select(x => new DynamicStringFormat
                                   {
                                       SourceFieldId = x.DataFieldId,
                                       Format = x.Format,
                                       Priority = x.Priority,
                                       Conditions = x.DynamicStringFormatConditions.Select(c => new DynamicStringFormatCondition
                                       {
                                           SourceFieldPath = c.DynamicStringFormatDataField.FieldPath,
                                           EqualToValue = c.EqualToValue
                                       }).ToList()
                                   }).ToListAsync();

        return data.ToLookup(x => x.SourceFieldId);
    }
}