using DomainServices.CodebookService.Contracts.Endpoints.RelationshipCustomerProductTypes;

namespace DomainServices.CodebookService.Endpoints.RelationshipCustomerProductTypes;

public class RelationshipCustomerRelationshipCustomerProductTypesHandler
    : IRequestHandler<RelationshipCustomerProductTypesRequest, List<RelationshipCustomerProductTypeItem>>
{

    #region Construction

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProviderXxd;
    private readonly CIS.Core.Data.IConnectionProvider _connectionProviderCodebooks;
    private readonly ILogger<RelationshipCustomerRelationshipCustomerProductTypesHandler> _logger;
    private readonly CIS.Infrastructure.Caching.IGlobalCache<ISharedInMemoryCache> _cache;
    private readonly MediatR.IMediator _mediator;

    public RelationshipCustomerRelationshipCustomerProductTypesHandler(
        MediatR.IMediator mediator,
        CIS.Infrastructure.Caching.IGlobalCache<ISharedInMemoryCache> cache,
        CIS.Core.Data.IConnectionProvider connectionProviderCodebooks,
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProviderXxd,
        ILogger<RelationshipCustomerRelationshipCustomerProductTypesHandler> logger)
    {
        _mediator = mediator;
        _cache = cache;
        _logger = logger;
        _connectionProviderCodebooks = connectionProviderCodebooks;
        _connectionProviderXxd = connectionProviderXxd;
    }

    private const string _cacheKey = "RelationshipCustomerProductTypes";


    #endregion

    #region Extension

    private class RelationshipCustomerProductTypeExtension
    {
        public int RelationshipCustomerProductTypeId { get; set; }
        public string MpHomeApiContractRelationshipType { get; set; }
    }

    #endregion

    // dotaz na codebook do SB
    const string _sql = @"SELECT [ID_VZTAHU] 'Id', [POPIS_VZTAHU] 'Name' FROM [SBR].[VZTAH] ORDER BY [ID_VZTAHU] ASC";

    // dotaz na extenstion
    const string _sqlExtension = @"SELECT [RelationshipCustomerProductTypeId], [MpHomeApiContractRelationshipType] FROM [dbo].[RelationshipCustomerProductTypeExtension]";

    
    public async Task<List<RelationshipCustomerProductTypeItem>> Handle(RelationshipCustomerProductTypesRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (_cache.Exists(_cacheKey))
            {
                _logger.ItemFoundInCache(_cacheKey);
                return await _cache.GetAllAsync<RelationshipCustomerProductTypeItem>(_cacheKey);
            }
            else
            {
                _logger.TryAddItemToCache(_cacheKey);

                // load codebook items from XXD
                var items = await _connectionProviderXxd.ExecuteDapperRawSqlToList<RelationshipCustomerProductTypeItem>(_sql, cancellationToken);

                // load extensions
                var extensions = await _connectionProviderCodebooks.ExecuteDapperRawSqlToList<RelationshipCustomerProductTypeExtension>(_sqlExtension, cancellationToken);
                var dictExtensions = extensions.ToDictionary(i => i.RelationshipCustomerProductTypeId, i => (MpHomeContractRelationshipType)Enum.Parse(typeof(MpHomeContractRelationshipType), i.MpHomeApiContractRelationshipType));

                // assign MpHome mapping to items
                items.ForEach(item => item.MpHomeContractRelationshipType = dictExtensions[item.Id]);

                await _cache.SetAllAsync(_cacheKey, items);

                return items;
            }
        }
        catch (Exception ex)
        {
            _logger.GeneralException(ex);
            throw;
        }
    }

}
