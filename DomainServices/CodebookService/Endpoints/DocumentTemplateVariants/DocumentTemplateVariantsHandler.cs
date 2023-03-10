using DomainServices.CodebookService.Contracts.Endpoints.DocumentTemplateVariants;

namespace DomainServices.CodebookService.Endpoints.DocumentTemplateVariants;
internal class DocumentTemplateVariantsHandler
    : IRequestHandler<DocumentTemplateVariantsRequest, List<DocumentTemplateVariantItem>>
{
    #region Construction

    const string _sqlQuery = @"SELECT Id, DocumentTemplateVersionId, DocumentVariant, Description FROM [dbo].[DocumentTemplateVariant] ORDER BY Id ASC";

    private readonly CIS.Core.Data.IConnectionProvider _connectionProviderCodebooks;

    public DocumentTemplateVariantsHandler(CIS.Core.Data.IConnectionProvider connectionProviderCodebooks)
    {
        _connectionProviderCodebooks = connectionProviderCodebooks;
    }

    #endregion

    public async Task<List<DocumentTemplateVariantItem>> Handle(DocumentTemplateVariantsRequest request, CancellationToken cancellationToken)
    {
        return await FastMemoryCache.GetOrCreate<DocumentTemplateVariantItem>(nameof(DocumentTemplateVariantsHandler), async () => await _connectionProviderCodebooks.ExecuteDapperRawSqlToList<DocumentTemplateVariantItem>(_sqlQuery, cancellationToken));
    }
}