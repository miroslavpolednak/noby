using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIS.InternalServices.NotificationService.Api.Database.Entities;

[Table("Email", Schema = "dbo")]
internal sealed class Email
{
    [Key]
    public Guid Id { get; set; }

    public Contracts.v2.NotificationStates State { get; set; }

    public string Text { get; set; } = null!;

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
