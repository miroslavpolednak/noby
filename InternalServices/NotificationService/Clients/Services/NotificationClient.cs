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

    public async Task<SendSmsResponse> SendSms(SendSmsRequest request, CancellationToken token)
    {
        _logger.RequestHandlerStarted(nameof(SendSms));
        return await _notificationService.SendSms(request, token);
    }

    public async Task<SendSmsFromTemplateResponse> SendSmsFromTemplate(SendSmsFromTemplateRequest request, CancellationToken token)
    {
        _logger.RequestHandlerStarted(nameof(SendSmsFromTemplate));
        return await _notificationService.SendSmsFromTemplate(request, token);
    }

    public async Task<SendEmailResponse> SendEmail(SendEmailRequest request, CancellationToken token)
    {
        _logger.RequestHandlerStarted(nameof(SendEmail));
        return await _notificationService.SendEmail(request, token);
    }

    public async Task<SendEmailFromTemplateResponse> SendEmailFromTemplate(SendEmailFromTemplateRequest request, CancellationToken token)
    {
        _logger.RequestHandlerStarted(nameof(SendEmailFromTemplate));
        return await _notificationService.SendEmailFromTemplate(request, token);
    }

    public async Task<GetResultResponse> GetResult(GetResultRequest request, CancellationToken token)
    {
        _logger.RequestHandlerStarted(nameof(GetResult));
        return await _notificationService.GetResult(request, token);
    }
}