namespace CIS.InternalServices.NotificationService.Api.Database.DocumentDataEntities;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

internal sealed class EmailData
    : SharedComponents.DocumentDataStorage.IDocumentData
{
    public int Version => 1;

    public string Subject { get; set; }

    public string Text { get; set; }

    public Contracts.v2.SendEmailRequest.Types.EmailAddress Sender { get; set; }

    public List<Contracts.v2.SendEmailRequest.Types.EmailAddress> To { get; set; }

    public List<Contracts.v2.SendEmailRequest.Types.EmailAddress>? Cc { get; set; }

    public List<Contracts.v2.SendEmailRequest.Types.EmailAddress>? Bcc { get; set; }

    public Contracts.v2.SendEmailRequest.Types.EmailAddress? ReplyTo { get; set; }

    public Contracts.v2.SendEmailRequest.Types.EmailFormats Format { get; set; }

    public Contracts.v2.SendEmailRequest.Types.EmailLanguages Language { get; set; }

    public List<EmailAttachment>? Attachments { get; set; } = new();

    public sealed class EmailAttachment
    {
        public string Data { get; set; }

        public string Filename { get; set; }
    }
}
