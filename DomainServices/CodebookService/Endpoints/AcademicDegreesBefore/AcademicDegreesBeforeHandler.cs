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
            new GenericCodebookItem() { Id = 0, Name ="Neuvedeno", IsValid = true},
            new GenericCodebookItem() { Id = 3, Name ="DR.", IsValid = true},
            new GenericCodebookItem() { Id = 7, Name ="JUDR.", IsValid = true},
            new GenericCodebookItem() { Id = 10, Name ="RNDR.", IsValid = true},
            new GenericCodebookItem() { Id = 20, Name ="MGR.", IsValid = true},
            new GenericCodebookItem() { Id = 47, Name ="ING.", IsValid = true},
            new GenericCodebookItem() { Id = 66, Name ="PROF.", IsValid = true},
            new GenericCodebookItem() { Id = 79, Name ="THMGR", IsValid = true},
            new GenericCodebookItem() { Id = 84, Name ="MVDR.", IsValid = true},
            new GenericCodebookItem() { Id = 90, Name ="PHMR.", IsValid = true},
            new GenericCodebookItem() { Id = 91, Name ="BC.", IsValid = true},
            new GenericCodebookItem() { Id = 92, Name ="MGA.", IsValid = true},
            new GenericCodebookItem() { Id = 94, Name ="PAEDDR", IsValid = true},
            new GenericCodebookItem() { Id = 96, Name ="THDR.", IsValid = true},
            new GenericCodebookItem() { Id = 97, Name ="THLIC.", IsValid = true},
            new GenericCodebookItem() { Id = 100, Name ="DOC.", IsValid = true},
            new GenericCodebookItem() { Id = 148, Name ="MDDr.", IsValid = true},
            new GenericCodebookItem() { Id = 5001, Name ="AKAD.", IsValid = true},
            new GenericCodebookItem() { Id = 5002, Name ="BCA.", IsValid = true},
            new GenericCodebookItem() { Id = 5003, Name ="RSDR.", IsValid = true},
            new GenericCodebookItem() { Id = 5004, Name ="PHDR.", IsValid = true},
            new GenericCodebookItem() { Id = 5005, Name ="ARCH.", IsValid = true},
            new GenericCodebookItem() { Id = 5006, Name ="MUDR.", IsValid = true},
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
