namespace NOBY.Api.Notifications;

internal sealed record MainCustomerUpdatedNotification(
    long CaseId, 
    int SalesArrangementId, 
    int CustomerOnSAId,
    IEnumerable<CIS.Infrastructure.gRPC.CisTypes.Identity>? CustomerIdentifiers)
    : INotification
{
}
