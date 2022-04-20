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
            try
            {
                return await FastMemoryCache.GetOrCreate<GenericCodebookItem>(nameof(AcademicDegreesBeforeHandler), async () =>
                {
                    await using (var connection = _connectionProvider.Create())
                    {
                        await connection.OpenAsync();
                        return (await connection.QueryAsync<GenericCodebookItem>("SELECT KOD 'Id', TEXT 'Name' FROM [SBR].[CIS_TITULY] WHERE KOD>0 ORDER BY TEXT ASC")).ToList();
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
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
