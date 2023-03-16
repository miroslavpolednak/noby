using DomainServices.CodebookService.Contracts.Endpoints.ProductTypes;

namespace DomainServices.CodebookService.Endpoints.ProductTypes;

public class ProductTypesHandler
    : IRequestHandler<ProductTypesRequest, List<ProductTypeItem>>
{
    public async Task<List<ProductTypeItem>> Handle(ProductTypesRequest request, CancellationToken cancellationToken)
    {
        return await FastMemoryCache.GetOrCreate<ProductTypeItem>(nameof(ProductTypesHandler), async () =>
        {
            // vytahnout mapovani na product category
            var extMapper = await _connectionProviderCodebooks.ExecuteDapperRawSqlToList<ExtensionMapper>(_sqlQueryExtension, cancellationToken);
            var extMapperById = extMapper.ToDictionary(i => i.ProductTypeId);

            // vytahnout vlastni ciselnik z XXD
            var result = await _connectionProviderXxd.ExecuteDapperRawSqlToList<ProductTypeItem>(_sqlQuery, cancellationToken);

            // pouze platne loan kinds https://jira.kb.cz/browse/HFICH-2984
            var loanKinds = (await _mediator.Send(new Contracts.Endpoints.LoanKinds.LoanKindsRequest(), cancellationToken)).Where(t => t.IsValid).Select(t => t.Id).ToArray();

            // namapovat kategorie z extensions tabulky
            result.ForEach(t =>
            {
                var ext = extMapperById.ContainsKey(t.Id) ? extMapperById[t.Id] : null;
                t.LoanKindIds = t.MpHomeApiLoanType.ParseIDs()?.Where(x => loanKinds.Contains(x)).ToList();
                t.MpHomeApiLoanType = ext?.MpHomeApiLoanType;
                t.KonsDbLoanType = ext?.KonsDbLoanType;
            });

            

            return result;
        });
    }

    private class ExtensionMapper
    {
        public int ProductTypeId { get; set; }
        public string? MpHomeApiLoanType { get; set; }
        public int? KonsDbLoanType { get; set; }
    }
    
    // dotaz na rozsirene vlastnosti codebooku mimo SB
    const string _sqlQueryExtension = "SELECT ProductTypeId, MpHomeApiLoanType, KonsDbLoanType FROM dbo.ProductTypeExtension";

    // dotaz na codebook do SB
    const string _sqlQuery = @"
SELECT KOD_PRODUKTU 'Id', NAZOV_PRODUKTU 'Name', NULLIF(MANDANT, 0) 'MandantId', PORADIE_ZOBRAZENIA 'Order', MIN_VYSKA_UV 'LoanAmountMin', MAX_VYSKA_UV 'LoanAmountMax', MIN_SPLATNOST_V_ROKOCH 'LoanDurationMin', MAX_SPLATNOST_V_ROKOCH 'LoanDurationMax', MIN_VYSKA_LTV 'LtvMin', MAX_VYSKA_LTV 'LtvMax', CAST(CASE WHEN PLATNOST_DO IS NULL THEN 1 ELSE 0 END as bit) 'IsActual', DRUH_UV_POVOLENY 'MpHomeApiLoanType', CAST(CASE WHEN GETDATE() BETWEEN PLATNOST_OD AND ISNULL(PLATNOST_DO,'2099-01-01') THEN 1 ELSE 0 END as bit) 'IsValid' 
FROM SBR.CIS_PRODUKTY_UV
ORDER BY PORADIE_ZOBRAZENIA ASC";

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProviderXxd;
    private readonly CIS.Core.Data.IConnectionProvider _connectionProviderCodebooks;
    private readonly ILogger<ProductTypesHandler> _logger;
    private readonly IMediator _mediator;
    
    public ProductTypesHandler(
        IMediator mediator,
        CIS.Core.Data.IConnectionProvider connectionProviderCodebooks,
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProviderXxd,
        ILogger<ProductTypesHandler> logger)
    {
        _mediator = mediator;
        _logger = logger;
        _connectionProviderCodebooks = connectionProviderCodebooks;
        _connectionProviderXxd = connectionProviderXxd;
    }
}
