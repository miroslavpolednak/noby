#### [CIS.Infrastructure.ExternalServicesHelpers](index.md 'index')
### [CIS.Infrastructure.ExternalServicesHelpers.HttpHandlers](CIS.Infrastructure.ExternalServicesHelpers.HttpHandlers.md 'CIS.Infrastructure.ExternalServicesHelpers.HttpHandlers')

## BasicAuthenticationHttpHandler Class

Middleware pridavajici Authorization header do requestu. Username a password je zadavan do konstruktoru handleru pri pridavani HttpClienta.

```csharp
public sealed class BasicAuthenticationHttpHandler : System.Net.Http.DelegatingHandler
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; [System.Net.Http.HttpMessageHandler](https://docs.microsoft.com/en-us/dotnet/api/System.Net.Http.HttpMessageHandler 'System.Net.Http.HttpMessageHandler') &#129106; [System.Net.Http.DelegatingHandler](https://docs.microsoft.com/en-us/dotnet/api/System.Net.Http.DelegatingHandler 'System.Net.Http.DelegatingHandler') &#129106; BasicAuthenticationHttpHandler