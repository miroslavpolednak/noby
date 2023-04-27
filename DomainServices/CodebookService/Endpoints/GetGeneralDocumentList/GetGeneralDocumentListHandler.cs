using CIS.Core.Data;
using DomainServices.CodebookService.Contracts.Endpoints.GetGeneralDocumentList;

namespace DomainServices.CodebookService.Endpoints.GetGeneralDocumentList;

public class GetGeneralDocumentListHandler
    : IRequestHandler<GetGeneralDocumentListRequest, List<GetGeneralDocumentListItem>>
{
    #region Construction

    private readonly IConnectionProvider _connectionProvider;

    private const string _sqlQuery =
        @"SELECT Id, Name, Filename, Format FROM [dbo].[GeneralDocumentList] ORDER BY Id ASC";

    public GetGeneralDocumentListHandler(IConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    #endregion

    public async Task<List<GetGeneralDocumentListItem>> Handle(GetGeneralDocumentListRequest request, CancellationToken cancellationToken)
    {
        return await FastMemoryCache.GetOrCreate<GetGeneralDocumentListItem>(nameof(GetGeneralDocumentListHandler), async () =>
            await _connectionProvider.ExecuteDapperRawSqlToList<GetGeneralDocumentListItem>(_sqlQuery, cancellationToken)
        );
    }
}