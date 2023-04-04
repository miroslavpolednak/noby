#### [CIS.Infrastructure.ExternalServicesHelpers](index.md 'index')
### [CIS.Infrastructure.ExternalServicesHelpers.HttpHandlers](CIS.Infrastructure.ExternalServicesHelpers.HttpHandlers.md 'CIS.Infrastructure.ExternalServicesHelpers.HttpHandlers')

## KbPartyHeaderHttpHandler Class

Middleware přidávájící KB hlavičku s informací o přihlášeném uživateli do requestu.

```csharp
public sealed class KbPartyHeaderHttpHandler : System.Net.Http.DelegatingHandler
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; [System.Net.Http.HttpMessageHandler](https://docs.microsoft.com/en-us/dotnet/api/System.Net.Http.HttpMessageHandler 'System.Net.Http.HttpMessageHandler') &#129106; [System.Net.Http.DelegatingHandler](https://docs.microsoft.com/en-us/dotnet/api/System.Net.Http.DelegatingHandler 'System.Net.Http.DelegatingHandler') &#129106; KbPartyHeaderHttpHandler

### Remarks
Přidává hlavičku X-KB-Party-Identity-In-Service.