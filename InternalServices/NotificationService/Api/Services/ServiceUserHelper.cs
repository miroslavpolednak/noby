using CIS.Core.Attributes;
using CIS.Core.Security;

namespace CIS.InternalServices.NotificationService.Api.Services;

[ScopedService, SelfService]
internal sealed class ServiceUserHelper
{
    public string UserName 
        => _serviceUser.User!.Name!;
    
    public string ConsumerId
        => _appConfiguration.Consumers.FirstOrDefault(t => t.Username == UserName)?.ConsumerId
        ?? throw new CIS.Core.Exceptions.CisConfigurationException(0, $"Consumer '{UserName}' not found in application configuration");

    private readonly Configuration.AppConfiguration _appConfiguration;
    private readonly Core.Security.IServiceUserAccessor _serviceUser;

    public ServiceUserHelper(IServiceUserAccessor serviceUser, Configuration.AppConfiguration appConfiguration)
    {
        _serviceUser = serviceUser;
        _appConfiguration = appConfiguration;
    }
}
