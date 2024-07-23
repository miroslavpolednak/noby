using System.ServiceModel;
using CIS.InternalServices.NotificationService.LegacyContracts.Email;
using CIS.InternalServices.NotificationService.LegacyContracts.Result;
using CIS.InternalServices.NotificationService.LegacyContracts.Sms;
using CIS.InternalServices.NotificationService.LegacyContracts.Statistics;
using CIS.InternalServices.NotificationService.LegacyContracts.Resend;

namespace CIS.InternalServices.NotificationService.LegacyContracts;

[ServiceContract(Name = "CIS.InternalServices.NotificationService")]
public interface INotificationService
{
    [OperationContract]
    Task<SendSmsResponse> SendSms(SendSmsRequest request, CancellationToken token);
    
    [OperationContract]
    Task<SendSmsFromTemplateResponse> SendSmsFromTemplate(SendSmsFromTemplateRequest request, CancellationToken token);
    
    [OperationContract]
    Task<SendEmailResponse> SendEmail(SendEmailRequest request, CancellationToken token);

    [OperationContract]
    Task<SendEmailFromTemplateResponse> SendEmailFromTemplate(SendEmailFromTemplateRequest request, CancellationToken token);

    [OperationContract]
    Task<GetResultResponse> GetResult(GetResultRequest request, CancellationToken token);

    [OperationContract]
    Task<SearchResultsResponse> SearchResults(SearchResultsRequest request, CancellationToken token);

    [OperationContract]
    Task<GetStatisticsResponse> GetStatistics(GetStatisticsRequest request, CancellationToken token);

    [OperationContract]
    Task<GetDetailedStatisticsResponse> GetDetailedStatistics(GetDetailedStatisticsRequest request, CancellationToken token);

    [OperationContract]
    Task Resend(ResendRequest request, CancellationToken token);
}