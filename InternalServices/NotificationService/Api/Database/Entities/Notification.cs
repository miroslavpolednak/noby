﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CIS.InternalServices.NotificationService.Api.Database.Entities;

[Table("Notification", Schema = "dbo")]
internal sealed class Notification
{
    [Key]
    public Guid Id { get; set; }

    public Contracts.v2.NotificationChannels Channel { get; set; }

    public Contracts.v2.NotificationStates State { get; set; }

    public string? Identity { get; set; }

    public SharedTypes.GrpcTypes.UserIdentity.Types.UserIdentitySchemes? IdentityScheme { get; set; }

    public long? CaseId { get; set; }

    public string? CustomId { get; set; }

    public string? DocumentId { get; set; }

    public string? DocumentHash { get; set; }

    public Contracts.v2.DocumentHash.Types.HashAlgorithms? HashAlgorithm { get; set; }

    public string? CreatedUserName { get; set; }

    public DateTime CreatedTime { get; set; }

    public DateTime? ResultTime { get; set; }

    public List<NotificationError>? Errors { get; set; }

    public Mandants? Mandant { get; set; }

    public bool Resend { get; set; }
}

internal sealed class NotificationError
{
    public string Code { get; set; } = null!;
    public string Message { get; set; } = null!;
}