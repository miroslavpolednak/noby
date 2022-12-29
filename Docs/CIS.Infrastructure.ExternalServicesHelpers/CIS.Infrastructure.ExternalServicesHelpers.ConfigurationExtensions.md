#### [CIS.Infrastructure.ExternalServicesHelpers](index.md 'index')
### [CIS.Infrastructure.ExternalServicesHelpers](CIS.Infrastructure.ExternalServicesHelpers.md 'CIS.Infrastructure.ExternalServicesHelpers')

## ConfigurationExtensions Class

```csharp
public static class ConfigurationExtensions
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; ConfigurationExtensions
### Methods

<a name='CIS.Infrastructure.ExternalServicesHelpers.ConfigurationExtensions.AddExternalServiceConfiguration_TClient_(thisMicrosoft.AspNetCore.Builder.WebApplicationBuilder,string,string)'></a>

## ConfigurationExtensions.AddExternalServiceConfiguration<TClient>(this WebApplicationBuilder, string, string) Method

Načtení konfigurace externí služby a její vložení do DI.

```csharp
public static CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration<TClient> AddExternalServiceConfiguration<TClient>(this Microsoft.AspNetCore.Builder.WebApplicationBuilder builder, string serviceName, string serviceImplementationVersion)
    where TClient : class, CIS.Infrastructure.ExternalServicesHelpers.IExternalServiceClient;
```
#### Type parameters

<a name='CIS.Infrastructure.ExternalServicesHelpers.ConfigurationExtensions.AddExternalServiceConfiguration_TClient_(thisMicrosoft.AspNetCore.Builder.WebApplicationBuilder,string,string).TClient'></a>

`TClient`

Typ klienta - interface pro danou verzi proxy nad API třetí strany
#### Parameters

<a name='CIS.Infrastructure.ExternalServicesHelpers.ConfigurationExtensions.AddExternalServiceConfiguration_TClient_(thisMicrosoft.AspNetCore.Builder.WebApplicationBuilder,string,string).builder'></a>

`builder` [Microsoft.AspNetCore.Builder.WebApplicationBuilder](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.AspNetCore.Builder.WebApplicationBuilder 'Microsoft.AspNetCore.Builder.WebApplicationBuilder')

<a name='CIS.Infrastructure.ExternalServicesHelpers.ConfigurationExtensions.AddExternalServiceConfiguration_TClient_(thisMicrosoft.AspNetCore.Builder.WebApplicationBuilder,string,string).serviceName'></a>

`serviceName` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Název konzumované služby třetí strany

<a name='CIS.Infrastructure.ExternalServicesHelpers.ConfigurationExtensions.AddExternalServiceConfiguration_TClient_(thisMicrosoft.AspNetCore.Builder.WebApplicationBuilder,string,string).serviceImplementationVersion'></a>

`serviceImplementationVersion` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Verze proxy nad API třetí strany

#### Returns
[CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration&lt;](CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration_TClient_.md 'CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration<TClient>')[TClient](CIS.Infrastructure.ExternalServicesHelpers.ConfigurationExtensions.md#CIS.Infrastructure.ExternalServicesHelpers.ConfigurationExtensions.AddExternalServiceConfiguration_TClient_(thisMicrosoft.AspNetCore.Builder.WebApplicationBuilder,string,string).TClient 'CIS.Infrastructure.ExternalServicesHelpers.ConfigurationExtensions.AddExternalServiceConfiguration<TClient>(this Microsoft.AspNetCore.Builder.WebApplicationBuilder, string, string).TClient')[&gt;](CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration_TClient_.md 'CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration<TClient>')

#### Exceptions

[CIS.Core.Exceptions.CisConfigurationException](https://docs.microsoft.com/en-us/dotnet/api/CIS.Core.Exceptions.CisConfigurationException 'CIS.Core.Exceptions.CisConfigurationException')  
Chyba v konfiguraci služby - např. špatně zadaný typ implementace.

[CIS.Core.Exceptions.CisConfigurationNotFound](https://docs.microsoft.com/en-us/dotnet/api/CIS.Core.Exceptions.CisConfigurationNotFound 'CIS.Core.Exceptions.CisConfigurationNotFound')  
Konfigurace pro klíč ES:{serviceName}:{serviceImplementationVersion} nebyla nalezena v sekci ExternalServices v appsettings.json