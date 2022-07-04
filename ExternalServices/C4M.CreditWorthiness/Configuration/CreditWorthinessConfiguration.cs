namespace ExternalServices.C4M.CreditWorthiness.Configuration;

public sealed class CreditWorthinessConfiguration
    : CIS.ExternalServicesHelpers.Configuration.ExternalServiceBaseConfiguration
{
    public Versions Version { get; set; } = Versions.Unknown;
}
