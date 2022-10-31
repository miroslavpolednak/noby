using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using CIS.InternalServices.NotificationService.Contracts.Result.Dto;

namespace CIS.InternalServices.NotificationService.Api.Repositories.Entities;

[Table("Result", Schema = "dbo")]
public class Result
{
    [Key]
    public Guid Id { get; set; }
    public NotificationState State { get; set; }
    public NotificationChannel Channel { get; set; }
    
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }
    public string Errors { get; set; }
    
    [NotMapped]
    public HashSet<string> ErrorSet
    {
        get => JsonSerializer.Deserialize<HashSet<string>>(Errors)!;
        set => Errors = JsonSerializer.Serialize(value);
    }
}