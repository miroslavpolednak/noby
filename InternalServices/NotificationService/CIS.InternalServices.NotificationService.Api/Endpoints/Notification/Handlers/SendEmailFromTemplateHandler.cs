using System.Text.Json;
using CIS.InternalServices.NotificationService.Api.Repositories;
using CIS.InternalServices.NotificationService.Api.Services;
using CIS.InternalServices.NotificationService.Contracts.Email;
using CIS.InternalServices.NotificationService.Contracts.Result.Dto;
using MediatR;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.Notification.Handlers;

public class SendEmailFromTemplateHandler : IRequestHandler<EmailFromTemplateSendRequest, EmailFromTemplateSendResponse>
{
    private readonly McsBusinessService _mcsBusinessService;
    private readonly McsLogmanService _mcsLogmanService;
    private readonly NotificationRepository _repository;
    private readonly ILogger<SendEmailFromTemplateHandler> _logger;

    public SendEmailFromTemplateHandler(
        McsBusinessService mcsBusinessService,
        McsLogmanService mcsLogmanService,
        NotificationRepository repository,
        ILogger<SendEmailFromTemplateHandler> logger)
    {
        _mcsBusinessService = mcsBusinessService;
        _mcsLogmanService = mcsLogmanService;
        _repository = repository;
        _logger = logger;
    }
    
    public async Task<EmailFromTemplateSendResponse> Handle(EmailFromTemplateSendRequest request, CancellationToken cancellationToken)
    {
        var notificationResult = await _repository.CreateResult(NotificationChannel.Email, cancellationToken);
        var notificationId = notificationResult.Id;
     
        // todo: send attachments to s3
        
        // todo: mapping attachment, use s3 content
        var sendEmail = new SendApi.v2.email.SendEmail
        {
            id = notificationId.ToString(),
            // todo:
        };
        
        _logger.LogInformation("Sending email from template: {sendEmail}", JsonSerializer.Serialize(sendEmail));

        // todo: decide Logman or Business
        var sendResult = await _mcsLogmanService.SendEmail(sendEmail, cancellationToken);
        
        var updateResult = await _repository.UpdateResult(
            notificationId,
            NotificationState.Sent,
            new HashSet<string>(),
            cancellationToken);
        
        return new EmailFromTemplateSendResponse
        {
            NotificationId = Guid.NewGuid()
        };
    }
}