using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using CIS.InternalServices.NotificationService.Contracts.Result.Dto;
using Microsoft.EntityFrameworkCore;

namespace CIS.InternalServices.NotificationService.Api.Services.Repositories.Entities.Abstraction;

[Index(nameof(CustomId), nameof(Identity), nameof(IdentityScheme), nameof(DocumentId), IsUnique = false)]
public abstract class Result
{
    public Guid Id { get; set; }
    
    public NotificationState State { get; set; }
    
    public NotificationChannel Channel { get; set; }
    
    public string? CustomId { get; set; }
    
    public string? Identity { get; set; }
    public string? IdentityScheme { get; set; }
    
    public string? DocumentId { get; set; }
    
    public DateTime? RequestTimestamp { get; set; }
    
    public DateTime? HandoverToMcsTimestamp { get; set; }
    
    public string Errors { get; set; } = null!;
    
    [NotMapped]
    public HashSet<string> ErrorSet
    {
        get => JsonSerializer.Deserialize<HashSet<string>>(Errors)!;
        set => Errors = JsonSerializer.Serialize(value);
    }

    public abstract Contracts.Result.Dto.Abstraction.Result ToDto();
}