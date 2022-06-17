using DomainServices.CodebookService.Contracts.Endpoints.EducationLevels;

namespace DomainServices.CodebookService.Endpoints.EducationLevels;

public class EducationLevelsHandler
    : IRequestHandler<EducationLevelsRequest, List<EducationLevelItem>>
{
    public async Task<List<EducationLevelItem>> Handle(EducationLevelsRequest request, CancellationToken cancellationToken)
    {
        try
        {
            return await FastMemoryCache.GetOrCreate<EducationLevelItem>(nameof(EducationLevelsHandler), async () =>
            {
                var extMapper = await _connectionProviderCodebooks.ExecuteDapperRawSqlToList<ExtensionMapper>(_sqlQueryExtension, cancellationToken);

                var result = await _connectionProvider.ExecuteDapperRawSqlToList<EducationLevelItem>(_sqlQuery, cancellationToken);

                result.ForEach(t =>
                {
                    t.RDMCode = extMapper.FirstOrDefault(s => s.EducationLevelId == t.Id)?.RDMCode;
                });

                return result;
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            throw;
        }
    }

    private class ExtensionMapper
    {
        public int EducationLevelId { get; set; }
        public string? RDMCode { get; set; }
    }

    private const string _sqlQuery =
        "SELECT ID_VZDELANI 'Id', NAZEV_VZDELANI 'Name' FROM [SBR].[CIS_VZDELANI] ORDER BY ID_VZDELANI ASC";
    const string _sqlQueryExtension = "Select * From EducationLevelExtension";

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    private readonly ILogger<EducationLevelsHandler> _logger;
    private readonly CIS.Core.Data.IConnectionProvider _connectionProviderCodebooks;

    public EducationLevelsHandler(

        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider, 
        ILogger<EducationLevelsHandler> logger,
        CIS.Core.Data.IConnectionProvider connectionProviderCodebooks)
    {
        _logger = logger;
        _connectionProvider = connectionProvider;
        _connectionProviderCodebooks = connectionProviderCodebooks;
    }
}
