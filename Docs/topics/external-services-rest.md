# Jak implementovat proxy projekt (REST služba)?
Zásadní pro implementaci REST služeb jsou dvě extension metody:
- `AddExternalServiceConfiguration()`, která načítá konfiguraci proxy z *appsettings.json* a vkládá ji do DI.
- `AddExternalServiceRestClient()`, která vytváří *HttpClient*-a pro volání RESTových služeb, registruje vybrané middleware/HttpHandler-y atd.

## Metoda AddExternalServiceRestClient()
```csharp
static IHttpClientBuilder AddExternalServiceRestClient<TClient, TImplementation>(
    this WebApplicationBuilder builder)
        where TClient : class, IExternalServiceClient
        where TImplementation : class, TClient
```
- **TClient** je interface proxy klienta - z příkladu víše je to `V1.IEasClient`. Tento interface musí vždy dědit z marker interface `CIS.Infrastructure.ExternalServicesHelpers.IExternalServiceClient`.
- **TImplementation** je implementace proxy klienta - z příkladu víše je to `V1.RealEasClient`.

**Flow akcí v AddExternalServiceRestClient:**
1. přidání nového typed *HttpClient* do *Services*
    * nastavení Polly policies requestu z konfigurace -> timeout, retry
    * nastavení `BaseAddress` HttpClient-a
    * nastavení autentizace (pokud je vyžadována)
2. pokud je v konfiguraci *IgnoreServerCertificateErrors=true*, přidání HttpHandleru který ignoruje SSL certificate chyby
3. pokud je v konfiguraci *UseLogging=true*, přidání HttpHandleru logujícího request/response (volitelně payload a hlavičky).

Metoda vrací instanci `IHttpClientBuilder`, takže je možné ji použít ve formě fluent syntaxe např. k přidání dalších HttpHandlerů.
Její volání tedy může v implementaci vypadat takto:
```csharp
builder
    .AddExternalServiceRestClient<V1.IEasClient, V1.RealEasClient>()
    .AddExternalServicesCorrelationIdForwarding()
    .AddExternalServicesErrorHandling(StartupExtensions.ServiceName);
```
Zde ze založí výchozí *HttpClient* a zároveň se do něj vloží middleware pro forwardování CorrelationId a middleware pro zachytávání vyjímek.

## Výchozí timeout / retry policy
Timeout HttpRequestu a retry policy nastavujeme pomocí **Polly**.
V konfiguraci je možné nastavit timeout requestu (*RequestTimeout*) v sekundách,
počet retry pokusů (*RequestRetryCount*) a timeout retry pokusů (*RequestRetryTimeout*) - tj. počet sekund mezi jednotlivými retry pokusy.

Výchozí nastavení je:
- RequestTimeout = 10s
- RequestRetryCount = 3
- RequestRetryTimeout = 10s

### Ukázka nastavení retry / timeout policy v konfiguraci služby
```json
"ExternalServices": {
    "SomeKbService": {
        "V1": {
            ...
            "RequestTimeout": 30, // timeout každého requestu nastav na 30s
            "RequestRetryCount": 2, // pokud první request timeoutuje, zkus ještě 2x opakovat
            "RequestRetryTimeout": 5 // mezi jednotlivými opakováními počkej 5s
        }
    }
}
```

## Připravené HttpHandlery
V `CIS.Infrastructure.ExternalServicesHelpers.HttpHandlers` jsou již připravené tyto *HttpHandler*-y, které je možné připojit do výchozí HttpClientFactory.

**BasicAuthenticationHttpHandler**  
Přidává Authorization HTTP header pro username a password v konfiguraci služby.
Přidává se do pipeline pokud v konfiguraci služby `Authentication`=Basic.

**CorrelationIdForwardingHttpHandler**  
Přidává do HTTP hlavičky correlation Id.

**ErrorHandlingHttpHandler**  
Middleware, který zachycuje standardní vyjímky a mění je na CIS exceptions.
Zároveň vyhodnocuje HTTP status kódy a pokud je StatusCode>=500, tak vyvolává `CisExternalServiceServerErrorException`.  
Extension metoda `IHttpClientBuilder.AddExternalServicesErrorHandling()`.

