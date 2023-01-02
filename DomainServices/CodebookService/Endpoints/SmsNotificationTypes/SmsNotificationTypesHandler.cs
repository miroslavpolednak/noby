using DomainServices.CodebookService.Contracts.Endpoints.SmsNotificationTypes;

namespace DomainServices.CodebookService.Endpoints.SmsNotificationTypes;

public class SmsNotificationTypesHandler
    : IRequestHandler<SmsNotificationTypesRequest, List<SmsNotificationTypeItem>>
{
    public async Task<List<SmsNotificationTypeItem>> Handle(SmsNotificationTypesRequest request, CancellationToken cancellationToken)
    {
        return await FastMemoryCache.GetOrCreate<SmsNotificationTypeItem>(nameof(SmsNotificationTypesHandler), async () =>
            await _connectionProvider.ExecuteDapperRawSqlToList<SmsNotificationTypeItem>(_sqlQuery, cancellationToken)
        );
    }

    private const string _sqlQuery = "SELECT * FROM [dbo].[SmsNotificationType]";

    private readonly CIS.Core.Data.IConnectionProvider _connectionProvider;

    public SmsNotificationTypesHandler(CIS.Core.Data.IConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }
}
