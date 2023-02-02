using DomainServices.CodebookService.Contracts.Endpoints.DocumentTypes;

namespace DomainServices.CodebookService.Endpoints.DocumentTypes;

public class DocumentTypesHandler
    : IRequestHandler<DocumentTypesRequest, List<DocumentTypeItem>>
{

    #region Construction

    private readonly CIS.Core.Data.IConnectionProvider _connectionProviderCodebooks;
    private readonly ILogger<DocumentTypesHandler> _logger;

    public DocumentTypesHandler(
        CIS.Core.Data.IConnectionProvider connectionProviderCodebooks,
        ILogger<DocumentTypesHandler> logger)
    {
        _logger = logger;
        _connectionProviderCodebooks = connectionProviderCodebooks;
    }

    #endregion

   
    // query
    const string _sql = @"SELECT [Id], Id 'EnumValue', [ShortName],[Name],[FileName],[SalesArrangementTypeId],[EACodeMainId],[IsFormIdRequested],CASE WHEN SYSDATETIME() BETWEEN[ValidFrom] AND ISNULL([ValidTo], '9999-12-31') THEN 1 ELSE 0 END 'IsValid' 
                          FROM [dbo].[DocumentTypes] ORDER BY [Id]";

    public async Task<List<DocumentTypeItem>> Handle(DocumentTypesRequest request, CancellationToken cancellationToken)
    {
        return await FastMemoryCache.GetOrCreate<DocumentTypeItem>(nameof(DocumentTypesHandler), async () =>
        {
            // load codebook items
            return await _connectionProviderCodebooks.ExecuteDapperRawSqlToList<DocumentTypeItem>(_sql, cancellationToken);
        });
    }
}