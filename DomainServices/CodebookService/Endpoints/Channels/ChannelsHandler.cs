using DomainServices.CodebookService.Contracts.Endpoints.Channels;


namespace DomainServices.CodebookService.Endpoints.Channels;
public class ChannelsHandler
    : IRequestHandler<ChannelsRequest, List<ChannelItem>>
{

    #region Construction

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    private readonly CIS.Core.Data.IConnectionProvider _connectionProviderCodebooks;
    private readonly ILogger<ChannelsHandler> _logger;

    public ChannelsHandler(
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider,
        CIS.Core.Data.IConnectionProvider connectionProviderCodebooks,
        ILogger<ChannelsHandler> logger)
    {
        _logger = logger;
        _connectionProvider = connectionProvider;
        _connectionProviderCodebooks = connectionProviderCodebooks;
    }

    #endregion

    // dotaz na codebook do SB
    const string _sql = @"SELECT KOD 'Id', TEXT 'Name', NULLIF(MANDANT, 0) 'MandantId', CODE 'Code', CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END 'IsValid' 
                                FROM [SBR].[CIS_ALT_KANALY] ORDER BY KOD ASC";

    // dotaz na rozsirene vlastnosti codebooku mimo SB
    const string _sqlExtension = "SELECT ChannelId, RdmCbChannelCode FROM dbo.ChannelExtension";

    private class ExtensionMapper
    {
        public int ChannelId { get; set; }
        public string? RdmCbChannelCode { get; set; }
    }


    public async Task<List<ChannelItem>> Handle(ChannelsRequest request, CancellationToken cancellationToken)
    {
        return await FastMemoryCache.GetOrCreate<ChannelItem>(nameof(ChannelsHandler), async () =>
        {
            // load extensions
            var extMapper = await _connectionProviderCodebooks.ExecuteDapperRawSqlToList<ExtensionMapper>(_sqlExtension, cancellationToken);
            var extMapperById = extMapper.ToDictionary(i => i.ChannelId);

            // load codebook items
            var result = await _connectionProvider.ExecuteDapperRawSqlToList<ChannelItem>(_sql, cancellationToken);

            // map RdmCode from extension table
            result.ForEach(t =>
            {
                var ext = extMapperById.ContainsKey(t.Id) ? extMapperById[t.Id] : null;
                t.RdmCbChannelCode = ext?.RdmCbChannelCode;
            });

            return result;
        });
    }
}