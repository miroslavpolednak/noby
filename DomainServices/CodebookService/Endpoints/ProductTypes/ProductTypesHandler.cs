using DomainServices.CodebookService.Contracts.Endpoints.ProductTypes;

namespace DomainServices.CodebookService.Endpoints.ProductTypes;

public class ProductTypesHandler
    : IRequestHandler<ProductTypesRequest, List<ProductTypeItem>>
{
    public async Task<List<ProductTypeItem>> Handle(ProductTypesRequest request, CancellationToken cancellationToken)
    {
        try
        {
            return await FastMemoryCache.GetOrCreate<ProductTypeItem>(nameof(ProductTypesHandler), async () =>
            {
                // vytahnout druhy uveru pro naparovani do kolekci
                var loanKinds = await _mediator.Send(new Contracts.Endpoints.LoanKinds.LoanKindsRequest(), cancellationToken);

                // vytahnout mapovani na product category
                var extMapper = await _connectionProviderCodebooks.ExecuteDapperRawSqlToList<ExtensionMapper>(_sqlQueryExtension, cancellationToken);

                // vytahnout vlastni ciselnik z XXD
                var result = await _connectionProviderXxd.ExecuteDapperRawSqlToList<ProductTypeItem>(_sqlQuery, cancellationToken);

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
                        t.MpHomeApiLoanType = item.MpHomeApiLoanType;
                        t.KonsDbLoanType = item.KonsDbLoanType;
                    }

                });

                return result;
            });
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
        public string? MpHomeApiLoanType { get; set; }
        public int? KonsDbLoanType { get; set; }
    }
    
    // dotaz na rozsirene vlastnosti codebooku mimo SB
    const string _sqlQueryExtension = "SELECT ProductTypeId, MpHomeApiLoanType, KonsDbLoanType FROM dbo.ProductTypeExtension";

    // dotaz na codebook do SB
    const string _sqlQuery = @"
SELECT KOD_PRODUKTU 'Id', NAZOV_PRODUKTU 'Name', MANDANT 'MandantId', MANDANT 'Mandant', PORADIE_ZOBRAZENIA 'Order', MIN_VYSKA_UV 'LoanAmountMin', MAX_VYSKA_UV 'LoanAmountMax', MIN_SPLATNOST_V_ROKOCH 'LoanDurationMin', MAX_SPLATNOST_V_ROKOCH 'LoanDurationMax', MIN_VYSKA_LTV 'LtvMin', MAX_VYSKA_LTV 'LtvMax', CAST(CASE WHEN PLATNOST_DO IS NULL THEN 1 ELSE 0 END as bit) 'IsActual', DRUH_UV_POVOLENY 'MpHomeApiLoanType', CAST(CASE WHEN GETDATE() BETWEEN PLATNOST_OD AND ISNULL(PLATNOST_DO,'2099-01-01') THEN 1 ELSE 0 END as bit) 'IsValid' 
FROM SBR.CIS_PRODUKTY_UV
ORDER BY PORADIE_ZOBRAZENIA ASC";

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProviderXxd;
    private readonly CIS.Core.Data.IConnectionProvider _connectionProviderCodebooks;
    private readonly ILogger<ProductTypesHandler> _logger;
    private readonly MediatR.IMediator _mediator;
    
    public ProductTypesHandler(
        MediatR.IMediator mediator,
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
