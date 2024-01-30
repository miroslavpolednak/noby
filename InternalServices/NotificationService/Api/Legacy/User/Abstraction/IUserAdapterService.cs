namespace CIS.InternalServices.NotificationService.Api.Services.User.Abstraction;

internal interface IUserAdapterService
{
    string GetUsername();

    string GetConsumerId();

    UserAdapterService CheckSendEmailAccess();

    UserAdapterService CheckSendSmsAccess();

    UserAdapterService CheckReadResultAccess();

    UserAdapterService CheckReceiveStatisticsAccess();

    UserAdapterService CheckResendNotificationsAccess();
}