using DomainServices.CodebookService.Contracts.Endpoints.IdentificationDocumentTypes;

namespace DomainServices.CodebookService.Endpoints.IdentificationDocumentTypes;

public class IdentificationDocumentTypesHandler
    : IRequestHandler<IdentificationDocumentTypesRequest, List<IdentificationDocumentTypesItem>>
{
    public async Task<List<IdentificationDocumentTypesItem>> Handle(IdentificationDocumentTypesRequest request, CancellationToken cancellationToken)
    {
        return await FastMemoryCache.GetOrCreate<IdentificationDocumentTypesItem>(nameof(IdentificationDocumentTypesHandler), async () =>
        {

            // load codebook items
            var items = await _connectionProvider.ExecuteDapperRawSqlToList<IdentificationDocumentTypesItem>(_sqlQuery, cancellationToken);

            // load extensions
            var extensions = await _connectionProviderCodebooks.ExecuteDapperRawSqlToList<ExtensionMapper>(_sqlQueryExtension, cancellationToken);
            var dictExtensions = extensions.ToDictionary(i => i.IdentificationDocumentTypeId);

            // assign MpHome mapping to items
            items.ForEach(item =>
            {
                var itemExt = dictExtensions.ContainsKey(item.Id) ? dictExtensions[item.Id] : null;
                item.MpDigiApiCode = itemExt?.MpDigiApiCode;
            });

            return items;
        });
    }

    private class ExtensionMapper
    {
        public int IdentificationDocumentTypeId { get; set; }
        public string? MpDigiApiCode { get; set; }
    }

    const string _sqlQuery = "SELECT KOD 'Id', TEXT 'Name', TEXT_SKRATKA 'ShortName', CODE 'RdmCode', CAST(DEF as bit) 'IsDefault' FROM [SBR].[CIS_TYPY_DOKLADOV] ORDER BY KOD ASC";
    const string _sqlQueryExtension = "SELECT [IdentificationDocumentTypeId],[MpDigiApiCode] FROM [dbo].[IdentificationDocumentTypeExtension]";
    
    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    private readonly ILogger<IdentificationDocumentTypesHandler> _logger;
    private readonly CIS.Core.Data.IConnectionProvider _connectionProviderCodebooks;

    public IdentificationDocumentTypesHandler(
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider,
        ILogger<IdentificationDocumentTypesHandler> logger,
        CIS.Core.Data.IConnectionProvider connectionProviderCodebooks)
    {
        _logger = logger;
        _connectionProvider = connectionProvider;
        _connectionProviderCodebooks = connectionProviderCodebooks;
    }

    private const string _cacheKey = "IdentificationDocumentTypes";
}
