#### [CIS.Infrastructure.ExternalServicesHelpers](index.md 'index')
### [CIS.Infrastructure.ExternalServicesHelpers](CIS.Infrastructure.ExternalServicesHelpers.md 'CIS.Infrastructure.ExternalServicesHelpers')

## StartupRestExtensions Class

```csharp
public static class StartupRestExtensions
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; StartupRestExtensions
### Methods

<a name='CIS.Infrastructure.ExternalServicesHelpers.StartupRestExtensions.AddExternalServiceRestClient_TClient,TImplementation,TConfiguration_(thisMicrosoft.AspNetCore.Builder.WebApplicationBuilder,string,string,TConfiguration,System.Action_Microsoft.Extensions.DependencyInjection.IHttpClientBuilder,TConfiguration_)'></a>

## StartupRestExtensions.AddExternalServiceRestClient<TClient,TImplementation,TConfiguration>(this WebApplicationBuilder, string, string, TConfiguration, Action<IHttpClientBuilder,TConfiguration>) Method

Založení typed HttpClienta pro implementaci ExternalService.

```csharp
public static Microsoft.Extensions.DependencyInjection.IHttpClientBuilder AddExternalServiceRestClient<TClient,TImplementation,TConfiguration>(this Microsoft.AspNetCore.Builder.WebApplicationBuilder builder, string serviceName, string serviceImplementationVersion, TConfiguration configuration, System.Action<Microsoft.Extensions.DependencyInjection.IHttpClientBuilder,TConfiguration>? additionalHandlersRegistration=null)
    where TClient : class, CIS.Infrastructure.ExternalServicesHelpers.IExternalServiceClient
    where TImplementation : class, TClient
    where TConfiguration : class, CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration<TClient>;
