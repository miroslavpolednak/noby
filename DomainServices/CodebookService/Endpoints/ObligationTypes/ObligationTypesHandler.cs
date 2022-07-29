using DomainServices.CodebookService.Contracts.Endpoints.ObligationTypes;

namespace DomainServices.CodebookService.Endpoints.ObligationTypes;

public class ObligationTypesHandler
    : IRequestHandler<ObligationTypesRequest, List<ObligationTypesItem>>
{
    public async Task<List<ObligationTypesItem>> Handle(ObligationTypesRequest request, CancellationToken cancellationToken)
    {
        try
        {
            return await FastMemoryCache.GetOrCreate<ObligationTypesItem>(nameof(ObligationTypesHandler), async () =>
            {
                var items = await _connectionProvider.ExecuteDapperRawSqlToList<ObligationTypesItemExt>(_sqlQuery, cancellationToken);

                return items.Select(i => new ObligationTypesItem
                {
                    Id = i.Id,
                    Name = i.Name,
                    Code = i.Code,
                    ObligationCorrectionTypeIds = i.ObligationCorrectionTypeId?.ParseIDs(),
                    IsValid = i.IsValid,

                }).ToList();
            });
        }
        catch (Exception ex)
        {
            _logger.GeneralException(ex);
            throw;
        }
    }

    private class ObligationTypesItemExt : ObligationTypesItem
    {
        public string? ObligationCorrectionTypeId { get; set; }
    }

    const string _sqlQuery = @"SELECT KOD 'Id', CODE 'Code', TEXT 'Name', KOREKCE_ZAVAZKU 'ObligationCorrectionTypeId', CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END 'IsValid' 
                               FROM [SBR].CIS_DRUH_ZAVAZKU ORDER BY KOD ASC";

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    private readonly ILogger<ObligationTypesHandler> _logger;

    public ObligationTypesHandler(
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider,
        ILogger<ObligationTypesHandler> logger)
    {
        _logger = logger;
        _connectionProvider = connectionProvider;
    }
}
