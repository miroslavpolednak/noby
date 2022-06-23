using DomainServices.CodebookService.Contracts;
using DomainServices.CodebookService.Contracts.Endpoints.Channels;

namespace DomainServices.CodebookService.Endpoints.Channels
{
    public class ChannelsHandler
        : IRequestHandler<ChannelsRequest, List<ChannelItem>>
    {
        public Task<List<ChannelItem>> Handle(ChannelsRequest request, CancellationToken cancellationToken)
        {
            //try
            //{
            //    return await FastMemoryCache.GetOrCreate(nameof(ChannelsHandler), async () =>
            //        await _connectionProvider.ExecuteDapperRawSqlToList<ChannelItem>(_sqlQuery, cancellationToken)
            //    );
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex, ex.Message);
            //    throw;
            //}

            // TODO: Redirect to real data source!     
            return Task.FromResult(new List<ChannelItem>
            {
                new ChannelItem() { Id = 1, MandantId = 1, Name = "Interní síť MP", Code = null, IsValid = true },
                new ChannelItem() { Id = 2, MandantId = 1 , Name = "Hypocentrum MP", Code = null, IsValid = true },
                new ChannelItem() { Id = 3, MandantId = 1 , Name = "Agentury MP", Code = null, IsValid = true },
                new ChannelItem() { Id = 4, MandantId = 2 , Name = "Interní síť KB", Code = "BR", IsValid = true },
                new ChannelItem() { Id = 6, MandantId = 2 , Name = "Agentury KB", Code = "MC", IsValid = true },
                new ChannelItem() { Id = 10, MandantId = 1 , Name = "POS MP", Code = null, IsValid = true },
            });
        }

        private const string _sqlQuery =
            "SELECT KOD 'Id', TEXT 'Name', CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END 'IsValid' FROM [SBR].[CIS_ALT_KANALY] ORDER BY KOD ASC";

        private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
        private readonly ILogger<ChannelsHandler> _logger;

        public ChannelsHandler(
            CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider, 
            ILogger<ChannelsHandler> logger)
        {
            _logger = logger;
            _connectionProvider = connectionProvider;
        }
    }
}