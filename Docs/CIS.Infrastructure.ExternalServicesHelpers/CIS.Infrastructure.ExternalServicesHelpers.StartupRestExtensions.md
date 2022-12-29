#### [CIS.Infrastructure.ExternalServicesHelpers](index.md 'index')
### [CIS.Infrastructure.ExternalServicesHelpers](CIS.Infrastructure.ExternalServicesHelpers.md 'CIS.Infrastructure.ExternalServicesHelpers')

## StartupRestExtensions Class

```csharp
public static class StartupRestExtensions
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; StartupRestExtensions
### Methods

<a name='CIS.Infrastructure.ExternalServicesHelpers.StartupRestExtensions.AddExternalServiceRestClient_TClient,TImplementation_(thisMicrosoft.AspNetCore.Builder.WebApplicationBuilder)'></a>

## StartupRestExtensions.AddExternalServiceRestClient<TClient,TImplementation>(this WebApplicationBuilder) Method

Založení typed HttpClienta pro implementaci ExternalService.

```csharp
public static Microsoft.Extensions.DependencyInjection.IHttpClientBuilder AddExternalServiceRestClient<TClient,TImplementation>(this Microsoft.AspNetCore.Builder.WebApplicationBuilder builder)
    where TClient : class, CIS.Infrastructure.ExternalServicesHelpers.IExternalServiceClient
    where TImplementation : class, TClient;
```
#### Type parameters

<a name='CIS.Infrastructure.ExternalServicesHelpers.StartupRestExtensions.AddExternalServiceRestClient_TClient,TImplementation_(thisMicrosoft.AspNetCore.Builder.WebApplicationBuilder).TClient'></a>

`TClient`

Typ klienta - interface pro danou verzi proxy nad API třetí strany

<a name='CIS.Infrastructure.ExternalServicesHelpers.StartupRestExtensions.AddExternalServiceRestClient_TClient,TImplementation_(thisMicrosoft.AspNetCore.Builder.WebApplicationBuilder).TImplementation'></a>

`TImplementation`

Interní implementace TClient interface
#### Parameters

<a name='CIS.Infrastructure.ExternalServicesHelpers.StartupRestExtensions.AddExternalServiceRestClient_TClient,TImplementation_(thisMicrosoft.AspNetCore.Builder.WebApplicationBuilder).builder'></a>

`builder` [Microsoft.AspNetCore.Builder.WebApplicationBuilder](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.AspNetCore.Builder.WebApplicationBuilder 'Microsoft.AspNetCore.Builder.WebApplicationBuilder')

#### Returns
[Microsoft.Extensions.DependencyInjection.IHttpClientBuilder](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.DependencyInjection.IHttpClientBuilder 'Microsoft.Extensions.DependencyInjection.IHttpClientBuilder')

### Remarks
Některé HttpHandlery jsou vkládané pomocí konfigurace - to je proto, že potřebujeme na úrovni CI/CD řešit, zda budou v pipeline nebo ne.