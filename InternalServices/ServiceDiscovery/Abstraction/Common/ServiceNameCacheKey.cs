using CIS.InternalServices.ServiceDiscovery.Contracts;

namespace CIS.InternalServices.ServiceDiscovery.Common;

internal record ServiceNameCacheKey(ServiceTypes ServiceType, string ServiceName)
{
    public override string ToString()
        => $"{(int)ServiceType}:{ServiceName}";

    public static implicit operator string(ServiceNameCacheKey key) => key.ToString();

    public static (ServiceTypes ServiceType, string ServiceName) Deconstruct(string key)
    {
        int idx = key.IndexOf(':');
        return (ServiceType: (ServiceTypes)Convert.ToInt32(key.Substring(0, idx), System.Globalization.CultureInfo.InvariantCulture), ServiceName: key.Substring(idx + 1));
    }
}
