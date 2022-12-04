# Vytváření proxy klientů nad službami třetích stran
Žádnou službu třetí strany (KB, MpHome...), nad každou takovou službou vytvoříme proxy/wrapper, který má podobnou funkci jako `Clients` projekt u doménových služeb.
Jde o to, že nikde v kódu služby/aplikace nebudeme řešit jak vytvářet *HttpClient*, jak dělat logování request/response atd. - celá tato funkčnost bude zapouzdřena do proxy projektu.

Proxy projekty vznikají pro Json služby (REST) i SOAP (WCF) služby. 
Konzument by neměl na interface proxy projektu poznat rozdíl mezi REST nebo SOAP službou, rozdílná je pouze vnitřní implementace.

Proxy projekty nad službami třetích stran vznikají na dvou místech:
- **služba třetí strany, která může být volána z více projektů NOBY.**  
Projekt bude založen v adresáři */ExternalServices*, název projektu (namespace) je `ExternalServices.{nazev_sluzby_treti_strany}`.
- **služba třetí strany, která je vždy pevně spjata pouze s jedním projektem/službou NOBY.**  
Projekt bude založen v adresáři dané služby, v namespace `{název DS}.ExternalServices.{název proxy}`.

## Registrace proxy projektu v aplikaci konzumenta
Proxy projekt vystavuje vždy jeden public interface pro každou verzi implementace. Tento interface musí dědit z `CIS.Infrastructure.ExternalServicesHelpers.IExternalServiceClient`.  
Dále vystavuje extension metodu, kterou zajišťuje vložení proxy do DI konzumenta. Tato metoda má vždy stejnou signaturu:
```
public static WebApplicationBuilder AddExternalService<TClient>(this WebApplicationBuilder builder)
    where TClient : class, IExternalServiceClient
```
TClient je interface proxy projektu pro danou verzi implementace. Tj. např.:
```
builder.AddExternalService<IEasClient>();
```

## Konfigurace proxy projektu
Všechny proxy externích služeb mají společnou strukturu a umístění konfigurace. 
V *appsettings.json* souboru služby (aplikace), která proxy používá, existuje klíč `ExternalServices`, který obsahuje seznam objektů / konfigurací proxy projektů v této službě použitých.
C# reprezentace konfigurace jsou interface v namespace `CIS.Infrastructure.ExternalServicesHelpers.Configuration`.

Proxy projekt je v konfiguraci vždy ve dvou úrovních.
Na první je název proxy projektu, na druhé jsou jednotlivé verze implementace.
```
    "nazev_projektu": {
        "verze_implementace_1": { ... },
        "verze_implementace_2": { ... }
    }
```

### Adresa služby třetí strany
Adresa / URL služby třetí strany může být zadaná přímo v konfiguračním souboru nebo může být načítána ze *ServiceDiscovery*.
Preferovanou variantou je *ServiceDiscovery*.

### Příklad konfigurace v appsettings.json
```
"ExternalServices": {
    "AddressWhisperer": {
        "V1": {
            "ImplementationType": "Real",
            "UseServiceDiscovery": true,
            "Authentication": "Basic",
            "Username": "my_user",
            "Password": "password"
        }
    },
    "Eas": {
        "V1": {
            "ImplementationType": "Real",
            "UseServiceDiscovery": true
        }
    }
}
```

## Adresářová struktura proxy projektu
Příklad pro proxy projekt **Eas**:
```
[Dto]
    GeneralIdRequest.cs     (adresář pro společné Dto objekty)
[V1]                        (verze implementace proxy klienta)
    IEasClient.cs           (interface reprezentující proxy klienta v DI)
    RealEasClient.cs        (implementace proxy klienta)
    MockEasClient.cs        (mock implementace proxy klienta)
    swagger.json            (REST - Swagger kontrakt služby)
    service.wsdl            (SOAP - wsdl popis služby)
    ...                     (další soubory, DTO, nastavení NSwag atd.)
[V2]                        (další verze klienta)
    ...                     
StartupExtensions.cs        (extension metoda pro zapojení proxy projektu do DI konzumující aplikace)
```

## Jak implementovat proxy projekt (REST služba)?
Pro vlastní implementaci máme připravenou společnou infrastrukturu v projektu `CIS.Infrastructure.ExternalServicesHelpers`.

Zásadní pro implementaci jsou dvě extension metody:
- `AddExternalServiceConfiguration()`, která načítá konfiguraci proxy z *appsettings.json* a vkládá ji do DI.
- `AddExternalServiceRestClient()`, která vytváří *HttpClient*-a, registruje vybrané middleware/HttpHandler-y atd.

