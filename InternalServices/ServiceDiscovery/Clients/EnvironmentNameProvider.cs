namespace CIS.InternalServices.ServiceDiscovery.Clients;

internal sealed class EnvironmentNameProvider
{
    public bool IsSet { get; init; }

    public Core.Types.ApplicationEnvironmentName Name { get; init; }

    public EnvironmentNameProvider(string? name)
    {
        if (string.IsNullOrEmpty(name))
        {
            IsSet = false;
            Name = new Core.Types.ApplicationEnvironmentName("");
        }
        else
        {
            IsSet = true;
            Name = new(name);
        }
    }
}
