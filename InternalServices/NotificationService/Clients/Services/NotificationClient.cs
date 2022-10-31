using CIS.Core.Results;
using CIS.Infrastructure.Logging;
using CIS.InternalServices.NotificationService.Clients.Interfaces;
using CIS.InternalServices.NotificationService.Contracts;
using CIS.InternalServices.NotificationService.Contracts.Email;
using CIS.InternalServices.NotificationService.Contracts.Result;
using CIS.InternalServices.NotificationService.Contracts.Sms;
using Microsoft.Extensions.Logging;

namespace CIS.InternalServices.NotificationService.Clients.Services;

public class NotificationClient : INotificationClient
{
    private readonly INotificationService _notificationService;
    private readonly ILogger<NotificationClient> _logger;

    public NotificationClient(INotificationService notificationService, ILogger<NotificationClient> logger)
    {
        _notificationService = notificationService;
        _logger = logger;
    }

    public async Task<IServiceCallResult> SendSms(SmsSendRequest request, CancellationToken token)
    {
        _logger.RequestHandlerStarted(nameof(SendSms));
        var response = await _notificationService.SendSms(request, token);
        return new SuccessfulServiceCallResult<SmsSendResponse>(response);
    }

    public async Task<IServiceCallResult> SendSmsFromTemplate(SmsFromTemplateSendRequest request, CancellationToken token)
    {
        _logger.RequestHandlerStarted(nameof(SendSmsFromTemplate));
        var response = await _notificationService.SendSmsFromTemplate(request, token);
        return new SuccessfulServiceCallResult<SmsFromTemplateSendResponse>(response);
    }

    public async Task<IServiceCallResult> SendEmail(EmailSendRequest request, CancellationToken token)
    {
        _logger.RequestHandlerStarted(nameof(SendEmail));
        var response = await _notificationService.SendEmail(request, token);
        return new SuccessfulServiceCallResult<EmailSendResponse>(response);
    }

    public async Task<IServiceCallResult> SendEmailFromTemplate(EmailFromTemplateSendRequest request, CancellationToken token)
    {
        _logger.RequestHandlerStarted(nameof(SendEmailFromTemplate));
        var response = await _notificationService.SendEmailFromTemplate(request, token);
        return new SuccessfulServiceCallResult<EmailFromTemplateSendResponse>(response);
    }

    public async Task<IServiceCallResult> GetResult(ResultGetRequest request, CancellationToken token)
    {
        _logger.RequestHandlerStarted(nameof(GetResult));
        var response = await _notificationService.GetResult(request, token);
        return new SuccessfulServiceCallResult<ResultGetResponse>(response);
    }
}