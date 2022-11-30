#### [CIS.Infrastructure.ExternalServicesHelpers](index.md 'index')
### [CIS.Infrastructure.ExternalServicesHelpers](CIS.Infrastructure.ExternalServicesHelpers.md 'CIS.Infrastructure.ExternalServicesHelpers')

## StartupRestExtensions Class

```csharp
public static class StartupRestExtensions
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; StartupRestExtensions
### Methods

<a name='CIS.Infrastructure.ExternalServicesHelpers.StartupRestExtensions.AddExternalServiceRestClient_TClient,TImplementation,TConfiguration_(thisMicrosoft.AspNetCore.Builder.WebApplicationBuilder,string,CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration,System.Action_Microsoft.Extensions.DependencyInjection.IHttpClientBuilder,CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration_)'></a>

## StartupRestExtensions.AddExternalServiceRestClient<TClient,TImplementation,TConfiguration>(this WebApplicationBuilder, string, IExternalServiceConfiguration, Action<IHttpClientBuilder,IExternalServiceConfiguration>) Method

Založení typed HttpClienta pro implementaci ExternalService.

```csharp
public static Microsoft.Extensions.DependencyInjection.IHttpClientBuilder AddExternalServiceRestClient<TClient,TImplementation,TConfiguration>(this Microsoft.AspNetCore.Builder.WebApplicationBuilder builder, string serviceImplementationVersion, CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration configuration, System.Action<Microsoft.Extensions.DependencyInjection.IHttpClientBuilder,CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration>? additionalHandlersRegistration=null)
    where TClient : class, CIS.Infrastructure.ExternalServicesHelpers.IExternalServiceClient
    where TImplementation : class, TClient
    where TConfiguration : class, CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration<TClient>;
```
#### Type parameters

<a name='CIS.Infrastructure.ExternalServicesHelpers.StartupRestExtensions.AddExternalServiceRestClient_TClient,TImplementation,TConfiguration_(thisMicrosoft.AspNetCore.Builder.WebApplicationBuilder,string,CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration,System.Action_Microsoft.Extensions.DependencyInjection.IHttpClientBuilder,CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration_).TClient'></a>

`TClient`

Typ klienta - interface pro danou verzi proxy nad API třetí strany

<a name='CIS.Infrastructure.ExternalServicesHelpers.StartupRestExtensions.AddExternalServiceRestClient_TClient,TImplementation,TConfiguration_(thisMicrosoft.AspNetCore.Builder.WebApplicationBuilder,string,CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration,System.Action_Microsoft.Extensions.DependencyInjection.IHttpClientBuilder,CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration_).TImplementation'></a>

`TImplementation`

Interní implementace TClient interface

<a name='CIS.Infrastructure.ExternalServicesHelpers.StartupRestExtensions.AddExternalServiceRestClient_TClient,TImplementation,TConfiguration_(thisMicrosoft.AspNetCore.Builder.WebApplicationBuilder,string,CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration,System.Action_Microsoft.Extensions.DependencyInjection.IHttpClientBuilder,CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration_).TConfiguration'></a>

`TConfiguration`

Typ konfigurace, který bude pro tohoto TClient vložen do Di
#### Parameters

<a name='CIS.Infrastructure.ExternalServicesHelpers.StartupRestExtensions.AddExternalServiceRestClient_TClient,TImplementation,TConfiguration_(thisMicrosoft.AspNetCore.Builder.WebApplicationBuilder,string,CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration,System.Action_Microsoft.Extensions.DependencyInjection.IHttpClientBuilder,CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration_).builder'></a>

`builder` [Microsoft.AspNetCore.Builder.WebApplicationBuilder](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.AspNetCore.Builder.WebApplicationBuilder 'Microsoft.AspNetCore.Builder.WebApplicationBuilder')

<a name='CIS.Infrastructure.ExternalServicesHelpers.StartupRestExtensions.AddExternalServiceRestClient_TClient,TImplementation,TConfiguration_(thisMicrosoft.AspNetCore.Builder.WebApplicationBuilder,string,CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration,System.Action_Microsoft.Extensions.DependencyInjection.IHttpClientBuilder,CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration_).serviceImplementationVersion'></a>

`serviceImplementationVersion` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Verze proxy nad API třetí strany

<a name='CIS.Infrastructure.ExternalServicesHelpers.StartupRestExtensions.AddExternalServiceRestClient_TClient,TImplementation,TConfiguration_(thisMicrosoft.AspNetCore.Builder.WebApplicationBuilder,string,CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration,System.Action_Microsoft.Extensions.DependencyInjection.IHttpClientBuilder,CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration_).configuration'></a>

`configuration` [IExternalServiceConfiguration](CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration.md 'CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration')

<a name='CIS.Infrastructure.ExternalServicesHelpers.StartupRestExtensions.AddExternalServiceRestClient_TClient,TImplementation,TConfiguration_(thisMicrosoft.AspNetCore.Builder.WebApplicationBuilder,string,CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration,System.Action_Microsoft.Extensions.DependencyInjection.IHttpClientBuilder,CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration_).additionalHandlersRegistration'></a>

`additionalHandlersRegistration` [System.Action&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.Action-2 'System.Action`2')[Microsoft.Extensions.DependencyInjection.IHttpClientBuilder](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.DependencyInjection.IHttpClientBuilder 'Microsoft.Extensions.DependencyInjection.IHttpClientBuilder')[,](https://docs.microsoft.com/en-us/dotnet/api/System.Action-2 'System.Action`2')[IExternalServiceConfiguration](CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration.md 'CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.Action-2 'System.Action`2')

Možnost zaregistrovat další HttpHandlery do pipeline.

#### Returns
[Microsoft.Extensions.DependencyInjection.IHttpClientBuilder](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.DependencyInjection.IHttpClientBuilder 'Microsoft.Extensions.DependencyInjection.IHttpClientBuilder')