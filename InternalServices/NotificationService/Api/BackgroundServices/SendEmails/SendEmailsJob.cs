using CIS.InternalServices.NotificationService.Api.Services.Repositories;
using CIS.InternalServices.NotificationService.Contracts.Result.Dto;
using CIS.InternalServices.NotificationService.Api.Services.Repositories.Entities;
using CIS.InternalServices.NotificationService.Api.Configuration;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using SharedComponents.DocumentDataStorage;
using System.Net.Security;
using CIS.Core;
using CIS.Core.Exceptions;
using System.Linq;
using CIS.InternalServices.NotificationService.Api.Services.Smtp;
using System.Threading;

namespace CIS.InternalServices.NotificationService.Api.BackgroundServices.SendEmails;

public sealed class SendEmailsJob
    : Infrastructure.BackgroundServices.ICisBackgroundServiceJob
{
    private readonly AppConfiguration _appConfiguration;
    private readonly SendEmailsJobConfiguration _configuration;
    private readonly NotificationDbContext _dbContext;
    private readonly IDocumentDataStorage _documentDataStorage;
    private readonly ILogger<SendEmailsJob> _logger;
    private readonly IDateTime _dateTime;

    public SendEmailsJob(
        AppConfiguration appConfiguration,
        SendEmailsJobConfiguration configuration, 
        NotificationDbContext dbContext, 
        IDocumentDataStorage documentDataStorage,
        ILogger<SendEmailsJob> logger,
        IDateTime dateTime)
    {
        _appConfiguration = appConfiguration;
        _configuration = configuration;
        _dbContext = dbContext;
        _documentDataStorage = documentDataStorage;
        _logger = logger;
        _dateTime = dateTime;
    }

    public async Task ExecuteJobAsync(CancellationToken cancellationToken)
    {
        // nastavit unsent emailum ktere uz nejsou k odeslani
        await UpdateExpired(cancellationToken);

        // vytahnout emaily k odeslani
        var emails = await _dbContext.EmailResults
            .Where(t => t.State == NotificationState.InProgress && t.SenderType == Contracts.Statistics.Dto.SenderType.MP)
            .Where(t => t.RequestTimestamp >= _dateTime.Now.AddMinutes(_configuration.EmailSlaInMinutes * -1) || t.Resent)
            .OrderBy(t => t.RequestTimestamp)
            .Take(_configuration.NumberOfEmailsAtOnce)
            .ToListAsync(cancellationToken);

        if (!emails.Any())
            return;

        // vytvorit smtp klienta
        using var client = new SmtpClient()
        {
            CheckCertificateRevocation = false,
            Timeout = _configuration.SmtpConfiguration.Timeout * 1000,
            ServerCertificateValidationCallback = (sender, certificate, chain, errors) => {
                if (errors == SslPolicyErrors.None)
                    return true;

                return _configuration.SmtpConfiguration.DisableServerCertificateValidation;
            }
        };

        // navazat spojeni
        await client.ConnectAsync(_configuration.SmtpConfiguration.Host, _configuration.SmtpConfiguration.Port, _configuration.SmtpConfiguration.SecureSocket, default);

        // vytahnout k emailum payload
        var emailPayloads = await _documentDataStorage.GetList<SendEmail>(emails.Select(t => t.Id.ToString()).ToArray(), cancellationToken);

        // projit vsechny emaily a odeslat je
        foreach (var email in emails)
        {
            try
            {
                // vlozit id do tabulky, aby nedoslo k nasobnemu odeslani z vice instanci NS
                await _dbContext.Database.ExecuteSqlInterpolatedAsync($"Insert Into SentNotification Values {email.Id}", default);
            } catch(Exception ex)
            {
                // id nejde vlozit (zamknout), pokracujeme na dalsi email
                continue;
            }

            // zkontrolovat jestli ma email payload
            if (!emailPayloads.Any(t => t.EntityId == email.Id.ToString()))
            {
                _logger.LogWarning($"Cannot handle {nameof(SendEmailsJob)}, because original request payload with id = '{email.Id}' was not found.");
                continue;
            }
            
            try
            {
                var payload = emailPayloads.First(t => t.EntityId == email.Id.ToString()).Data!.Data!;
                // zkontrolovat whitelist
                // TODO: proc je to tady a ne ve validaci requestu?
                if (_appConfiguration.EmailDomainWhitelist.Any())
                {
                    var emailAddresses = payload.To.Select(t => t.Value)
                    .Union(payload.Cc.Select(t => t.Value))
                    .Union(payload.Bcc.Select(t => t.Value))
                    .ToList();
                    if (!emailAddresses.All(t => IsWhitelisted(t)))
                        throw new CisValidationException($"Could not send MPSS email to recipient outside the whitelist: {string.Join(", ", emailAddresses.Where(t => !IsWhitelisted(t)))}");
                }

                // vytvorit zpravu
                var message = MimeMessageExtensions
                .Create()
                .AddFrom(payload.From.Value)
                .AddReplyTo(payload.ReplyTo?.Value ?? "")
                .AddSubject(payload.Subject)
                .AddTo(payload.To.Select(t => t.Value))
                .AddCc(payload.Cc.Select(t => t.Value))
                .AddBcc(payload.Bcc.Select(t => t.Value))
                .AddContent(payload.Content.Format, payload.Content.Text, payload.Attachments.Select(t => new SmtpAttachment
                {
                    Filename = t.Filename,
                    Binary = Convert.FromBase64String(t.Binary)
                }));

                // nastavit result
                email.State = NotificationState.Sent;
                email.ResultTimestamp = _dateTime.Now;
                email.Errors = string.Empty;
                email.Resent = false;

                await _dbContext.SaveChangesAsync(default);

                // odeslat
                await client.SendAsync(message, cancellationToken = default);
            }
            catch (CisValidationException validationException)
            {
                var errorSet = new HashSet<ResultError>();
                errorSet.UnionWith(email.ErrorSet);
                errorSet.Add(new ResultError
                {
                    // todo: code is not specified in IT ANA (used same Code as MCS uses)
                    Code = "SMTP-WHITELIST-EXCEPTION",
                    Message = validationException.Message
                });
                email.ErrorSet = errorSet;
                email.State = NotificationState.Error;
                _logger.LogError(validationException, $"Could not send email from {nameof(SendEmailsJob)}.");
            }
            catch (Exception exception)
            {
                var errorSet = new HashSet<ResultError>();
                errorSet.UnionWith(email.ErrorSet);
                errorSet.Add(new ResultError
                {
                    // todo: code is not specified in IT ANA (used same Code as MCS uses)
                    Code = "SMTP-UNKNOWN-EXCEPTION",
                    Message = exception.Message
                });
                email.ErrorSet = errorSet;
                _logger.LogError(exception, $"{nameof(SendEmailsJob)} failed.");
            }
        }

        await client.DisconnectAsync(true, default);

    }

    private bool IsWhitelisted(string email) =>
        !_appConfiguration.EmailDomainWhitelist.Any() ||
        _appConfiguration.EmailDomainWhitelist.Any(w => email.EndsWith(w, StringComparison.OrdinalIgnoreCase));

    private async Task UpdateExpired(CancellationToken cancellationToken)
    {
        var emails = await _dbContext.EmailResults
            .Where(t => t.State == NotificationState.InProgress && t.SenderType == Contracts.Statistics.Dto.SenderType.MP)
            .Where(t => t.RequestTimestamp < _dateTime.Now.AddMinutes(_configuration.EmailSlaInMinutes * -1) && !t.Resent)
            .ToListAsync(cancellationToken);

        if (emails.Any())
        {
            foreach (var email in emails)
            {
                email.State = NotificationState.Unsent;
                email.ResultTimestamp = _dateTime.Now;
            }
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
