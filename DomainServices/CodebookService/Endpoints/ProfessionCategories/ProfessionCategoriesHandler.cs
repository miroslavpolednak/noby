using DomainServices.CodebookService.Contracts.Endpoints.ProfessionCategories;

namespace DomainServices.CodebookService.Endpoints.ProfessionCategories;

public class ProfessionCategoriesHandler
    : IRequestHandler<ProfessionCategoriesRequest, List<ProfessionCategoryItem>>
{
    public async Task<List<ProfessionCategoryItem>> Handle(ProfessionCategoriesRequest request, CancellationToken cancellationToken)
    {
        return await FastMemoryCache.GetOrCreate<ProfessionCategoryItem>(nameof(ProfessionCategoriesHandler), async () =>
        {
            // load extensions
            var extMapper = await _connectionProviderCodebooks.ExecuteDapperRawSqlToList<ExtensionMapper>(_sqlQueryExtension, cancellationToken);
            var extMapperById = extMapper.ToDictionary(i => i.ProfessionCategoryId);

            // vytahnout vlastni ciselnik z XXD
            // var result = await _connectionProviderXxd.ExecuteDapperRawSqlToList<ProfessionCategoryItem>(_sqlQuery, cancellationToken);
            var result = new List<ProfessionCategoryItem>() { 
                new ProfessionCategoryItem { Id = 0, Name = "odmítl sdělit", IsValid = true},
                new ProfessionCategoryItem { Id = 1, Name = "státní zaměstnanec", IsValid = true},
                new ProfessionCategoryItem { Id = 2, Name = "zaměstnanec subjektu se státní majetkovou účastí", IsValid = true},
                new ProfessionCategoryItem { Id = 3, Name = "zaměstnanec subjektu se zahraničním vlastníkem", IsValid = true},
                new ProfessionCategoryItem { Id = 4, Name = "podnikatel", IsValid = true},
                new ProfessionCategoryItem { Id = 5, Name = "zaměstnanec soukromé společnosti", IsValid = true},
                new ProfessionCategoryItem { Id = 6, Name = "bez zaměstnání", IsValid = true},
                new ProfessionCategoryItem { Id = 7, Name = "nezjištěno", IsValid = true},
                new ProfessionCategoryItem { Id = 8, Name = "kombinace profesí", IsValid = true},
            };

            // namapovat kategorie z extensions tabulky
            result.ForEach(t =>
            {
                var ext = extMapperById.ContainsKey(t.Id) ? extMapperById[t.Id] : null;
                t.ProfessionIds = ext?.ProfessionIds?.ParseIDs();
                t.IncomeMainTypeAMLIds = ext?.IncomeMainTypeAMLIds?.ParseIDs();
            });

            return result;
        });
    }

    private class ExtensionMapper
    {
        public int ProfessionCategoryId { get; set; }
        public string? ProfessionIds { get; set; }
        public string? IncomeMainTypeAMLIds { get; set; }
    }
    
    // dotaz na rozsirene vlastnosti codebooku mimo SB
    const string _sqlQueryExtension = "SELECT ProfessionCategoryId, ProfessionIds, IncomeMainTypeAMLIds FROM dbo.ProfessionCategoryExtension";

    // dotaz na codebook do SB
    // const string _sqlQuery = @"";

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProviderXxd;
    private readonly CIS.Core.Data.IConnectionProvider _connectionProviderCodebooks;
    private readonly ILogger<ProfessionCategoriesHandler> _logger;
    private readonly IMediator _mediator;
    
    public ProfessionCategoriesHandler(
        IMediator mediator,
        CIS.Core.Data.IConnectionProvider connectionProviderCodebooks,
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProviderXxd,
        ILogger<ProfessionCategoriesHandler> logger)
    {
        _mediator = mediator;
        _logger = logger;
        _connectionProviderCodebooks = connectionProviderCodebooks;
        _connectionProviderXxd = connectionProviderXxd;
    }
}
