using DomainServices.CodebookService.Contracts.Endpoints.GetDeveloper;

namespace DomainServices.CodebookService.Endpoints.GetDeveloper;

public class GetDeveloperHandler
    : IRequestHandler<GetDeveloperRequest, DeveloperItem>
{
    public async Task<DeveloperItem> Handle(GetDeveloperRequest request, CancellationToken cancellationToken)
    {
        return await _connectionProvider.ExecuteDapperRawSqlFirstOrDefault<DeveloperItem>(_sqlQuery, cancellationToken)
            ?? throw new CIS.Core.Exceptions.CisNotFoundException(20000, $"Developer {request.DeveloperId} not found");
    }

    const string _sqlQuery = @"
        SELECT DEVELOPER_ID 'Id', NAZEV 'Name', ICO_RC 'Cin', CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END 'IsValid',
        PRIZNAK_OK 'Status', BALICEK_BENEFITU 'BenefitPackage', CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_BENEFITU_OD] AND ISNULL([PLATNOST_BENEFITU_DO], '9999-12-31') THEN 1 ELSE 0 END 'IsBenefitValid',
        BENEFITY_NAD_RAMEC_BALICKU 'BenefitsBeyondPackage'
        FROM [SBR].[CIS_DEVELOPER]
        WHERE DEVELOPER_ID=@id
    ";

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    
    public GetDeveloperHandler(CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }
}
