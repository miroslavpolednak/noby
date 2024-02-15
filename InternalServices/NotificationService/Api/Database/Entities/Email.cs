using System.ComponentModel.DataAnnotations.Schema;

namespace CIS.InternalServices.NotificationService.Api.Database.Entities;

[Table("Email", Schema = "dbo")]
internal sealed class Email
    : BaseNotification
{
    public string From { get; set; } = null!;

    public string? ReplyTo { get; set; }

    public string[] To { get; set; }

    public string[]? Cc { get; set; }

    public string[]? Bcc { get; set; }

    public string Subject { get; set; } = null!;

    public string ContentText { get; set; } = null!;

    public string ContentFormat { get; set; } = null!;

    public string ContentLanguage { get; set; } = null!;
}
