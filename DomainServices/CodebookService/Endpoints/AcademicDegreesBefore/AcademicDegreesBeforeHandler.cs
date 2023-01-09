using Dapper;
using DomainServices.CodebookService.Contracts;
using DomainServices.CodebookService.Contracts.Endpoints.AcademicDegreesBefore;

namespace DomainServices.CodebookService.Endpoints.AcademicDegreesBefore
{
    public class AcademicDegreesBeforeHandler
        : IRequestHandler<AcademicDegreesBeforeRequest, List<GenericCodebookItem>>
    {
        public async Task<List<GenericCodebookItem>> Handle(AcademicDegreesBeforeRequest request, CancellationToken cancellationToken)
        {
            return await FastMemoryCache.GetOrCreate<GenericCodebookItem>(nameof(AcademicDegreesBeforeHandler), async () =>
            {
                await using (var connection = _connectionProvider.Create())
                {
                    await connection.OpenAsync();
                    return (await connection.QueryAsync<GenericCodebookItem>("SELECT KOD 'Id', TEXT 'Name', CAST(1 as bit) 'IsValid' FROM [SBR].[CIS_TITULY] ORDER BY TEXT ASC")).ToList();
                }
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
}
