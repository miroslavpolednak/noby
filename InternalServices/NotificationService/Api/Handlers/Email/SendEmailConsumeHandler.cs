using CIS.Core;
using CIS.Core.Exceptions;
using CIS.InternalServices.NotificationService.Api.Configuration;
using CIS.InternalServices.NotificationService.Api.Handlers.Email.Requests;
using CIS.InternalServices.NotificationService.Api.Services.Repositories;
using CIS.InternalServices.NotificationService.Api.Services.S3;
using CIS.InternalServices.NotificationService.Api.Services.Smtp;
using CIS.InternalServices.NotificationService.Contracts.Result.Dto;
using MediatR;
using Microsoft.Extensions.Options;
using Entity = CIS.InternalServices.NotificationService.Api.Services.Repositories.Entities.Abstraction;

namespace CIS.InternalServices.NotificationService.Api.Handlers.Email;

public class SendEmailConsumeHandler : IRequestHandler<SendEmailConsumeRequest, SendEmailConsumeResponse>
{
    private readonly IDateTime _dateTime;
    private readonly NotificationRepository _repository;
    private readonly SmtpAdapterService _smtpAdapterService;
    private readonly S3AdapterService _s3AdapterService;
    private readonly S3Buckets _buckets;
    private readonly ILogger<SendEmailConsumeHandler> _logger;

    public SendEmailConsumeHandler(
        IDateTime dateTime,
        NotificationRepository repository,
        SmtpAdapterService smtpAdapterService,
        S3AdapterService s3AdapterService,
        IOptions<AppConfiguration> options,
        ILogger<SendEmailConsumeHandler> logger)
    {
        _dateTime = dateTime;
        _repository = repository;
        _smtpAdapterService = smtpAdapterService;
        _s3AdapterService = s3AdapterService;
        _logger = logger;
        _buckets = options.Value.S3Buckets;
    }

    private bool TryGetResult(SendEmailConsumeRequest request, CancellationToken cancellationToken, out Entity.Result? result)
    {
        try
        {
            result = _repository.GetResult(request.Id, cancellationToken).GetAwaiter().GetResult();
            return true;
        }
        catch (CisNotFoundException e)
        {
            _logger.LogWarning(e, $"Cannot handle {nameof(SendEmailConsumeRequest)}, because result with id = '{request.Id}' was not found.");
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"{nameof(SendEmailConsumeHandler)} failed.");
        }

        result = default;
        return false;
    }

    private async Task<List<SmtpAttachment>> GetAttachments(
        SendEmailConsumeRequest request,
        CancellationToken token)
    {
        var smtpAttachments = new List<SmtpAttachment>();

        foreach (var attachment in request.Attachments)
        {
            var fileContent = await _s3AdapterService.GetFile(attachment.S3Key, _buckets.Mpss, token);
            smtpAttachments.Add(new SmtpAttachment { Filename = attachment.Filename, Binary = fileContent });
        }

        return smtpAttachments;
    }
    
    public async Task<SendEmailConsumeResponse> Handle(SendEmailConsumeRequest request, CancellationToken cancellationToken)
    {
        if (!TryGetResult(request, cancellationToken, out var result) || result is null)
        {
            return new SendEmailConsumeResponse();
        }

        if (result.State is NotificationState.Sent or NotificationState.Error || result.ResultTimestamp != null)
        {
            _logger.LogWarning($"{nameof(SendEmailConsumeRequest)} with id = '{result.Id}' was already processed.");
            return new SendEmailConsumeResponse();
        }
        
        try
        {
            var smtpAttachments =  await GetAttachments(request, cancellationToken);

            await _smtpAdapterService.SendEmail(
                request.From, request.ReplyTo,
                request.Subject, request.Content,
                request.To, request.Cc, request.Bcc,
                smtpAttachments
            );
            
            _logger.LogInformation($"{nameof(SendEmailConsumeRequest)} was handled. MPSS Email was sent.");
            result.State = NotificationState.Sent;
        }
        catch (Exception e)
        {
            // todo: result.ErrorSet
            result.State = NotificationState.Error;
            _logger.LogError(e, $"{nameof(SendEmailConsumeHandler)} failed.");
        }

        result.ResultTimestamp = _dateTime.Now;
        await _repository.SaveChanges(cancellationToken);

        return new SendEmailConsumeResponse();
    }
}