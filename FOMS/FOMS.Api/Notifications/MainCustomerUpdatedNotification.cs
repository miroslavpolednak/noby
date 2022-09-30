namespace FOMS.Api.Notifications;

internal record MainCustomerUpdatedNotification(
    long CaseId, 
    int SalesArrangementId, 
    int CustomerOnSAId,
    IEnumerable<CIS.Infrastructure.gRPC.CisTypes.Identity>? CustomerIdentifiers)
    : INotification
{
}
