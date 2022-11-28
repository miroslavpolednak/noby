namespace CIS.Infrastructure.ExternalServicesHelpers.Configuration;

public interface IExternalServiceConfiguration<TClient> 
    : IExternalServiceConfiguration
    where TClient : class
{ }
