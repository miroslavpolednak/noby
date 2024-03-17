using CIS.InternalServices.NotificationService.Contracts.v2;

namespace CIS.InternalServices.NotificationService.Clients.v2;

public interface INotificationServiceClient
{
    Task<Guid> SendSms(SendSmsRequest request, CancellationToken cancellationToken = default);
    
    Task<Guid> SendEmail(SendEmailRequest request, CancellationToken cancellationToken = default);
    
    Task<ResultData> GetResult(Guid notificationId, CancellationToken cancellationToken = default);

    Task<List<ResultData>> SearchResults(SearchResultsRequest request, CancellationToken cancellationToken = default);
}