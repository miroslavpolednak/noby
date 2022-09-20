using DomainServices.CodebookService.Contracts.Endpoints.IdentificationDocumentTypes;

namespace DomainServices.CodebookService.Endpoints.IdentificationDocumentTypes;

public class IdentificationDocumentTypesHandler
    : IRequestHandler<IdentificationDocumentTypesRequest, List<IdentificationDocumentTypesItem>>
{
    public Task<List<IdentificationDocumentTypesItem>> Handle(IdentificationDocumentTypesRequest request, CancellationToken cancellationToken)
    {
        //try
        //{
        //    return await FastMemoryCache.GetOrCreate<IdentificationDocumentTypesItem>(nameof(IdentificationDocumentTypesHandler), async () =>
        //    {
        //        var extMapper = await _connectionProviderCodebooks.ExecuteDapperRawSqlToList<ExtensionMapper>(_sqlQueryExtension, cancellationToken);

        //        var result = await _connectionProvider.ExecuteDapperRawSqlToList<IdentificationDocumentTypesItem>(_sqlQuery, cancellationToken);

        //        result.ForEach(t =>
        //        {
        //            t.RDMCode = extMapper.FirstOrDefault(s => s.IdentificationDocumentTypeId == t.Id)?.RDMCode;
        //        });
        //        return result;
        //    });
        //}
        //catch (Exception ex)
        //{
        //    _logger.GeneralException(ex);
        //    throw;
        //}

        // TODO: Redirect to real data source! Extension table (IdentificationDocumentTypeExtension) can be removed (Sloupec CODE pro mapování na KB/C4M hodnotu bude do číselníku ve SB bude přidán v dropu 1.2.)     
        return Task.FromResult(new List<IdentificationDocumentTypesItem>
            {
                new IdentificationDocumentTypesItem() { Id = 0, Name = "Nedefinovaný", ShortName = "ND", RdmCode = null, IsDefault = true, MpDigiApiCode ="Undefined" },
                new IdentificationDocumentTypesItem() { Id = 1, Name = "Občanský průkaz", ShortName = "OP", RdmCode = "A", MpDigiApiCode ="IDCard" },
                new IdentificationDocumentTypesItem() { Id = 2, Name = "Pas", ShortName = "PS", RdmCode = "B", MpDigiApiCode ="Passport" },
                new IdentificationDocumentTypesItem() { Id = 3, Name = "Průkaz k povolení k pobytu", ShortName = "PP", RdmCode = "F", MpDigiApiCode ="ResidencePermit" },
            });
    }

    private class ExtensionMapper
    {
        public int IdentificationDocumentTypeId { get; set; }
        public string? RDMCode { get; set; }
    }

    const string _sqlQuery = "SELECT KOD 'Id', TEXT 'Name', TEXT_SKRATKA 'ShortName', CAST(DEF as bit) 'IsDefault' FROM [SBR].[CIS_TYPY_DOKLADOV] ORDER BY TEXT ASC";
    const string _sqlQueryExtension = "Select * From IdentificationDocumentTypeExtension";

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
