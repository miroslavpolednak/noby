using System.ServiceModel;
using CIS.InternalServices.NotificationService.Contracts.Email;
using CIS.InternalServices.NotificationService.Contracts.Result;
using CIS.InternalServices.NotificationService.Contracts.Sms;

namespace CIS.InternalServices.NotificationService.Contracts;

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
}