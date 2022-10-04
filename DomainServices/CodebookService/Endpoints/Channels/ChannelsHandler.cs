using DomainServices.CodebookService.Contracts.Endpoints.Channels;

namespace DomainServices.CodebookService.Endpoints.Channels
{
    public class ChannelsHandler
        : IRequestHandler<ChannelsRequest, List<ChannelItem>>
    {

        #region Construction

        private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
        private readonly ILogger<ChannelsHandler> _logger;

        public ChannelsHandler(
            CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider,
            ILogger<ChannelsHandler> logger)
        {
            _logger = logger;
            _connectionProvider = connectionProvider;
        }

        #endregion

        // dotaz na codebook do SB
        const string _sql = @"SELECT KOD 'Id', TEXT 'Name', MANDANT 'MandantId', CODE 'Code', CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END 'IsValid' 
                                FROM [SBR].[CIS_ALT_KANALY] ORDER BY KOD ASC";

        public async Task<List<ChannelItem>> Handle(ChannelsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                return await FastMemoryCache.GetOrCreate<ChannelItem>(nameof(ChannelsHandler), async () =>
                {
                    // load codebook items
                    return await _connectionProvider.ExecuteDapperRawSqlToList<ChannelItem>(_sql, cancellationToken);
                });
            }
            catch (Exception ex)
            {
                _logger.GeneralException(ex);
                throw;
            }
        }
    }
}