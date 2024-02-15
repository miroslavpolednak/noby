using System.ComponentModel.DataAnnotations;

namespace CIS.InternalServices.NotificationService.Api.Database.Entities;

internal abstract class BaseNotification
{
    [Key]
    public Guid Id { get; set; }

    public Contracts.v2.NotificationStates State { get; set; }

    public string? Identity { get; set; }

    public string? IdentityScheme { get; set; }

    public long? CaseId { get; set; }

    public string? CustomId { get; set; }

    public string? DocumentId { get; set; }

    public string? DocumentHash { get; set; }

    public string? HashAlgorithm { get; set; }

    public string? CreatedUserName { get; set; }

    public DateTime CreatedTime { get; set; }
}
