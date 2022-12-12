using CIS.InternalServices.DocumentDataAggregator.Configuration.Document;
using CIS.InternalServices.DocumentDataAggregator.Configuration.EasForm;
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

    public Task<List<DocumentSourceField>> LoadDocumentSourceFields(int documentId, string documentVersion)
    {
        return GetSourceFieldsQuery().Concat(GetSpecialSourceFields()).ToListAsync();

        IQueryable<DocumentSourceField> GetSourceFieldsQuery()
        {
            return _dbContext.DocumentDataFields
                             .AsNoTracking()
                             .Where(f => f.DocumentId == documentId && f.DocumentVersion == documentVersion)
                             .Select(f => new DocumentSourceField
                             {
                                 SourceFieldId = f.DocumentDataFieldId,
                                 DataSource = (DataSource)f.DataField.DataServiceId,
                                 FieldPath = f.DataField.FieldPath,
                                 AcroFieldName = f.AcroFieldName,
                                 StringFormat = Convert.ToString(f.StringFormat ?? f.DataField.DefaultStringFormat),
                                 DefaultTextIfNull =  Convert.ToString(f.DefaultTextIfNull)
                             });
        }

        IQueryable<DocumentSourceField> GetSpecialSourceFields()
        {
            return _dbContext.DocumentSpecialDataFields
                             .AsNoTracking()
                             .Where(f => f.DocumentId == documentId)
                             .Select(f => new DocumentSourceField
                             {
                                 SourceFieldId = default,
                                 DataSource = (DataSource)f.DataServiceId,
                                 FieldPath = f.FieldPath,
                                 AcroFieldName = f.AcroFieldName,
                                 StringFormat = Convert.ToString(f.StringFormat),
                                 DefaultTextIfNull = Convert.ToString(default(string))
                             });
        }
    }

    public async Task<ILookup<int, DocumentDynamicStringFormat>> LoadDocumentDynamicStringFormats(int documentId, string documentVersion)
    {
        var data = await _dbContext.DynamicStringFormats
                                   .AsNoTracking()
                                   .Where(x => x.DocumentDataField.DocumentId == documentId && x.DocumentDataField.DocumentVersion == documentVersion)
                                   .Include(x => x.DynamicStringFormatConditions)
                                   .ThenInclude(x => x.DataField)
                                   .AsSplitQuery()
                                   .Select(x => new DocumentDynamicStringFormat
                                   {
                                       SourceFieldId = x.DocumentDataField.DocumentDataFieldId,
                                       Format = x.Format,
                                       Priority = x.Priority,
                                       Conditions = x.DynamicStringFormatConditions.Select(c => new DocumentDynamicStringFormatCondition
                                       {
                                           SourceFieldPath = c.DataField.FieldPath,
                                           EqualToValue = c.EqualToValue
                                       }).ToList()
                                   }).ToListAsync();

        return data.ToLookup(x => x.SourceFieldId);
    }

    public Task<List<DocumentTable>> LoadDocumentTables(int documentId, string documentVersion)
    {
        return _dbContext.DocumentTables
                         .AsNoTracking()
                         .Where(x => x.DocumentId == documentId && x.DocumentVersion == documentVersion)
                         .Include(x => x.DocumentTableColumns)
                         .AsSplitQuery()
                         .Select(x => new DocumentTable
                         {
                             AcroFieldPlaceholder = x.AcroFieldPlaceholderName,
                             DataSource = (DataSource)x.DataField.DataServiceId,
                             CollectionSourcePath = x.DataField.FieldPath,
                             Columns = x.DocumentTableColumns.OrderBy(c => c.Order).Select(c => new DocumentTable.Column
                             {
                                 CollectionFieldPath = c.FieldPath,
                                 Header = c.Header,
                                 WidthPercentage = c.WidthPercentage,
                                 StringFormat = c.StringFormat
                             }).ToList(),
                             ConcludingParagraph = x.ConcludingParagraph
                         })
                         .ToListAsync();
    }

    public Task<List<EasFormSourceField>> LoadEasFormSourceFields(int requestTypeId)
    {
        return GetSourceFields().Concat(GetSpecialSourceFields()).ToListAsync();

        IQueryable<EasFormSourceField> GetSourceFields()
        {
            return _dbContext.EasFormDataFields
                             .AsNoTracking()
                             .Where(e => e.EasRequestTypeId == requestTypeId)
                             .Select(e => new EasFormSourceField
                             {
                                 SourceFieldId = e.EasFormTypeId,
                                 DataSource = (DataSource)e.DataField.DataServiceId,
                                 FormType = (EasFormType)e.EasFormTypeId,
                                 FieldPath = e.DataField.FieldPath,
                                 JsonPropertyName = e.JsonPropertyName
                             });
        }

        IQueryable<EasFormSourceField> GetSpecialSourceFields()
        {
            return _dbContext.EasFormSpecialDataFields
                             .AsNoTracking()
                             .Where(e => e.EasRequestTypeId == requestTypeId)
                             .Select(e => new EasFormSourceField
                             {
                                 SourceFieldId = default,
                                 DataSource = (DataSource)e.DataServiceId,
                                 FormType = (EasFormType)e.EasFormTypeId,
                                 FieldPath = e.FieldPath,
                                 JsonPropertyName = e.JsonPropertyName
                             });
        }
    }

    public Task<List<DynamicInputParameter>> LoadEasFormDynamicInputFields(int requestTypeId)
    {
        return _dbContext.EasFormDynamicInputParameters
                         .AsNoTracking()
                         .Where(e => e.EasRequestTypeId == requestTypeId)
                         .Select(e => new DynamicInputParameter
                         {
                             InputParameterName = e.InputParameter.InputParameterName,
                             TargetDataSource = (DataSource)e.TargetDataService.DataServiceId,
                             SourceDataSource = (DataSource)e.SourceDataField.DataServiceId,
                             SourceFieldPath = e.SourceDataField.FieldPath
                         })
                         .ToListAsync();
    }
}