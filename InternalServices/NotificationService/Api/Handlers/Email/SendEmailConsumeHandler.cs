﻿using CIS.InternalServices.NotificationService.Api.Handlers.Email.Requests;
using CIS.InternalServices.NotificationService.Api.Services.Repositories;
using CIS.InternalServices.NotificationService.Api.Services.S3;
using CIS.InternalServices.NotificationService.Api.Services.Smtp;
using CIS.InternalServices.NotificationService.Contracts.Result.Dto;
using MediatR;

namespace CIS.InternalServices.NotificationService.Api.Handlers.Email;

public class SendEmailConsumeHandler : IRequestHandler<SendEmailConsumeRequest, SendEmailConsumeResponse>
{
    private readonly NotificationRepository _repository;
    private readonly SmtpAdapterService _smtpAdapterService;
    private readonly S3AdapterService _s3AdapterService;

    public SendEmailConsumeHandler(
        NotificationRepository repository,
        SmtpAdapterService smtpAdapterService,
        S3AdapterService s3AdapterService)
    {
        _repository = repository;
        _smtpAdapterService = smtpAdapterService;
        _s3AdapterService = s3AdapterService;
    }
    
    public async Task<SendEmailConsumeResponse> Handle(SendEmailConsumeRequest request, CancellationToken cancellationToken)
    {
        var smtpAttachments = new List<SmtpAttachment>();

        foreach (var attachment in request.Attachments)
        {
            var fileContent = await _s3AdapterService.GetFile(attachment.S3Key, Buckets.Mpss);
            smtpAttachments.Add(new SmtpAttachment{ Filename = attachment.Filename, Binary = fileContent });
        }
        
        await _smtpAdapterService.SendEmail(
            request.From,
            request.ReplyTo,
            request.Subject,
            request.Content,
            request.To,
            request.Cc,
            request.Bcc,
            smtpAttachments
            );
        
        // todo: error handling
        var resultId = request.Id;
        await _repository.UpdateResult(resultId, NotificationState.Sent, token: cancellationToken);
        
        return new SendEmailConsumeResponse();
    }
}