namespace FOMS.Api.Notifications;

internal record CustomerFullyIdentifiedNotification(
    long CaseId, 
    int SalesArrangementId, 
    CIS.Foms.Types.CustomerIdentity Identity,
    int NewMpCustomerId)
    : INotification
{
}
