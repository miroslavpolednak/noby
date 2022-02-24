using DomainServices.CodebookService.Contracts.Endpoints.ProductTypes;

namespace DomainServices.CodebookService.Endpoints.ProductTypes;

public class ProductTypesHandler
    : IRequestHandler<ProductTypesRequest, List<ProductTypeItem>>
{
    public async Task<List<ProductTypeItem>> Handle(ProductTypesRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (_cache.Exists(_cacheKey))
            {
                _logger.ItemFoundInCache(_cacheKey);
                return await _cache.GetAllAsync<ProductTypeItem>(_cacheKey);
            }
            else
            {
                _logger.TryAddItemToCache(_cacheKey);

                // vytahnout druhy uveru pro naparovani do kolekci
                var loanKinds = await _mediator.Send(new Contracts.Endpoints.LoanKinds.LoanKindsRequest(), cancellationToken);
                
                // vytahnout mapovani na product category
                var extMapper = await _connectionProviderCodebooks.ExecuteDapperRawSqlToList<ExtensionMapper>(_sqlCodebooks, cancellationToken);

                // vytahnout vlastni ciselnik z XXD
                var result = await _connectionProviderXxd.ExecuteDapperRawSqlToList<ProductTypeItem>(_sql, cancellationToken);
                
                // namapovat kategorie z extensions tabulky
                result.ForEach(t =>
                {
                    // dohrat podrizene LoanKind - jako prasarna, ale nechtelo se mi kvuli tomu delat special objekt
                    if (!string.IsNullOrWhiteSpace(t.MpHomeApiLoanType))
                    {
                        var loanKindIds = t.MpHomeApiLoanType
                            .Split(",", StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                            .Select(x => Convert.ToInt32(x))
                            .ToArray();
                        if (loanKinds.Any())
                            t.LoanKinds = loanKinds.Where(x => loanKindIds.Contains(x.Id)).ToList();
                        t.MpHomeApiLoanType = null;
                        t.KonsDbLoanType = null;
                    }
                        
                    // rozsirena nastaveni z extension tabulky
                    var item = extMapper.FirstOrDefault(x => x.ProductTypeId == t.Id);
                    if (item is not null)
                    {
                        t.ProductCategory = item.ProductCategory.HasValue ? (ProductTypeCategory)item.ProductCategory : ProductTypeCategory.Unknown;
                        t.MpHomeApiLoanType = item.MpHomeApiLoanType;
                        t.KonsDbLoanType = item.KonsDbLoanType;
                    }
                        
                });

                await _cache.SetAllAsync(_cacheKey, result);

                return result;
            }
        }
        catch (Exception ex)
        {
            _logger.GeneralException(ex);
            throw;
        }
    }

    private class ExtensionMapper
    {
        public int ProductTypeId { get; set; }
        public int? ProductCategory { get; set; }
        public string? MpHomeApiLoanType { get; set; }
        public int? KonsDbLoanType { get; set; }
    }
    
    // dotaz na rozsirene vlastnosti codebooku mimo SB
    const string _sqlCodebooks = "SELECT * FROM ProductTypeExtension";

    // dotaz na codebook do SB
    const string _sql = @"
SELECT KOD_PRODUKTU 'Id', NAZOV_PRODUKTU 'Name', null 'Description', MANDANT 'Mandant', null 'ProductCategory', PORADIE_ZOBRAZENIA 'Order', MIN_VYSKA_UV 'LoanAmountMin', MAX_VYSKA_UV 'LoanAmountMax', MIN_SPLATNOST_V_ROKOCH 'LoanDurationMin', MAX_SPLATNOST_V_ROKOCH 'LoanDurationMax', MIN_VYSKA_LTV 'LtvMin', MAX_VYSKA_LTV 'LtvMax', 'xxx' 'MpHomeApiLoanType', CAST(CASE WHEN PLATNOST_DO IS NULL THEN 1 ELSE 0 END as bit) 'IsActual', DRUH_UV_POVOLENY 'MpHomeApiLoanType' 
FROM SBR.CIS_PRODUKTY_UV
ORDER BY PORADIE_ZOBRAZENIA ASC";

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProviderXxd;
    private readonly CIS.Core.Data.IConnectionProvider _connectionProviderCodebooks;
    private readonly ILogger<ProductTypesHandler> _logger;
    private readonly CIS.Infrastructure.Caching.IGlobalCache<ISharedInMemoryCache> _cache;
    private readonly MediatR.IMediator _mediator;
    
    public ProductTypesHandler(
        MediatR.IMediator mediator,
        CIS.Infrastructure.Caching.IGlobalCache<ISharedInMemoryCache> cache,
        CIS.Core.Data.IConnectionProvider connectionProviderCodebooks,
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProviderXxd,
        ILogger<ProductTypesHandler> logger)
    {
        _mediator = mediator;
        _cache = cache;
        _logger = logger;
        _connectionProviderCodebooks = connectionProviderCodebooks;
        _connectionProviderXxd = connectionProviderXxd;
    }

    private const string _cacheKey = "ProductTypes";
}
