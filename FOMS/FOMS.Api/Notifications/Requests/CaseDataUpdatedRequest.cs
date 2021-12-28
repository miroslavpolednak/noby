namespace FOMS.Api.Notifications.Requests;

internal sealed class CaseDataUpdatedRequest
    : INotification
{
    public long CaseId { get; set; }

    public int? TargetAmount { get; set; }
    public bool? IsActionRequired { get; set; }
    public string? ContractNumber { get; set; }
}
