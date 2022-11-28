namespace CIS.ExternalServicesHelpers.Configuration;

public interface IExternalServiceConfiguration<TClient> 
    : IExternalServiceConfiguration
    where TClient : class
{ }
