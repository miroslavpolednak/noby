#### [CIS.Infrastructure.ExternalServicesHelpers](index.md 'index')
### [CIS.Infrastructure.ExternalServicesHelpers.Configuration](CIS.Infrastructure.ExternalServicesHelpers.Configuration.md 'CIS.Infrastructure.ExternalServicesHelpers.Configuration')

## IExternalServiceConfiguration<TClient> Interface

Generická verze konfigurace.

```csharp
public interface IExternalServiceConfiguration<TClient> :
CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration
    where TClient : class
```
#### Type parameters

<a name='CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration_TClient_.TClient'></a>

`TClient`

Typ HTTP klienta

Derived  
&#8627; [ExternalServiceConfiguration&lt;TClient&gt;](CIS.Infrastructure.ExternalServicesHelpers.Configuration.ExternalServiceConfiguration_TClient_.md 'CIS.Infrastructure.ExternalServicesHelpers.Configuration.ExternalServiceConfiguration<TClient>')

Implements [IExternalServiceConfiguration](CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration.md 'CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration')