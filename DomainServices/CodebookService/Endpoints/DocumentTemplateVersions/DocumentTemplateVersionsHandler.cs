using DomainServices.CodebookService.Contracts.Endpoints.DocumentTemplateVersions;

namespace DomainServices.CodebookService.Endpoints.DocumentTemplateVersions;
internal class DocumentTemplateVersionsHandler
    : IRequestHandler<DocumentTemplateVersionsRequest, List<DocumentTemplateVersionItem>>
{

    #region Construction

    const string _sqlQuery = @"SELECT Id, DocumentTypeId, DocumentVersion, FormTypeId, CASE WHEN SYSDATETIME() BETWEEN[ValidFrom] AND ISNULL([ValidTo], '9999-12-31') THEN 1 ELSE 0 END 'IsValid'
                               FROM[dbo].[DocumentTemplateVersion] ORDER BY Id ASC";

    private readonly CIS.Core.Data.IConnectionProvider _connectionProviderCodebooks;

    public DocumentTemplateVersionsHandler(CIS.Core.Data.IConnectionProvider connectionProviderCodebooks)
    {
        _connectionProviderCodebooks = connectionProviderCodebooks;
    }

    #endregion

    public async Task<List<DocumentTemplateVersionItem>> Handle(DocumentTemplateVersionsRequest request, CancellationToken cancellationToken)
    {
        return await FastMemoryCache.GetOrCreate<DocumentTemplateVersionItem>(nameof(DocumentTemplateVersionsHandler), async () => await _connectionProviderCodebooks.ExecuteDapperRawSqlToList<DocumentTemplateVersionItem>(_sqlQuery, cancellationToken));
    }
}