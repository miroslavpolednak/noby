using DomainServices.CodebookService.Contracts.Endpoints.GetDeveloper;
using Microsoft.Data.SqlClient;

namespace DomainServices.CodebookService.Endpoints.GetDeveloper;

public class GetDeveloperHandler
    : IRequestHandler<GetDeveloperRequest, DeveloperItem>
{
    public async Task<DeveloperItem> Handle(GetDeveloperRequest request, CancellationToken cancellationToken)
    {
        return await _connectionProvider.ExecuteDapperRawSqlFirstOrDefault<DeveloperItem>(_sqlQuery, new SqlParameter("id", request.DeveloperId), cancellationToken)
            ?? throw new CIS.Core.Exceptions.CisNotFoundException(20000, $"Developer {request.DeveloperId} not found");
    }

    const string _sqlQuery = @"
        SELECT DEVELOPER_ID 'Id', 
            NAZEV 'Name', 
            ICO_RC 'Cin', 
            CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_OD] AND ISNULL([PLATNOST_DO], '9999-12-31') THEN 1 ELSE 0 END 'IsValid',
            PRIZNAK_OK 'StatusId', 
            CASE WHEN PRIZNAK_OK=-1 THEN 'Probíhá prověřování' WHEN PRIZNAK_OK=0 THEN 'Zamítnutý' ELSE 'Schválený' END 'StatusText',
            CAST(CASE WHEN BALICEK_BENEFITU=1 THEN 1 ELSE 0 END as bit) 'BenefitPackage',
            CASE WHEN SYSDATETIME() BETWEEN[PLATNOST_BENEFITU_OD] AND ISNULL([PLATNOST_BENEFITU_DO], '9999-12-31') THEN 1 ELSE 0 END 'IsBenefitValid',
            CAST(CASE WHEN BENEFITY_NAD_RAMEC_BALICKU IS NOT NULL THEN 1 ELSE 0 END as bit) 'BenefitsBeyondPackage'
        FROM [SBR].[CIS_DEVELOPER]
        WHERE DEVELOPER_ID=@id
    ";

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    
    public GetDeveloperHandler(CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }
}
