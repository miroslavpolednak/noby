namespace CIS.InternalServices.NotificationService.Api.Services.User.Abstraction;

public interface IUserAdapterService
{
    string GetUsername();

    string GetConsumerId();

    UserAdapterService CheckSendEmailAccess();

    UserAdapterService CheckSendSmsAccess();

    UserAdapterService CheckReadResultAccess();
}