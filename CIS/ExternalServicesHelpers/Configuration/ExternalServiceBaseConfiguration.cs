﻿using CIS.ExternalServicesHelpers.Configuration;
using CIS.Foms.Enums;

namespace CIS.ExternalServicesHelpers.Configuration;

public abstract class ExternalServiceBaseConfiguration
    : IExternalServiceConfiguration
{
    [Obsolete]
    public abstract string GetVersion();

    public int? RequestTimeout { get; set; } = 10;

    public string ServiceUrl { get; set; } = "";

    public bool UseServiceDiscovery { get; set; } = true;

    public ServiceImplementationTypes ImplementationType { get; set; } = ServiceImplementationTypes.Unknown;
}

public class ExternalServiceBaseConfiguration<TClient>
    : ExternalServiceBaseConfiguration, IExternalServiceConfiguration<TClient>
    where TClient : class
{
    [Obsolete]
    public override string GetVersion() => throw new NotImplementedException();

}