**KbHeadersHttpHandler**  
Přidává do HTTP hlavičky hodnoty vyžadované KB službami. X-B3-TraceId, X-B3-SpanId, X-KB-Caller-System-Identity.    
Extension metoda `IHttpClientBuilder.AddExternalServicesKbHeaders()`.

**KbPartyHeaderHttpHandler**  
Přidává do HTTP hlavičku s aktuálním uživatelem vyžadovanou KB službami. X-KB-Party-Identity-In-Service.
Pokud v kontextu requestu není znám uživatel, hlavička se neposílá.  
Extension metoda `IHttpClientBuilder.AddExternalServicesKbPartyHeaders()`.

Některé služby v KB hlavičku vyžadují, proto existuje ještě možnost fallback na hardcoded username v případě, že není znám přihlášený uživatel. 
Pro tento případ existuje metoda `AddExternalServicesKbPartyHeadersWithFallback(string username)`, která pošle hlavičku vždy.
Jako parametr `username` se většinou používá technický uživatel pod kterým je dané API KB volané.

**LoggingHttpHandler**  
Přidává logování request a response (volitelně payloadu a hlavičky).
Nastavení logování se řeší v konfiguraci *appsettings.json*.
```json
"ExternalServices": {
  "SomeKbService": {
    "V1": {
      ...
      "UseLogging": true,           // zapnutí a vypnutí HTTP logování
      "LogRequestPayload": true,    // logování payloadu a hlaviček requestu
      "LogResponsePayload": false // logování payloadu a hlaviček responsu
    }
  }
}
```

## Přidání vlastních HttpHandlerů
V případě potřeby je možné do registrační pipeline přidat i vlastní *HttpHandler* standardní cestou fluent builderu.

```csharp
builder
    .AddExternalServiceRestClient<LuxpiService.V1.ILuxpiServiceClient, LuxpiService.V1.RealLuxpiServiceClient>()
    // vložení vlastního handleru
    .AddHttpMessageHandler(services => new LuxpiService.TokenService.TokenHttpHandler(configuration.Password, services.GetRequiredService<LuxpiService.TokenService.ITokenService>()));
```

## Příklad implementace
Ukázka nastavení služby v `StartupExtensions.cs`
```csharp
public static class StartupExtensions
{
    internal const string ServiceName = "PreorderService";

    public static WebApplicationBuilder AddExternalService<TClient>(this WebApplicationBuilder builder)
        where TClient : class, PreorderService.V1.IPreorderServiceClient
        => builder.AddPreorderService<TClient>(PreorderService.V1.IPreorderServiceClient.Version);

    static WebApplicationBuilder AddPreorderService<TClient>(this WebApplicationBuilder builder, string version)
        where TClient : class, IExternalServiceClient
    {
        // ziskat konfigurace pro danou verzi sluzby
        var configuration = builder.AddExternalServiceConfiguration<TClient>(ServiceName, version);

        switch (version, configuration.ImplementationType)
        {
            case (PreorderService.V1.IPreorderServiceClient.Version, ServiceImplementationTypes.Mock):
                builder.Services.AddTransient<PreorderService.V1.IPreorderServiceClient, PreorderService.V1.MockPreorderServiceClient>();
                break;

            case (PreorderService.V1.IPreorderServiceClient.Version, ServiceImplementationTypes.Real):
                builder
                    .AddExternalServiceRestClient<PreorderService.V1.IPreorderServiceClient, PreorderService.V1.RealPreorderServiceClient>()
                    .AddExternalServicesKbHeaders()
                    .AddExternalServicesKbPartyHeaders()
                    .AddExternalServicesErrorHandling(StartupExtensions.ServiceName);
                break;

            default:
                throw new NotImplementedException($"{ServiceName} version {typeof(TClient)} client not implemented");
        }

        PreorderService.ErrorCodeMapper.Init();

        return builder;
    }
}
```

A následně registrace proxy projektu, např. v doménové službě:
```csharp
builder.AddExternalService<IPreorderService>();
```