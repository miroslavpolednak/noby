using CIS.InternalServices.NotificationService.Contracts.v2;
using Google.Protobuf.WellKnownTypes;

namespace CIS.InternalServices.NotificationService.Clients.Services;

internal sealed class NotificationServiceClientV2(Contracts.v2.NotificationService.NotificationServiceClient _service)
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

    public async Task<GetStatisticsResponse> GetStatistics(GetStatisticsRequest request, CancellationToken cancellationToken = default)
        => await _service.GetStatisticsAsync(request, cancellationToken: cancellationToken);

    public async Task<GetDetailedStatisticsResponse> GetDetailedStatistics(GetDetailedStatisticsRequest request, CancellationToken cancellationToken = default)
        => await _service.GetDetailedStatisticsAsync(request, cancellationToken: cancellationToken);

    public async Task<Empty> Resend(ResendRequest request, CancellationToken cancellationToken = default)
        => await _service.ResendAsync(request, cancellationToken: cancellationToken);
}