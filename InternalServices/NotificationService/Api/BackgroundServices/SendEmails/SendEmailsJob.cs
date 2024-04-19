using CIS.InternalServices.NotificationService.Contracts.Result.Dto;
using CIS.InternalServices.NotificationService.Api.Services.Repositories;
using CIS.InternalServices.NotificationService.Api.Services.Repositories.Entities;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using SharedComponents.DocumentDataStorage;
using System.Net.Security;
using CIS.Core.Exceptions;

namespace CIS.InternalServices.NotificationService.Api.BackgroundServices.SendEmails;

public sealed class SendEmailsJob
    : Infrastructure.BackgroundServices.ICisBackgroundServiceJob
{
    private readonly SendEmailsJobConfiguration _configuration;
    private readonly NotificationDbContext _dbContext;
    private readonly IDocumentDataStorage _documentDataStorage;
    private readonly ILogger<SendEmailsJob> _logger;
    private readonly TimeProvider _dateTime;

    public SendEmailsJob(
        SendEmailsJobConfiguration configuration, 
        NotificationDbContext dbContext, 
        IDocumentDataStorage documentDataStorage,
        ILogger<SendEmailsJob> logger,
        TimeProvider dateTime)
    {
        _configuration = configuration;
        _dbContext = dbContext;
        _documentDataStorage = documentDataStorage;
        _logger = logger;
        _dateTime = dateTime;
    }

    public async Task ExecuteJobAsync(CancellationToken cancellationToken)
    {
        // vytahnout emaily k odeslani
        var emails = await _dbContext.EmailResults
            .Where(t => t.State == NotificationState.InProgress && t.SenderType == Contracts.Statistics.Dto.SenderType.MP)
            .OrderBy(t => Guid.NewGuid())
            .Take(_configuration.NumberOfEmailsAtOnce)
            .ToListAsync(cancellationToken);

        if (emails.Count == 0)
            return;

        _logger.LogInformation($"Number of emails to send from {nameof(SendEmailsJob)}: {emails.Count}");

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
        var emailPayloads = await _documentDataStorage.GetList<SendEmail, string>(emails.Select(t => t.Id.ToString()).ToArray(), cancellationToken);

        // projit vsechny emaily a odeslat je
        foreach (var email in emails)
        {
            try
            {
                // zkontrolovat jestli id neni uz odeslano / neodesila se v jine instanci
                // vlozit id do tabulky, aby nedoslo k nasobnemu odeslani z vice instanci NS
                // pokud id existuje, vraci se -1, proc nevraci 0 (rows affected)?
                if ((await _dbContext.Database.ExecuteSqlInterpolatedAsync(@$"
                        If (Not Exists(Select * From SentNotification Where Id = {email.Id}))
                            Insert Into SentNotification Values ({email.Id})", default)) != 1)
                    continue;
            } catch
            {
                // id nejde vlozit (zamknout), pokracujeme na dalsi email
                continue;
            }
            
            try
            {
                var payload = emailPayloads.FirstOrDefault(t => t.EntityId == email.Id.ToString())?.Data!.Data!;

                if (payload is null)
                    throw new CisNotFoundException(0, $"Request payload for email result id {email.Id} was not found.");

                // zkontrolovat whitelist
                if (_configuration.EmailDomainWhitelist.Count != 0)
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
                .AddContent(payload.Content.Format, payload.Content.Text, payload.Attachments.Select(t => new Dto.SmtpAttachment
                {
                    Filename = t.Filename,
                    Binary = Convert.FromBase64String(t.Binary)
                }));

                // nastavit result
                email.State = NotificationState.Sent;
                email.ResultTimestamp = _dateTime.GetLocalNow().DateTime;
                email.ErrorSet = new HashSet<ResultError>();
                email.Resend = false;

                await _dbContext.SaveChangesAsync(default);

                // odeslat
                var response = await client.SendAsync(message, cancellationToken = default);
                _logger.LogInformation($"Email '{email.Id}' was sent: {response}");
            }
            catch (CisNotFoundException ex)
            {
                var errorSet = new HashSet<ResultError>();
                errorSet.UnionWith(email.ErrorSet);
                errorSet.Add(new ResultError
                {
                    // todo: code is not specified in IT ANA (used same Code as MCS uses)
                    Code = "SMTP-NON-EXISTING-PAYLOAD",
                    Message = ex.Message
                });
                email.ErrorSet = errorSet;
                // znovuodeslani uz nechceme
                email.State = NotificationState.Error;
                await _dbContext.SaveChangesAsync(default);

                _logger.LogError(ex, $"Cannot handle {nameof(SendEmailsJob)}, original request payload with id '{email.Id}' was not found.");
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
                // znovuodeslani uz nechceme
                email.State = NotificationState.Error;
                await _dbContext.SaveChangesAsync(default);

                _logger.LogError(validationException, $"Could not send email from {nameof(SendEmailsJob)}.");
            }
            catch (Exception exception)
            {
                var errorSet = new HashSet<ResultError>
                {
                    new ResultError
                    {
                        // todo: code is not specified in IT ANA (used same Code as MCS uses)
                        Code = "SMTP-UNKNOWN-EXCEPTION",
                        Message = exception.Message
                    }
                };
                // nastavit state zpet na InProgress
                // pokud jde puvodne resend, bude se email zkouset odeslat tak dlouho, nez ho zneplatni SetExpiredEmailsJob
                email.State = NotificationState.InProgress;
                email.ErrorSet = errorSet;
                await _dbContext.SaveChangesAsync(default);

                // odstranit zamek, chceme zkusit odeslat znovu
                await _dbContext.Database.ExecuteSqlInterpolatedAsync($"Delete From SentNotification Where Id = {email.Id}", default);

                _logger.LogError(exception, $"{nameof(SendEmailsJob)} failed.");
            }

            // odstranit payload
            try
            {
                if (email.State == NotificationState.Sent)
                    await _documentDataStorage.DeleteByEntityId<string, SendEmail>(email.Id.ToString());
            }
            catch { }
        }

        await client.DisconnectAsync(true, default);

        _logger.LogInformation($"Number of emails sent from {nameof(SendEmailsJob)}: {emails.Where(t => t.State == NotificationState.Sent).Count()}");
    }

    private bool IsWhitelisted(string email) =>
        _configuration.EmailDomainWhitelist.Count == 0 ||
        _configuration.EmailDomainWhitelist.Any(w => email.EndsWith(w, StringComparison.OrdinalIgnoreCase));
}
