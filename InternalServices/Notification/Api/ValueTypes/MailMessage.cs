using System;
using System.Collections.Generic;
using System.Linq;

namespace CIS.InternalServices.Notification
{
    internal class MailMessage
    {
        public MailMessage(Contracts.Email request)
        {
            if (string.IsNullOrEmpty(request.Subject))
                throw new ArgumentNullException("Subject", "");
            else if (request.Subject.Length > 200)
                throw new ArgumentOutOfRangeException("Subject", $"Subject max length is 200, current: {request.Subject.Length}");

            Subject = request.Subject;
            HtmlBody = request.HtmlBody;
            FromAddress = new(request.FromAddress);
            if (request.Recipients != null && request.Recipients.Any())
                Recipients = request.Recipients.Select(t => new MailAddress(t)).ToList();
            if (request.Cc != null && request.Cc.Any())
                Cc = request.Cc.Select(t => new MailAddress(t)).ToList();
            if (request.Bcc != null && request.Bcc.Any())
                Bcc = request.Bcc.Select(t => new MailAddress(t)).ToList();
        }

        public MailAddress FromAddress { get; init; }

        public List<MailAddress> Recipients { get; init; }

        public List<MailAddress> Cc { get; set; }

        public List<MailAddress> Bcc { get; init; }

        public string Subject { get; init; }

        public string HtmlBody { get; init; }
    }
}