### AddExternalServiceConfiguration
```
public static IExternalServiceConfiguration<TClient> AddExternalServiceConfiguration<TClient>(
    this WebApplicationBuilder builder,
    string serviceName,
    string serviceImplementationVersion)
        where TClient : class, IExternalServiceClient
```
- **TClient** je interface proxy klienta - z příkladu víše je to `V1.IEasClient`. Tento interface musí vždy dědit z marker interface `CIS.Infrastructure.ExternalServicesHelpers.IExternalServiceClient`.
- **serviceName** je název služby, např. "Eas".
- **serviceImplementationVersion** je verze implementace, např. "V1".

### AddExternalServiceRestClient
```
public static IHttpClientBuilder AddExternalServiceRestClient<TClient, TImplementation>(
    this WebApplicationBuilder builder)
        where TClient : class, IExternalServiceClient
        where TImplementation : class, TClient
```
- **TClient** je interface proxy klienta - z příkladu víše je to `V1.IEasClient`. Tento interface musí vždy dědit z marker interface `CIS.Infrastructure.ExternalServicesHelpers.IExternalServiceClient`.
- **TImplementation** je implementace proxy klienta - z příkladu víše je to `V1.RealEasClient`.

### Flow akcí v AddExternalServiceRestClient
1. přidání nového typed *HttpClient* do *Services*
    * nastavení `Timeout` requestu z konfigurace
    * nastavení `BaseAddress` HttpClient-a
    * nastavení autentizace (pokud je vyžadována)
2. pokud je v konfiguraci *IgnoreServerCertificateErrors=true*, přidání HttpHandleru který ignoruje SSL certificate chyby

### Připravené HttpHandlery
V `CIS.Infrastructure.ExternalServicesHelpers.HttpHandlers` jsou již připravené tyto *HttpHandler*-y.

**BasicAuthenticationHttpHandler**  
Přidává Authorization HTTP header pro username a password v konfiguraci služby.
Přidává se do pipeline pokud v konfiguraci služby `Authentication`=Basic.

**CorrelationIdForwardingHttpHandler**  
Přidává do HTTP hlavičky correlation Id.

**ErrorHandlingHttpHandler**  
Middleware, který zachycuje standardní vyjímky a mění je na CIS exceptions.
Zároveň vyhodnocuje HTTP status kódy a pokud je StatusCode>=500, tak vyvolává `CisServiceServerErrorException`.  
Extension metoda `IHttpClientBuilder.AddExternalServicesErrorHandling()`.

**KbHeadersHttpHandler**  
Přidává do HTTP hlavičky hodnoty vyžadované KB službami. X-B3-TraceId, X-B3-SpanId, X-KB-Caller-System-Identity.    
Extension metoda `IHttpClientBuilder.AddExternalServicesKbHeaders()`.

**LoggingHttpHandler**  
Přidává logování request a response payloadu a hlavičky.

### Příklad implementace
Ukázka nastavení služby v `StartupExtensions.cs`
```
public static class StartupExtensions
{
    internal const string ServiceName = "Eas";

    public static WebApplicationBuilder AddExternalService<TClient>(this WebApplicationBuilder builder)
        where TClient : class, IExternalServiceClient
    {
        // ziskat konfigurace pro danou verzi sluzby
        string version = getVersion<TClient>();
        var configuration = builder.AddExternalServiceConfiguration<TClient>(ServiceName, version);

        switch (version, configuration.ImplementationType)
        {
            case (V1.IEasClient.Version, ServiceImplementationTypes.Mock):
                builder.Services.AddTransient<V1.IEasClient, V1.MockEasClient>();
                break;

            case (V1.IEasClient.Version, ServiceImplementationTypes.Real):
                builder
                    .AddExternalServiceRestClient<V1.IEasClient, V1.RealEasClient>()
                    .AddExternalServicesCorrelationIdForwarding()
                    .AddExternalServicesErrorHandling(StartupExtensions.ServiceName);
                break;

            default:
                throw new NotImplementedException($"{ServiceName} version {typeof(TClient)} client not implemented");
        }

        return builder;
    }

    static string getVersion<TClient>()
        => typeof(TClient) switch
        {
            Type t when t.IsAssignableFrom(typeof(V1.IEasClient)) => V1.IEasClient.Version,
            _ => throw new NotImplementedException($"Unknown implmenetation {typeof(TClient)}")
        };
}
```

A následně registrace proxy projektu, např. v doménové službě:
```
builder.AddExternalService<IEasClient>();
```

## Jak implementovat proxy projekt (SOAP služba)?
TODO