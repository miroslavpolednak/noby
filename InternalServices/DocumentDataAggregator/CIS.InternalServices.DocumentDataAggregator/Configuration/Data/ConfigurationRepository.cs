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

    public Task<List<DynamicInputParameter>> LoadDocumentDynamicInputFields(int documentId, string documentVersion)
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

    public Task<List<SourceField>> LoadSourceFields(int documentId, string documentVersion)
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
                                 StringFormat = Convert.ToString(f.StringFormat ?? f.DataField.DefaultStringFormat)
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
                                 FieldPath = f.FieldPath,
                                 TemplateFieldName = f.TemplateFieldName,
                                 StringFormat = default
                             });
        }
    }

    public async Task<ILookup<int, DocumentDynamicStringFormat>> LoadDocumentDynamicStringFormats(int documentId, string documentVersion)
    {
        var data = await _dbContext.DynamicStringFormats
                                   .AsNoTracking()
                                   .Where(x => x.DocumentId == documentId && x.DocumentVersion == documentVersion)
                                   .Include(x => x.DynamicStringFormatConditions)
                                   .ThenInclude(x => x.DynamicStringFormatDataField)
                                   .AsSplitQuery()
                                   .Select(x => new DocumentDynamicStringFormat
                                   {
                                       SourceFieldId = x.DataFieldId,
                                       Format = x.Format,
                                       Priority = x.Priority,
                                       Conditions = x.DynamicStringFormatConditions.Select(c => new DocumentDynamicStringFormatCondition
                                       {
                                           SourceFieldPath = c.DynamicStringFormatDataField.FieldPath,
                                           EqualToValue = c.EqualToValue
                                       }).ToList()
                                   }).ToListAsync();

        return data.ToLookup(x => x.SourceFieldId);
    }
}