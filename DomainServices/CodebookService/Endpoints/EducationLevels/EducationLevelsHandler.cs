using DomainServices.CodebookService.Contracts.Endpoints.EducationLevels;

namespace DomainServices.CodebookService.Endpoints.EducationLevels;

public class EducationLevelsHandler
    : IRequestHandler<EducationLevelsRequest, List<EducationLevelItem>>
{
    public async Task<List<EducationLevelItem>> Handle(EducationLevelsRequest request, CancellationToken cancellationToken)
    {
        return await FastMemoryCache.GetOrCreate<EducationLevelItem>(nameof(EducationLevelsHandler), async () =>
        {
            var result = await _connectionProvider.ExecuteDapperRawSqlToList<EducationLevelItem>(_sqlQuery, cancellationToken);
            return result;
        });
    }

    private const string _sqlQuery =
        "SELECT KOD 'Id', TEXT 'Name', CODE_NAME 'ShortName', CODE 'RdmCode',  KOD_SCORING 'ScoringCode', " +
        "CASE WHEN PLATNY_PRE_ES = 1 AND SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END 'IsValid' " +
        "FROM [SBR].[CIS_VZDELANIE] WHERE MANDANT IN (0, 2) ORDER BY KOD ASC";

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    private readonly ILogger<EducationLevelsHandler> _logger;

    public EducationLevelsHandler(

        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider, 
        ILogger<EducationLevelsHandler> logger)
    {
        _logger = logger;
        _connectionProvider = connectionProvider;
    }
}
