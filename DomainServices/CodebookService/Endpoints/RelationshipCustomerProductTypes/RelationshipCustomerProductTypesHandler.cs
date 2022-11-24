using DomainServices.CodebookService.Contracts.Endpoints.RelationshipCustomerProductTypes;

namespace DomainServices.CodebookService.Endpoints.RelationshipCustomerProductTypes;

public class RelationshipCustomerRelationshipCustomerProductTypesHandler
    : IRequestHandler<RelationshipCustomerProductTypesRequest, List<RelationshipCustomerProductTypeItem>>
{

    #region Construction

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProviderXxd;
    private readonly CIS.Core.Data.IConnectionProvider _connectionProviderCodebooks;
    private readonly ILogger<RelationshipCustomerRelationshipCustomerProductTypesHandler> _logger;

    public RelationshipCustomerRelationshipCustomerProductTypesHandler(
        CIS.Core.Data.IConnectionProvider connectionProviderCodebooks,
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProviderXxd,
        ILogger<RelationshipCustomerRelationshipCustomerProductTypesHandler> logger)
    {
        _logger = logger;
        _connectionProviderCodebooks = connectionProviderCodebooks;
        _connectionProviderXxd = connectionProviderXxd;
    }
    #endregion

    #region Extension

    private class RelationshipCustomerProductTypeExtension
    {
        public int RelationshipCustomerProductTypeId { get; set; }
        public string RdmCode { get; set; } = null!;
        public string MpDigiApiCode { get; set; } = null!;
        public string NameNoby { get; set; } = null!;
    }

    #endregion

    // dotaz na codebook do SB
    const string _sql = "SELECT [ID_VZTAHU] 'Id', [POPIS_VZTAHU] 'Name' FROM [SBR].[VZTAH] ORDER BY [ID_VZTAHU] ASC";

    // dotaz na extenstion
    const string _sqlExtension = "SELECT [RelationshipCustomerProductTypeId], [RdmCode], [MpDigiApiCode], [NameNoby] FROM [dbo].[RelationshipCustomerProductTypeExtension]";

    public async Task<List<RelationshipCustomerProductTypeItem>> Handle(RelationshipCustomerProductTypesRequest request, CancellationToken cancellationToken)
    {
        return await FastMemoryCache.GetOrCreate<RelationshipCustomerProductTypeItem>(nameof(RelationshipCustomerRelationshipCustomerProductTypesHandler), async () =>
        {
            // load codebook items from XXD
            var items = await _connectionProviderXxd.ExecuteDapperRawSqlToList<RelationshipCustomerProductTypeItem>(_sql, cancellationToken);

            // load extensions
            var extensions = await _connectionProviderCodebooks.ExecuteDapperRawSqlToList<RelationshipCustomerProductTypeExtension>(_sqlExtension, cancellationToken);
            var dictExtensions = extensions.ToDictionary(i => i.RelationshipCustomerProductTypeId);

            // assign MpHome mapping to items
            items.ForEach(item =>
            {
                var itemExt = dictExtensions.ContainsKey(item.Id) ? dictExtensions[item.Id] : null;
                item.RdmCode = itemExt?.RdmCode;
                item.MpDigiApiCode = itemExt?.MpDigiApiCode;
                item.NameNoby = itemExt?.NameNoby;
            });

            return items;
        });
    }

}
