using Dapper;
using DomainServices.CodebookService.Contracts.Endpoints.PostCodes;

namespace DomainServices.CodebookService.Endpoints.PostCodes
{
    public class PostCodesHandler
        : IRequestHandler<PostCodesRequest, List<PostCodeItem>>
    {
        public async Task<List<PostCodeItem>> Handle(PostCodesRequest request, CancellationToken cancellationToken)
        {
            // nebudeme kesovat, stejne je to potemkin
            _logger.LogDebug("Reading PostCodes from database");

            await using (var connection = _connectionProvider.Create())
            {
                await connection.OpenAsync();
                var result = (await connection.QueryAsync<PostCodeItem>("SELECT TOP 20 PSC 'PostCode', NAZEV 'Name', KOD_KRAJA 'Disctrict', KOD_OBCE 'Municipality' FROM [SBR].[CIS_PSC] ORDER BY PSC ASC")).ToList();

                result.ForEach(i => {
                    i.Name = i.Name.Trim();
                });

                return result;
            }
        }

        private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
        private readonly ILogger<PostCodesHandler> _logger;

        public PostCodesHandler(
            CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider,
            ILogger<PostCodesHandler> logger)
        {
            _logger = logger;
            _connectionProvider = connectionProvider;
        }
    }
}