```
#### Type parameters

<a name='CIS.Infrastructure.ExternalServicesHelpers.StartupRestExtensions.AddExternalServiceRestClient_TClient,TImplementation,TConfiguration_(thisMicrosoft.AspNetCore.Builder.WebApplicationBuilder,string,string,TConfiguration,System.Action_Microsoft.Extensions.DependencyInjection.IHttpClientBuilder,TConfiguration_).TClient'></a>

`TClient`

Typ klienta - interface pro danou verzi proxy nad API třetí strany

<a name='CIS.Infrastructure.ExternalServicesHelpers.StartupRestExtensions.AddExternalServiceRestClient_TClient,TImplementation,TConfiguration_(thisMicrosoft.AspNetCore.Builder.WebApplicationBuilder,string,string,TConfiguration,System.Action_Microsoft.Extensions.DependencyInjection.IHttpClientBuilder,TConfiguration_).TImplementation'></a>

`TImplementation`

Interní implementace TClient interface

<a name='CIS.Infrastructure.ExternalServicesHelpers.StartupRestExtensions.AddExternalServiceRestClient_TClient,TImplementation,TConfiguration_(thisMicrosoft.AspNetCore.Builder.WebApplicationBuilder,string,string,TConfiguration,System.Action_Microsoft.Extensions.DependencyInjection.IHttpClientBuilder,TConfiguration_).TConfiguration'></a>

`TConfiguration`

Typ konfigurace, který bude pro tohoto TClient vložen do Di
#### Parameters

<a name='CIS.Infrastructure.ExternalServicesHelpers.StartupRestExtensions.AddExternalServiceRestClient_TClient,TImplementation,TConfiguration_(thisMicrosoft.AspNetCore.Builder.WebApplicationBuilder,string,string,TConfiguration,System.Action_Microsoft.Extensions.DependencyInjection.IHttpClientBuilder,TConfiguration_).builder'></a>

`builder` [Microsoft.AspNetCore.Builder.WebApplicationBuilder](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.AspNetCore.Builder.WebApplicationBuilder 'Microsoft.AspNetCore.Builder.WebApplicationBuilder')

<a name='CIS.Infrastructure.ExternalServicesHelpers.StartupRestExtensions.AddExternalServiceRestClient_TClient,TImplementation,TConfiguration_(thisMicrosoft.AspNetCore.Builder.WebApplicationBuilder,string,string,TConfiguration,System.Action_Microsoft.Extensions.DependencyInjection.IHttpClientBuilder,TConfiguration_).serviceName'></a>

`serviceName` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Název konzumované služby třetí strany

<a name='CIS.Infrastructure.ExternalServicesHelpers.StartupRestExtensions.AddExternalServiceRestClient_TClient,TImplementation,TConfiguration_(thisMicrosoft.AspNetCore.Builder.WebApplicationBuilder,string,string,TConfiguration,System.Action_Microsoft.Extensions.DependencyInjection.IHttpClientBuilder,TConfiguration_).serviceImplementationVersion'></a>

`serviceImplementationVersion` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Verze proxy nad API třetí strany

<a name='CIS.Infrastructure.ExternalServicesHelpers.StartupRestExtensions.AddExternalServiceRestClient_TClient,TImplementation,TConfiguration_(thisMicrosoft.AspNetCore.Builder.WebApplicationBuilder,string,string,TConfiguration,System.Action_Microsoft.Extensions.DependencyInjection.IHttpClientBuilder,TConfiguration_).configuration'></a>

`configuration` [TConfiguration](CIS.Infrastructure.ExternalServicesHelpers.StartupRestExtensions.md#CIS.Infrastructure.ExternalServicesHelpers.StartupRestExtensions.AddExternalServiceRestClient_TClient,TImplementation,TConfiguration_(thisMicrosoft.AspNetCore.Builder.WebApplicationBuilder,string,string,TConfiguration,System.Action_Microsoft.Extensions.DependencyInjection.IHttpClientBuilder,TConfiguration_).TConfiguration 'CIS.Infrastructure.ExternalServicesHelpers.StartupRestExtensions.AddExternalServiceRestClient<TClient,TImplementation,TConfiguration>(this Microsoft.AspNetCore.Builder.WebApplicationBuilder, string, string, TConfiguration, System.Action<Microsoft.Extensions.DependencyInjection.IHttpClientBuilder,TConfiguration>).TConfiguration')

<a name='CIS.Infrastructure.ExternalServicesHelpers.StartupRestExtensions.AddExternalServiceRestClient_TClient,TImplementation,TConfiguration_(thisMicrosoft.AspNetCore.Builder.WebApplicationBuilder,string,string,TConfiguration,System.Action_Microsoft.Extensions.DependencyInjection.IHttpClientBuilder,TConfiguration_).additionalHandlersRegistration'></a>

`additionalHandlersRegistration` [System.Action&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Action-2 'System.Action`2')[Microsoft.Extensions.DependencyInjection.IHttpClientBuilder](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.DependencyInjection.IHttpClientBuilder 'Microsoft.Extensions.DependencyInjection.IHttpClientBuilder')[,](https://docs.microsoft.com/en-us/dotnet/api/System.Action-2 'System.Action`2')[TConfiguration](CIS.Infrastructure.ExternalServicesHelpers.StartupRestExtensions.md#CIS.Infrastructure.ExternalServicesHelpers.StartupRestExtensions.AddExternalServiceRestClient_TClient,TImplementation,TConfiguration_(thisMicrosoft.AspNetCore.Builder.WebApplicationBuilder,string,string,TConfiguration,System.Action_Microsoft.Extensions.DependencyInjection.IHttpClientBuilder,TConfiguration_).TConfiguration 'CIS.Infrastructure.ExternalServicesHelpers.StartupRestExtensions.AddExternalServiceRestClient<TClient,TImplementation,TConfiguration>(this Microsoft.AspNetCore.Builder.WebApplicationBuilder, string, string, TConfiguration, System.Action<Microsoft.Extensions.DependencyInjection.IHttpClientBuilder,TConfiguration>).TConfiguration')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Action-2 'System.Action`2')

Možnost zaregistrovat další HttpHandlery do pipeline.

#### Returns
[Microsoft.Extensions.DependencyInjection.IHttpClientBuilder](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.DependencyInjection.IHttpClientBuilder 'Microsoft.Extensions.DependencyInjection.IHttpClientBuilder')

#### Exceptions

[CIS.Core.Exceptions.CisConfigurationException](https://docs.microsoft.com/en-us/dotnet/api/CIS.Core.Exceptions.CisConfigurationException 'CIS.Core.Exceptions.CisConfigurationException')  
Chyba v konfiguraci služby - např. špatně zadaný typ implementace.

[CIS.Core.Exceptions.CisConfigurationNotFound](https://docs.microsoft.com/en-us/dotnet/api/CIS.Core.Exceptions.CisConfigurationNotFound 'CIS.Core.Exceptions.CisConfigurationNotFound')  
Konfigurace typu TConfiguration pro klíč ES:{serviceName}:{serviceImplementationVersion} nebyla nalezena v sekci ExternalServices v appsettings.json