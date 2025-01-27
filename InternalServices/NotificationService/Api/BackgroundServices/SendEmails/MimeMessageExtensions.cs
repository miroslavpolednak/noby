﻿using System.Globalization;
using MimeKit;

namespace CIS.InternalServices.NotificationService.Api.BackgroundServices.SendEmails;

internal static class MimeMessageExtensions
{
    public static MimeMessage Create() => new();

    public static MimeMessage AddFrom(this MimeMessage message, string from)
    {
        message.From.Add(MailboxAddress.Parse(from));
        return message;
    }

    public static MimeMessage AddReplyTo(this MimeMessage message, string replyTo)
    {
        if (!string.IsNullOrEmpty(replyTo))
        {
            message.ReplyTo.Add(MailboxAddress.Parse(replyTo));
        }

        return message;
    }

    public static MimeMessage AddSubject(this MimeMessage message, string subject)
    {
        message.Subject = subject;
        return message;
    }

    public static MimeMessage AddTo(this MimeMessage message, IEnumerable<string> to)
    {
        foreach (var t in to)
        {
            if (!string.IsNullOrEmpty(t))
            {
                message.To.Add(MailboxAddress.Parse(t));
            }
        }

        return message;
    }

    public static MimeMessage AddCc(this MimeMessage message, IEnumerable<string>? cc)
    {
        if (cc is null || !cc.Any())
            return message;

        foreach (var c in cc)
        {
            if (!string.IsNullOrEmpty(c))
            {
                message.Cc.Add(MailboxAddress.Parse(c));
            }
        }

        return message;
    }

    public static MimeMessage AddBcc(this MimeMessage message, IEnumerable<string>? bcc)
    {
        if (bcc is null || !bcc.Any())
            return message;

        foreach (var b in bcc)
        {
            if (!string.IsNullOrEmpty(b))
            {
                message.Bcc.Add(MailboxAddress.Parse(b));
            }
        }

        return message;
    }

    public static MimeMessage AddContent(this MimeMessage message, string format, string content, IEnumerable<Dto.SmtpAttachment>? attachments)
    {
        var bodyBuilder = new BodyBuilder();

        if (format.Contains("html", StringComparison.CurrentCultureIgnoreCase))
        {
            bodyBuilder.HtmlBody = content;
        }
        else
        {
            bodyBuilder.TextBody = content;
        }

        if (attachments != null && attachments.Any())
        {
            foreach (var attachment in attachments)
            {
                bodyBuilder.Attachments.Add(attachment.Filename, attachment.Binary);
            }
        }        

        message.Body = bodyBuilder.ToMessageBody();

        return message;
    }
}
