using System.Text.Json;
using CIS.InternalServices.NotificationService.Api.Mappers;
using CIS.InternalServices.NotificationService.Api.Repositories;
using CIS.InternalServices.NotificationService.Api.Services;
using CIS.InternalServices.NotificationService.Contracts.Email;
using CIS.InternalServices.NotificationService.Contracts.Result.Dto;
using cz.kb.osbs.mcs.sender.sendapi.v2;
using MediatR;

namespace CIS.InternalServices.NotificationService.Api.Endpoints.Notification.Handlers;

public class SendEmailHandler : IRequestHandler<EmailSendRequest, EmailSendResponse>
{
    private readonly McsBusinessService _mcsBusinessService;
    private readonly McsLogmanService _mcsLogmanService;
    private readonly NotificationRepository _repository;
    private readonly ILogger<SendEmailHandler> _logger;

    public SendEmailHandler(
        McsBusinessService mcsBusinessService,
        McsLogmanService mcsLogmanService,
        NotificationRepository repository,
        ILogger<SendEmailHandler> logger)
    {
        _mcsBusinessService = mcsBusinessService;
        _mcsLogmanService = mcsLogmanService;
        _repository = repository;
        _logger = logger;
    }
    
    public async Task<EmailSendResponse> Handle(EmailSendRequest request, CancellationToken cancellationToken)
    {
        var notificationResult = await _repository.CreateResult(NotificationChannel.Email, cancellationToken);
        var notificationId = notificationResult.Id;
     
        // todo: send attachments to s3
        
        // todo: mapping attachment, use s3 content
        var sendEmail = new SendApi.v2.email.SendEmail
        {
            id = notificationId.ToString(),
            sender = request.From.Map(),
            to = request.To.Map().ToList(),
            bcc = request.Bcc.Map().ToList(),
            cc = request.Cc.Map().ToList(),
            replyTo = request.ReplyTo?.Map(),
            subject = request.Subject,
            content = request.Content.Map(),
            attachments = new List<Attachment>(),
        };
        
        _logger.LogInformation("Sending email: {sendEmail}", JsonSerializer.Serialize(sendEmail));

        // todo: decide Logman or Business
        var sendResult = await _mcsLogmanService.SendEmail(sendEmail, cancellationToken);
        
        var updateResult = await _repository.UpdateResult(
            notificationId,
            NotificationState.Sent,
            new HashSet<string>(),
            cancellationToken);
        
        return new EmailSendResponse
        {
            NotificationId = notificationId
        };
    }
}