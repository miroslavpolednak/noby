using System.ComponentModel.DataAnnotations.Schema;

namespace CIS.InternalServices.NotificationService.Api.Database.Entities;

[Table("Sms", Schema = "dbo")]
internal sealed class Sms
    : BaseNotification
{
    public string Text { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string CountryCode { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public int? ProcessingPriority { get; set; }
}
