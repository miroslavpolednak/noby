using System.Data;
using CIS.Core.Data;
using CIS.Infrastructure.Data;
using CIS.InternalServices.DataAggregatorService.Api.Configuration.Document;
using Dapper;

namespace CIS.InternalServices.DataAggregatorService.Api.Configuration.Repositories;

internal class DocumentConfigurationRepository
{
    private readonly IConnectionProvider _connectionProvider;

    public DocumentConfigurationRepository(IConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    public Task<List<DocumentSourceField>> LoadDocumentSourceFields(DocumentKey documentKey, CancellationToken cancellationToken)
    {
        const string FieldsQuery =
            """
            SELECT AcroFieldName, DataServiceId as DataService, FieldPath, StringFormat, TextAlign, VAlign, DefaultTextIfNull
            FROM vw_DocumentFields
            WHERE DocumentId = @documentId AND DocumentVersion = @documentVersion AND DocumentVariant = @documentVariant
            """;

        return _connectionProvider.ExecuteDapperRawSqlToListAsync<DocumentSourceField>(FieldsQuery, documentKey.CreateSqlParams(), cancellationToken);
    }

    public Task<List<DynamicInputParameter>> LoadDocumentDynamicInputParameters(DocumentKey documentKey, CancellationToken cancellationToken)
    {
        const string DynamicInputParametersQuery =
            """
            SELECT InputParameter, TargetDataServiceId as TargetDataService, SourceDataServiceId as SourceDataService, SourceFieldPath
            FROM vw_DocumentDynamicInputParameters
            WHERE DocumentId = @documentId AND DocumentVersion = @documentVersion
            """;

        return _connectionProvider.ExecuteDapperRawSqlToListAsync<DynamicInputParameter>(DynamicInputParametersQuery, documentKey.CreateSqlParams(), cancellationToken);
    }

    public async Task<ILookup<string, DocumentDynamicStringFormat>> LoadDocumentDynamicStringFormats(DocumentKey documentKey, CancellationToken cancellationToken)
    {
        const string DynamicStringFormatsQuery =
            """
            SELECT AcroFieldName, StringFormat, [Priority], DynamicStringFormatId, EqualToValue, FieldPath FROM vw_DocumentDynamicStringFormats
            WHERE DocumentId = @documentId AND DocumentVersion = @documentVersion
            ORDER BY AcroFieldName, [Priority]
            """;

        var formats = await _connectionProvider.ExecuteDapperQueryAsync(Query, cancellationToken);

        return formats.ToLookup(f => f.AcroFieldName);

        Task<IEnumerable<DocumentDynamicStringFormat>> Query(IDbConnection conn)
        {
            return conn.QueryAsync<DocumentDynamicStringFormat, DocumentDynamicStringFormatCondition, DocumentDynamicStringFormat>(
                new CommandDefinition(DynamicStringFormatsQuery, parameters: documentKey.CreateSqlParams(), cancellationToken: cancellationToken),
                (format, condition) =>
                {
                    format.Conditions.Add(condition);

                    return format;
                },
                splitOn: "DynamicStringFormatId");
        }
    }

    public async Task<List<DocumentTable>> LoadDocumentTable(DocumentKey documentKey, CancellationToken cancellationToken)
    {
        const string TableQuery =
            """
            SELECT DocumentTableId, TableSourcePath, AcroFieldPlaceholderName, ConcludingParagraph
            FROM vw_DocumentTables WHERE DocumentId = @documentId AND DocumentVersion = @documentVersion
            """;

        const string ColumnsQuery =
            """
            SELECT FieldPath, WidthPercentage, StringFormat, Header
            FROM vw_DocumentTableColumns WHERE DocumentTableId = @documentTableId
            ORDER BY [Order]
            """;

        var table = await _connectionProvider.ExecuteDapperRawSqlFirstOrDefaultAsync<DocumentTable>(TableQuery, documentKey.CreateSqlParams(), cancellationToken);

        if (table is null)
            return new List<DocumentTable>();

        var columns = await _connectionProvider.ExecuteDapperRawSqlToListAsync<DocumentTable.Column>(ColumnsQuery, new { table.DocumentTableId }, cancellationToken);

        table.Columns.AddRange(columns);

        return new List<DocumentTable> { table };
    }
}