﻿using CIS.InternalServices.NotificationService.LegacyContracts.Result.Dto;
using MailKit.Net.Smtp;
using SharedComponents.DocumentDataStorage;
using System.Net.Security;
using CIS.Core.Exceptions;
using CIS.InternalServices.NotificationService.Api.Database;
using CIS.InternalServices.NotificationService.Api.Database.DocumentDataEntities;
using MimeKit;

namespace CIS.InternalServices.NotificationService.Api.BackgroundServices.SendEmails;

internal sealed class SendEmailsJob(SendEmailsJobConfiguration _configuration,
        NotificationDbContext _dbContext,
        IDocumentDataStorage _documentDataStorage,
        ILogger<SendEmailsJob> _logger,
        TimeProvider _dateTime)
    : Infrastructure.BackgroundServices.ICisBackgroundServiceJob
{

    public async Task ExecuteJobAsync(CancellationToken cancellationToken)
    {
        // vytahnout emaily k odeslani
        var emails_v1 = await _dbContext.EmailResults
            .Where(t => t.State == NotificationState.InProgress && t.SenderType == LegacyContracts.Statistics.Dto.SenderType.MP)
            .OrderBy(t => t.RequestTimestamp)
            .Take(_configuration.NumberOfEmailsAtOnce)
            .ToListAsync(cancellationToken);

        var emails_v2 = await _dbContext.Notifications
            .Where(t => t.Channel == Contracts.v2.NotificationChannels.Email && t.State == Contracts.v2.NotificationStates.InProgress && t.Mandant == Mandants.Mp)
            .OrderBy(t => t.CreatedTime)
            .Take(_configuration.NumberOfEmailsAtOnce)
            .ToListAsync(cancellationToken);

        if (emails_v1.Count == 0 && emails_v2.Count == 0)
            return;

        //vytvorit smtp klienta
        using var client = new SmtpClient()
        {
            CheckCertificateRevocation = false,
            Timeout = _configuration.SmtpConfiguration.Timeout * 1000,
            ServerCertificateValidationCallback = (sender, certificate, chain, errors) =>
            {
                if (errors == SslPolicyErrors.None)
                    return true;

                return _configuration.SmtpConfiguration.DisableServerCertificateValidation;
            }
        };

        // navazat spojeni
        await client.ConnectAsync(_configuration.SmtpConfiguration.Host, _configuration.SmtpConfiguration.Port, _configuration.SmtpConfiguration.SecureSocket, default);

        // odeslat emaily
        await SendEmails_v1(client, emails_v1);

        await SendEmails_v2(client, emails_v2);

        await client.DisconnectAsync(true, default);

    }

    private async Task SendEmails_v1(SmtpClient client, List<Database.Entities.EmailResult> emails)
    {
        if (emails.Count == 0)
            return;

        await SendEmails<SendEmail>(
           client: client,
           emails.Select(t => t.Id).ToList(),
           createEmail: (id, payload) =>
           {
               var data = payload.Data;
               // vytvorit zpravu
               var email = MimeMessageExtensions
               .Create()
               .AddFrom(data!.From.Value)
               .AddReplyTo(data!.ReplyTo?.Value ?? "")
               .AddSubject(data!.Subject)
               .AddTo(data!.To.Select(t => t.Value))
               .AddCc(data!.Cc.Select(t => t.Value))
               .AddBcc(data!.Bcc.Select(t => t.Value))
               .AddContent(data!.Content.Format, data!.Content.Text, data!.Attachments.Select(t => new Dto.SmtpAttachment
               {
                   Filename = t.Filename,
                   Binary = Convert.FromBase64String(t.Binary)
               }));

               return email;
           },
           send: (id) => {
               var email = emails.First(t => t.Id == id);

               // nastavit result
               email.State = NotificationState.Sent;
               email.ResultTimestamp = _dateTime.GetLocalNow().DateTime;
               email.ErrorSet = new HashSet<ResultError>();
               email.Resend = false;
           },
           error: (id, code, message) => {
               var email = emails.First(t => t.Id == id);

               // nastavit result
               email.State = NotificationState.Error;
               email.ErrorSet = new() {
                    new () { Code = code, Message = message }
               };
           },
           exception: (id, code, message) => {
               var email = emails.First(t => t.Id == id);

               // nastavit result
               email.State = NotificationState.InProgress;
               email.ErrorSet = new() {
                    new () { Code = code, Message = message }
               };
           },
           cancellationToken: default);
    }

    private async Task SendEmails_v2(SmtpClient client, List<Database.Entities.Notification> emails)
    {
        if (emails.Count == 0)
            return;
        
        await SendEmails<Database.DocumentDataEntities.EmailData>(
           client: client,
           emails.Select(t => t.Id).ToList(),
           createEmail: (id, payload) =>
           {
               // vytvorit zpravu
               var email = MimeMessageExtensions
               .Create()
               .AddFrom(payload.Sender.Value)
               .AddReplyTo(payload.ReplyTo?.Value ?? "")
               .AddSubject(payload.Subject)
               .AddTo(payload.To.Select(t => t.Value))
               .AddCc(payload.Cc?.Select(t => t.Value))
               .AddBcc(payload.Bcc?.Select(t => t.Value))
               .AddContent(payload.Format == Contracts.v2.SendEmailRequest.Types.EmailFormats.PlainText ? "text/plain" : "text/html", payload.Text, payload.Attachments?.Select(t => new Dto.SmtpAttachment
               {
                   Filename = t.Filename,
                   Binary = Convert.FromBase64String(t.Data)
               }));

               return email;
           },
           send: (id) => {
               var email = emails.First(t => t.Id == id);

               // nastavit result
               email.State = Contracts.v2.NotificationStates.Sent;
               email.ResultTime = _dateTime.GetLocalNow().DateTime;
               email.Errors = new ();
               email.Resend = false;
           },
           error: (id, code, message) => {
               var email = emails.First(t => t.Id == id);

               // nastavit result
               email.State = Contracts.v2.NotificationStates.Error;
               email.Errors = [ new () { Code = code, Message = message }];
           },
           exception: (id, code, message) => {
               var email = emails.First(t => t.Id == id);

               // nastavit result
               email.State = Contracts.v2.NotificationStates.InProgress;
               email.Errors = [new() { Code = code, Message = message }];
           },
           cancellationToken: default);
    }

    private async Task SendEmails<TPayload>(
        SmtpClient client,
        List<Guid> ids, 
        Func<Guid, TPayload, MimeMessage> createEmail,
        Action<Guid> send,
        Action<Guid, string, string> error,
        Action<Guid, string, string> exception,
        CancellationToken cancellationToken) 
        where TPayload : class, IDocumentData
    {
        _logger.SendEmailsJobStart(ids.Count);

        int emailsSent = 0;

        // projit vsechny emaily a odeslat je
        foreach (var id in ids)
        {
            try
            {
                // zkontrolovat jestli id neni uz odeslano / neodesila se v jine instanci
                // vlozit id do tabulky, aby nedoslo k nasobnemu odeslani z vice instanci NS
                // pokud id existuje, vraci se -1, proc nevraci 0 (rows affected)?
                if ((await _dbContext.Database.ExecuteSqlInterpolatedAsync(@$"
                        If (Not Exists(Select * From SentNotification Where Id = {id}))
                            Insert Into SentNotification Values ({id})", cancellationToken)) != 1)
                    continue;
            }
            catch
            {
                // id nejde vlozit (zamknout), pokracujeme na dalsi email
                continue;
            }

            try
            {
                // vytahnout k emailu payload (kvuli velikym priloham delat jednotlive)
                var payload = await _documentDataStorage.FirstOrDefaultByEntityId<TPayload, string>(id.ToString(), cancellationToken);

                if (payload is null || payload.Data is null)
                    throw new CisNotFoundException(0, $"Request payload for email result id {id} was not found.");

                // vytvorit zpravu
                var email = createEmail(id, payload.Data);

                // zkontrolovat whitelist
                if (_configuration.EmailDomainWhitelist.Count != 0)
                {
                    var emailAddresses = email.GetRecipients(true).Select(t => t.Address);

                    if (!emailAddresses.All(t => IsWhitelisted(t)))
                        throw new CisValidationException($"Could not send MPSS email to recipient outside the whitelist: {string.Join(", ", emailAddresses.Where(t => !IsWhitelisted(t)))}");
                }

                // oznacit jako odeslano
                send(id);

                await _dbContext.SaveChangesAsync(default);

                // odeslat
                var response = await client.SendAsync(email, cancellationToken);

                emailsSent++;
                try
                {
                    // ve v1 odstranit payload
                    if (typeof(TPayload) == typeof(SendEmail))
                        await _documentDataStorage.DeleteByEntityId<string, SendEmail>(id.ToString());
                } catch { }
            }
            catch (CisNotFoundException ex)
            {
                error(id, "SMTP-NON-EXISTING-PAYLOAD", ex.Message);

                await _dbContext.SaveChangesAsync(default);

                _logger.SendEmailsJobPayloadFailed(id, ex);
            }
            catch (CisValidationException ex)
            {
                error(id, "SMTP-WHITELIST-EXCEPTION", ex.Message);

                await _dbContext.SaveChangesAsync(default);

                _logger.SendEmailsJobValidationError(id, ex);
            }
            catch (Exception ex)
            {
                exception(id, "SMTP-UNKNOWN-EXCEPTION", ex.Message);

                // odstranit zamek, chceme zkusit odeslat znovu
                await _dbContext.Database.ExecuteSqlInterpolatedAsync($"Delete From SentNotification Where Id = {id}", cancellationToken);

                await _dbContext.SaveChangesAsync(default);

                _logger.SendEmailsJobFailedToSend(id, ex);
            }
        }

        _logger.SendEmailsJobEnd(emailsSent);
    }

    private bool IsWhitelisted(string email) =>
        _configuration.EmailDomainWhitelist.Count == 0 ||
        _configuration.EmailDomainWhitelist.Any(w => email.EndsWith(w, StringComparison.OrdinalIgnoreCase));
}
