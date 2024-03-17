using CIS.InternalServices.NotificationService.Contracts.v2;

namespace CIS.InternalServices.NotificationService.Clients.Services;

internal sealed class NotificationServiceClientV2 
    : v2.INotificationServiceClient
{
    public async Task<ResultData> GetResult(Guid notificationId, CancellationToken cancellationToken = default)
        => await _service.GetResultAsync(new GetResultRequest
        {
            NotificationId = notificationId.ToString()
        }, cancellationToken: cancellationToken);

    public async Task<Guid> SendEmail(SendEmailRequest request, CancellationToken cancellationToken = default)
        => Guid.Parse((await _service.SendEmailAsync(request, cancellationToken: cancellationToken)).NotificationId);

    public async Task<Guid> SendSms(SendSmsRequest request, CancellationToken cancellationToken = default)
        => Guid.Parse((await _service.SendSmsAsync(request, cancellationToken: cancellationToken)).NotificationId);

    public async Task<List<ResultData>> SearchResults(SearchResultsRequest request, CancellationToken cancellationToken = default)
        => (await _service.SearchResultsAsync(request, cancellationToken: cancellationToken)).Results.ToList();

    private readonly Contracts.v2.NotificationService.NotificationServiceClient _service;
    public NotificationServiceClientV2(Contracts.v2.NotificationService.NotificationServiceClient service)
        => _service = service;
}