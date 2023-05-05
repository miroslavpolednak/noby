using DomainServices.CodebookService.Contracts.Endpoints.GetOperator;

namespace DomainServices.CodebookService.Endpoints.GetOperator;

internal class GetOperatorHandler
    : IRequestHandler<GetOperatorRequest, GetOperatorItem>
{
    public async Task<GetOperatorItem> Handle(GetOperatorRequest request, CancellationToken cancellationToken)
    {
        return await _connectionProvider.ExecuteDapperRawSqlFirstOrDefault<GetOperatorItem>(_sqlQuery, new { request.PerformerLogin }, cancellationToken)
            ?? throw new CIS.Core.Exceptions.CisNotFoundException(20002, $"Operator {request.PerformerLogin} not found");
    }

    const string _sqlQuery = @"SELECT MENO 'PerformerName', [LOGIN] 'PerformerLogin' FROM [xxd0vss].[SBR].[OPERATOR] WHERE DATUM_ZMENY IS NULL AND [LOGIN]=@PerformerLogin";

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;

    public GetOperatorHandler(CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }
}
