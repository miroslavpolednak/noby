namespace NOBY.Api.Notifications;

internal sealed record MainCustomerUpdatedNotification(
    long CaseId, 
    int SalesArrangementId, 
    int CustomerOnSAId,
    IEnumerable<SharedTypes.GrpcTypes.Identity>? CustomerIdentifiers)
    : INotification
{
}
