using DomainServices.CodebookService.Contracts.Endpoints.ContactTypes;

namespace DomainServices.CodebookService.Endpoints.ContactTypes;

public class ContactTypesHandler
    : IRequestHandler<ContactTypesRequest, List<ContactTypeItem>>
{

    #region Construction

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    private readonly CIS.Core.Data.IConnectionProvider _connectionProviderCodebooks;
    private readonly ILogger<ContactTypesHandler> _logger;

    public ContactTypesHandler(
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider,
        CIS.Core.Data.IConnectionProvider connectionProviderCodebooks,
        ILogger<ContactTypesHandler> logger)
    {
        _logger = logger;
        _connectionProviderCodebooks = connectionProviderCodebooks;
        _connectionProvider = connectionProvider;
    }

    #endregion

    #region Extension

    private class ContactTypeExtension
    {
        public int ContactTypeId { get; set; }
        public string MpDigiApiCode { get; set; } = null!;
    }

    #endregion

    // dotaz na codebook do SB
    const string _sql = @"SELECT TYP_KONTAKTU 'Id', TEXT 'Name', NULLIF(MANDANT, 0) 'MandantId', CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END 'IsValid' 
                               FROM [SBR].[CIS_TYPY_KONTAKTOV] ORDER BY TYP_KONTAKTU ASC";

    // dotaz na extenstion
    const string _sqlExtension = "SELECT [ContactTypeId], [MpDigiApiCode] FROM [dbo].[ContactTypeExtension]";


    public async Task<List<ContactTypeItem>> Handle(ContactTypesRequest request, CancellationToken cancellationToken)
    {
        return await FastMemoryCache.GetOrCreate<ContactTypeItem>(nameof(ContactTypesHandler), async () =>
        {
            // load codebook items
            var items = await _connectionProvider.ExecuteDapperRawSqlToList<ContactTypeItem>(_sql, cancellationToken);

            // load extensions
            var extensions = await _connectionProviderCodebooks.ExecuteDapperRawSqlToList<ContactTypeExtension>(_sqlExtension, cancellationToken);
            var dictExtensions = extensions.ToDictionary(i => i.ContactTypeId);

            // assign MpHome mapping to items
            items.ForEach(item =>
            {
                var itemExt = dictExtensions.ContainsKey(item.Id) ? dictExtensions[item.Id] : null;
                item.MpDigiApiCode = itemExt?.MpDigiApiCode;
            });

            return items;
        });
    }
}

