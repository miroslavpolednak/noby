#### [CIS.Infrastructure.ExternalServicesHelpers](index.md 'index')
### [CIS.Infrastructure.ExternalServicesHelpers.HttpHandlers](CIS.Infrastructure.ExternalServicesHelpers.HttpHandlers.md 'CIS.Infrastructure.ExternalServicesHelpers.HttpHandlers')

## LoggingHttpHandler Class

Middleware pro logování payloadu a hlavičke requestu a responsu.

```csharp
public sealed class LoggingHttpHandler : System.Net.Http.DelegatingHandler
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; [System.Net.Http.HttpMessageHandler](https://docs.microsoft.com/en-us/dotnet/api/System.Net.Http.HttpMessageHandler 'System.Net.Http.HttpMessageHandler') &#129106; [System.Net.Http.DelegatingHandler](https://docs.microsoft.com/en-us/dotnet/api/System.Net.Http.DelegatingHandler 'System.Net.Http.DelegatingHandler') &#129106; LoggingHttpHandler

### Remarks
Vloží do kontextu logovaného záznamu klíče Payload a Headers s odpovídajícími objekty. Pokud např. response payload neobsahuje, není tento klíč do kontextu logovaného záznamu vložen.