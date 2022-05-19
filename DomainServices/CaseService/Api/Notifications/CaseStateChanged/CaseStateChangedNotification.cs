namespace DomainServices.CaseService.Api.Notifications;

internal class CaseStateChangedNotification
    : INotification
{
    public long CaseId { get; set; }
    public int CaseStateId { get; set; }
    public string? ContractNumber { get; set; }
    public int ProductTypeId { get; set; }
    public string? ClientName { get; set; }
    public int CaseOwnerUserId { get; set; }
}
