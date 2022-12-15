using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CIS.InternalServices.NotificationService.Api.Services.Repositories.Entities.Abstraction;
using Microsoft.EntityFrameworkCore;

namespace CIS.InternalServices.NotificationService.Api.Services.Repositories.Entities;

[Table("SmsTrackingData", Schema = "dbo")]
[Index(nameof(CustomId), nameof(ClientId), nameof(DocumentId), IsUnique = false)]
public class SmsTrackingData : ITrackingData
{
    [Key]
    public long Id { get; set; }
    
    [ForeignKey(nameof(Result))]
    public Guid ResultId { get; set; }
    
    public string? CustomId { get; set; }

    public string? ClientId { get; set; }
    
    public string? DocumentId { get; set; }

    public string PhoneNumber { get; set; } = null!;
    
    public DateTime? ReceiveTimestamp { get; set; }
    
    public DateTime? McsTimestamp { get; set; }
    
    public DateTime? OperatorTimestamp { get; set; }
    public virtual Result Result { get; set; } = null!;
}