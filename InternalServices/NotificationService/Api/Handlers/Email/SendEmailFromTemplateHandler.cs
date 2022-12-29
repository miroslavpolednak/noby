﻿using System.Text.Json;
using CIS.InternalServices.NotificationService.Api.Services.Mcs.Mappers;
using CIS.InternalServices.NotificationService.Api.Services.Mcs.Producers;
using CIS.InternalServices.NotificationService.Api.Services.Repositories;
using CIS.InternalServices.NotificationService.Api.Services.S3;
using CIS.InternalServices.NotificationService.Contracts.Email;
using CIS.InternalServices.NotificationService.Contracts.Result.Dto;
using cz.kb.osbs.mcs.sender.sendapi.v4;
using MediatR;

namespace CIS.InternalServices.NotificationService.Api.Handlers.Email;

public class SendEmailFromTemplateHandler : IRequestHandler<SendEmailFromTemplateRequest, SendEmailFromTemplateResponse>
{
    private readonly MpssEmailProducer _mpssEmailProducer;
    private readonly McsEmailProducer _mcsEmailProducer;
    private readonly NotificationRepository _repository;
    private readonly S3AdapterService _s3Service;
    private readonly ILogger<SendEmailFromTemplateHandler> _logger;

    public SendEmailFromTemplateHandler(
        MpssEmailProducer mpssEmailProducer,
        McsEmailProducer mcsEmailProducer,
        NotificationRepository repository,
        S3AdapterService s3Service,
        ILogger<SendEmailFromTemplateHandler> logger)
    {
        _mpssEmailProducer = mpssEmailProducer;
        _mcsEmailProducer = mcsEmailProducer;
        _repository = repository;
        _s3Service = s3Service;
        _logger = logger;
    }
    
    public async Task<SendEmailFromTemplateResponse> Handle(SendEmailFromTemplateRequest request, CancellationToken cancellationToken)
    {
        var notificationResult = await _repository.CreateEmailResult(
            request.Identifier?.Identity,
            request.Identifier?.IdentityScheme,
            request.CustomId,
            request.DocumentId,
            cancellationToken);
        var notificationId = notificationResult.Id;
     
        var attachments = new List<Attachment>();
        var host = request.From.Value.ToLowerInvariant().Split('@').Last();
        var bucketName = host == "kb.cz" ? Buckets.Mcs : Buckets.Mpss;
        
        try
        {
            foreach (var attachment in request.Attachments)
            {
                var objectKey = await _s3Service.UploadFile(attachment.Binary, bucketName);
                attachments.Add( EmailMappers.Map(attachment.Filename, objectKey));
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Could not upload attachments.");
            throw;
        }
        
        // todo:
        var sendEmail = new SendApi.v4.email.SendEmail
        {
            id = notificationId.ToString(),
            sender = request.From.Map(),
            // to = request.To.Map().ToList(),
            // bcc = request.Bcc.Map().ToList(),
            // cc = request.Cc.Map().ToList(),
            // replyTo = request.ReplyTo?.Map(),
            // subject = request.Subject,
            // content = request.Content.Map(),
            attachments = attachments
        };

        _logger.LogInformation("Sending email from template: {sendEmail}", JsonSerializer.Serialize(sendEmail));

        // todo: decide KB or MP
        await _mcsEmailProducer.SendEmail(sendEmail, cancellationToken);
        
        var updateResult = await _repository.UpdateResult(
            notificationId,
            NotificationState.Sent,
            token: cancellationToken);
        
        return new SendEmailFromTemplateResponse
        {
            NotificationId = Guid.NewGuid()
        };
    }
}