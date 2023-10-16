using CIS.Core;
using CIS.Core.Exceptions;
using CIS.InternalServices.NotificationService.Api.Configuration;
using CIS.InternalServices.NotificationService.Api.Services.Repositories.Abstraction;
using CIS.InternalServices.NotificationService.Api.Services.S3.Abstraction;
using CIS.InternalServices.NotificationService.Api.Services.Smtp;
using CIS.InternalServices.NotificationService.Api.Services.Smtp.Abstraction;
using CIS.InternalServices.NotificationService.Contracts.Result.Dto;
using MediatR;
using Microsoft.Extensions.Options;
using Entity = CIS.InternalServices.NotificationService.Api.Services.Repositories.Entities.Abstraction;
using Exception = System.Exception;

namespace CIS.InternalServices.NotificationService.Api.Messaging.Consumers.Email;

public class ConsumeSendEmailHandler : IRequestHandler<ConsumeSendEmailRequest, ConsumeSendEmailResponse>
{
    private readonly AppConfiguration _appConfiguration;
    private readonly IDateTime _dateTime;
    private readonly INotificationRepository _repository;
    private readonly ISmtpAdapterService _smtpAdapterService;
    private readonly IS3AdapterService _s3AdapterService;
    private readonly S3Buckets _buckets;
    private readonly ILogger<ConsumeSendEmailHandler> _logger;

    public ConsumeSendEmailHandler(
        IOptions<AppConfiguration> appOptions,
        IDateTime dateTime,
        INotificationRepository repository,
        ISmtpAdapterService smtpAdapterService,
        IS3AdapterService s3AdapterService,
        IOptions<AppConfiguration> options,
        ILogger<ConsumeSendEmailHandler> logger)
    {
        _appConfiguration = appOptions.Value;
        _dateTime = dateTime;
        _repository = repository;
        _smtpAdapterService = smtpAdapterService;
        _s3AdapterService = s3AdapterService;
        _logger = logger;
        _buckets = options.Value.S3Buckets;
    }

    private bool TryGetResult(ConsumeSendEmailRequest request, CancellationToken cancellationToken, out Entity.Result? result)
    {
        try
        {
            result = _repository.GetResult(request.Id, cancellationToken).GetAwaiter().GetResult();
            return true;
        }
        catch (CisNotFoundException e)
        {
            _logger.LogWarning(e, $"Cannot handle {nameof(ConsumeSendEmailRequest)}, because result with id = '{request.Id}' was not found.");
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"{nameof(ConsumeSendEmailHandler)} failed.");
        }

        result = default;
        return false;
    }

    private async Task<List<SmtpAttachment>> GetAttachments(
        ConsumeSendEmailRequest request,
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

    private bool AreWhitelisted(IEnumerable<string> emails) => emails.All(IsWhitelisted);

    private bool IsWhitelisted(string email) =>
        !_appConfiguration.EmailDomainWhitelist.Any() ||
        _appConfiguration.EmailDomainWhitelist.Any(w => email.EndsWith(w, StringComparison.OrdinalIgnoreCase));

    public async Task<ConsumeSendEmailResponse> Handle(ConsumeSendEmailRequest request, CancellationToken cancellationToken)
    {
        if (!TryGetResult(request, cancellationToken, out var result) || result is null)
        {
            return new ConsumeSendEmailResponse();
        }

        if (result.State is NotificationState.Sent or NotificationState.Error || result.ResultTimestamp != null)
        {
            _logger.LogWarning($"{nameof(ConsumeSendEmailRequest)} with id = '{result.Id}' was already processed.");
            return new ConsumeSendEmailResponse();
        }

        try
        {
            var emails = request.To.Union(request.Cc).Union(request.Bcc).ToList();
            if (!AreWhitelisted(emails))
            {
                throw new CisValidationException($"Could not send MPSS email to recipient outside the whitelist: {string.Join(", ", emails.Where(e => !IsWhitelisted(e)))}");
            }

            var smtpAttachments = await GetAttachments(request, cancellationToken);

            await _smtpAdapterService.SendEmail(
                request.Format,
                request.From, request.ReplyTo,
                request.Subject, request.Content,
                request.To, request.Cc, request.Bcc,
                smtpAttachments
            );

            result.State = NotificationState.Sent;
            _logger.LogDebug($"{nameof(ConsumeSendEmailRequest)} was handled. MPSS email was sent.");
        }
        catch (CisValidationException validationException)
        {
            var errorSet = new HashSet<ResultError>();
            errorSet.UnionWith(result.ErrorSet);
            errorSet.Add(new ResultError
            {
                // todo: code is not specified in IT ANA (used same Code as MCS uses)
                Code = "SMTP-WHITELIST-EXCEPTION",
                Message = validationException.Message
            });
            result.ErrorSet = errorSet;
            result.State = NotificationState.Error;
            _logger.LogError(validationException, $"Could not send email from {nameof(ConsumeSendEmailHandler)}.");
        }
        catch (Exception exception)
        {
            var errorSet = new HashSet<ResultError>();
            errorSet.UnionWith(result.ErrorSet);
            errorSet.Add(new ResultError
            {
                // todo: code is not specified in IT ANA (used same Code as MCS uses)
                Code = "SMTP-UNKNOWN-EXCEPTION",
                Message = exception.Message
            });
            result.ErrorSet = errorSet;
            result.State = NotificationState.Error;
            _logger.LogError(exception, $"{nameof(ConsumeSendEmailHandler)} failed.");
        }

        result.ResultTimestamp = _dateTime.Now;
        await _repository.SaveChanges(cancellationToken);

        return new ConsumeSendEmailResponse();
    }
}