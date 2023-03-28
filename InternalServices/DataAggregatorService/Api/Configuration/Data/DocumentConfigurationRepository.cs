using CIS.InternalServices.DataAggregatorService.Api.Configuration.Document;
using Microsoft.EntityFrameworkCore;

namespace CIS.InternalServices.DataAggregatorService.Api.Configuration.Data;

internal class DocumentConfigurationRepository
{
    private readonly ConfigurationContext _dbContext;

    public DocumentConfigurationRepository(ConfigurationContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<List<DynamicInputParameter>> LoadDocumentDynamicInputFields(int documentId, string documentVersion, string? documentVariant, CancellationToken cancellationToken) =>
        _dbContext.DocumentDynamicInputParameters
                  .AsNoTracking()
                  .Where(d => d.DocumentId == documentId && d.DocumentVersion == documentVersion)
                  .Select(d => new DynamicInputParameter
                  {
                      InputParameterName = d.InputParameter.InputParameterName,
                      TargetDataSource = (DataSource)d.TargetDataServiceId,
                      SourceDataSource = (DataSource)d.SourceDataField.DataServiceId,
                      SourceFieldPath = d.SourceDataField.FieldPath
                  })
                  .ToListAsync(cancellationToken);

    public Task<List<DocumentSourceField>> LoadDocumentSourceFields(int documentId, string documentVersion, string? documentVariant, CancellationToken cancellationToken) =>
        GetSourceFieldsQuery(documentId, documentVersion, documentVariant)
            .Concat(GetSpecialSourceFieldQuery(documentId, documentVariant))
            .ToListAsync(cancellationToken);

    public async Task<ILookup<int, DocumentDynamicStringFormat>> LoadDocumentDynamicStringFormats(int documentId, string documentVersion, CancellationToken cancellationToken)
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
                                   }).ToListAsync(cancellationToken);

        return data.ToLookup(x => x.SourceFieldId);
    }

    public Task<List<DocumentTable>> LoadDocumentTables(int documentId, string documentVersion, CancellationToken cancellationToken)
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
                         .ToListAsync(cancellationToken);
    }

    private IQueryable<DocumentSourceField> GetSourceFieldsQuery(int documentId, string documentVersion, string? documentVariant)
    {
        var query = _dbContext.DocumentDataFields.AsNoTracking().Where(f => f.DocumentId == documentId && f.DocumentVersion == documentVersion);

        if (!string.IsNullOrWhiteSpace(documentVariant))
        {
            query = query.Join(_dbContext.DocumentDataFieldVariants, field => field.DocumentDataFieldId, variant => variant.DocumentDataFieldId, (field, variant) => new { Field = field, Variant = variant })
                         .Where(f => f.Variant.DocumentVariant == documentVariant)
                         .Select(f => f.Field);
        }

        return query.Select(f => new DocumentSourceField
        {
            SourceFieldId = f.DocumentDataFieldId,
            DataSource = (DataSource)f.DataField.DataServiceId,
            FieldPath = f.DataField.FieldPath,
            AcroFieldName = f.AcroFieldName,
            StringFormat = Convert.ToString(f.StringFormat ?? f.DataField.DefaultStringFormat),
            DefaultTextIfNull = Convert.ToString(f.DefaultTextIfNull),
            TextAlign = f.TextAlign
        });
    }

    private IQueryable<DocumentSourceField> GetSpecialSourceFieldQuery(int documentId, string? documentVariant)
    {
        var query = _dbContext.DocumentSpecialDataFields.AsNoTracking().Where(f => f.DocumentId == documentId);

        if (!string.IsNullOrWhiteSpace(documentVariant))
        {
            query = query.Join(_dbContext.DocumentSpecialDataFieldVariants,
                               field => new { field.DocumentId, field.AcroFieldName },
                               variant => new { variant.DocumentId, variant.AcroFieldName },
                               (field, variant) => new { Field = field, Variant = variant })
                         .Where(f => f.Variant.DocumentVariant == documentVariant)
                         .Select(f => f.Field);
        }

        return query.Select(f => new DocumentSourceField
        {
            SourceFieldId = default,
            DataSource = (DataSource)f.DataServiceId,
            FieldPath = f.FieldPath,
            AcroFieldName = f.AcroFieldName,
            StringFormat = Convert.ToString(f.StringFormat),
            DefaultTextIfNull = Convert.ToString(default(string)),
            TextAlign = f.TextAlign
        });
    }
}