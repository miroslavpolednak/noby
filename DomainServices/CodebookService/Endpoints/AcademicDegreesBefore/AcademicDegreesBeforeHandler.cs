using Dapper;
using DomainServices.CodebookService.Contracts;
using DomainServices.CodebookService.Contracts.Endpoints.AcademicDegreesBefore;


namespace DomainServices.CodebookService.Endpoints.AcademicDegreesBefore;

public class AcademicDegreesBeforeHandler
    : IRequestHandler<AcademicDegreesBeforeRequest, List<GenericCodebookItem>>
{
    //public async Task<List<GenericCodebookItem>> Handle(AcademicDegreesBeforeRequest request, CancellationToken cancellationToken)
    //{
    //    return await FastMemoryCache.GetOrCreate<GenericCodebookItem>(nameof(AcademicDegreesBeforeHandler), async () =>
    //    {
    //        await using (var connection = _connectionProvider.Create())
    //        {
    //            await connection.OpenAsync();
    //            return (await connection.QueryAsync<GenericCodebookItem>("SELECT KOD 'Id', TEXT 'Name', CAST(1 as bit) 'IsValid' FROM [SBR].[CIS_TITULY] ORDER BY TEXT ASC")).ToList();
    //        }
    //    });
    //}

    // [HFICH-4609] číselník titulů je nově zamockován, starbuild číselník je pro nás nepoužitelný

    public Task<List<GenericCodebookItem>> Handle(AcademicDegreesBeforeRequest request, CancellationToken cancellationToken)
    {
        return Task.FromResult(new List<GenericCodebookItem>
        {
            new GenericCodebookItem() { Id = 00, Name ="Neuvedeno", IsValid = true},
            new GenericCodebookItem() { Id = 30, Name ="DR.", IsValid = true},
            new GenericCodebookItem() { Id = 70, Name ="JUDR.", IsValid = true},
            new GenericCodebookItem() { Id = 100, Name ="RNDR.", IsValid = true},
            new GenericCodebookItem() { Id = 200, Name ="MGR.", IsValid = true},
            new GenericCodebookItem() { Id = 470, Name ="ING.", IsValid = true},
            new GenericCodebookItem() { Id = 660, Name ="PROF.", IsValid = true},
            new GenericCodebookItem() { Id = 790, Name ="THMGR", IsValid = true},
            new GenericCodebookItem() { Id = 840, Name ="MVDR.", IsValid = true},
            new GenericCodebookItem() { Id = 900, Name ="PHMR.", IsValid = true},
            new GenericCodebookItem() { Id = 910, Name ="BC.", IsValid = true},
            new GenericCodebookItem() { Id = 920, Name ="MGA.", IsValid = true},
            new GenericCodebookItem() { Id = 940, Name ="PAEDDR", IsValid = true},
            new GenericCodebookItem() { Id = 960, Name ="THDR.", IsValid = true},
            new GenericCodebookItem() { Id = 970, Name ="THLIC.", IsValid = true},
            new GenericCodebookItem() { Id = 1000, Name ="DOC.", IsValid = true},
            new GenericCodebookItem() { Id = 1480, Name ="MDDr.", IsValid = true},
            new GenericCodebookItem() { Id = 50010, Name ="AKAD.", IsValid = true},
            new GenericCodebookItem() { Id = 50020, Name ="BCA.", IsValid = true},
            new GenericCodebookItem() { Id = 50030, Name ="RSDR.", IsValid = true},
            new GenericCodebookItem() { Id = 50040, Name ="PHDR.", IsValid = true},
            new GenericCodebookItem() { Id = 50050, Name ="ARCH.", IsValid = true},
            new GenericCodebookItem() { Id = 50060, Name ="MUDR.", IsValid = true},
        });
    }

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    private readonly ILogger<AcademicDegreesBeforeHandler> _logger;

    public AcademicDegreesBeforeHandler(
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider, 
        ILogger<AcademicDegreesBeforeHandler> logger)
    {
        _logger = logger;
        _connectionProvider = connectionProvider;
    }
}
