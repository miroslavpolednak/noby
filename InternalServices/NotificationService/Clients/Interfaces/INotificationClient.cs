using CIS.InternalServices.NotificationService.Contracts.Email;
using CIS.InternalServices.NotificationService.Contracts.Result;
using CIS.InternalServices.NotificationService.Contracts.Sms;
using CIS.InternalServices.NotificationService.Contracts.Statistics;
using CIS.InternalServices.NotificationService.Contracts.Resend;

namespace CIS.InternalServices.NotificationService.Clients.Interfaces;

public interface INotificationClient
{
    Task<SendSmsResponse> SendSms(SendSmsRequest request, CancellationToken token);
    Task<SendSmsFromTemplateResponse> SendSmsFromTemplate(SendSmsFromTemplateRequest request, CancellationToken token);
    Task<SendEmailResponse> SendEmail(SendEmailRequest request, CancellationToken token);
    Task<SendEmailFromTemplateResponse> SendEmailFromTemplate(SendEmailFromTemplateRequest request, CancellationToken token);
    Task<SearchResultsResponse> SearchResults(SearchResultsRequest request, CancellationToken token);
    Task<GetResultResponse> GetResult(GetResultRequest request, CancellationToken token);
    Task<GetStatisticsResponse> GetStatistics(GetStatisticsRequest request, CancellationToken token);
    Task<GetDetailedStatisticsResponse> GetDetailedStatistics(GetDetailedStatisticsRequest request, CancellationToken token);
    Task Resend(ResendRequest request, CancellationToken token);
}