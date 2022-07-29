namespace FOMS.Api.Notifications;

internal record MainCustomerUpdatedNotification(
    long CaseId, 
    int SalesArrangementId, 
    int CustomerOnSAId,
    long? NewMpCustomerId)
    : INotification
{
}
