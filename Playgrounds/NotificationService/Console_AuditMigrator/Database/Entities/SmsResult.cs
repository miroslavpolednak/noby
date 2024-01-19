using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using CIS.InternalServices.NotificationService.LegacyContracts.Result.Dto;

namespace Console_AuditMigrator.Database.Entities;

public class SmsResult
{
    public Guid Id { get; set; }
    
    public NotificationState State { get; set; }
    
    public NotificationChannel Channel { get; set; }

    public string? Identity { get; set; }
    
    public string? IdentityScheme { get; set; }
    
    public long? CaseId { get; set; }
    
    public string? CustomId { get; set; }
    
    public string? DocumentId { get; set; }
    
    public string? DocumentHash { get; set; }
    
    public string? HashAlgorithm { get; set; }
    
    public DateTime? RequestTimestamp { get; set; }
    
    public DateTime? ResultTimestamp { get; set; }
    
    public string Errors { get; set; } = null!;
    
    [NotMapped]
    public HashSet<ResultError> ErrorSet
    {
        get => JsonSerializer.Deserialize<HashSet<ResultError>>(Errors)!;
        set => Errors = JsonSerializer.Serialize(value);
    }
    
    public string CreatedBy { get; set; } = null!;
    
    public string Type { get; set; } = null!;

    public string CountryCode { get; set; } = null!;
    
    public string PhoneNumber { get; set; } = null!;
}