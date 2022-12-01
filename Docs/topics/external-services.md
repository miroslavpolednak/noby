# Vytváření proxy klientů nad službami třetích stran
Žádnou službu třetí strany (KB, MpHome...), nad každou takovou službou vytvoříme proxy/wrapper, který má podobnou funkci jako `Clients` projekt u doménových služeb.
Jde o to, že nikde v kódu služby/aplikace nebudeme řešit jak vytvářet *HttpClient*, jak dělat logování request/response atd. - celá tato funkčnost bude zapouzdřena do proxy projektu.

Proxy projekty vznikají pro Json služby (REST) i SOAP (WCF) služby. 
Konzument by neměl na interface proxy projektu poznat rozdíl mezi REST nebo SOAP službou, rozdílná je pouze vnitřní implementace.

Proxy projekty nad službami třetích stran vznikají na dvou místech:
- **služba třetí strany, která může být volána z více projektů NOBY.**  
Projekt bude založen v adresáři */ExternalServices*, název projektu (namespace) je `ExternalServices.{nazev_sluzby_treti_strany}`.
- **služba třetí strany, která je vždy pevně spjata pouze s jedním projektem/službou NOBY.** 
Nezakládáme pro službu nový projekt, ale proxy klient bude součástí projektu ----- nebo ano???? TODO domyslet

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
V *appsettings.json* souboru služby (aplikace), která proxy používá, existuje klíč **ExternalServices*, který obsahuje seznam objektů / konfigurací proxy projektů v této službě použitých.
C# reprezentace konfigurace jsou interface v namespace `CIS.Infrastructure.ExternalServicesHelpers.Configuration`.

Klíč proxy projektu v konfiguračním souboru může mít dva tvary:
- `{nazev_proxy_projektu}` v tomto případě je konfigurace platná pro jakoukoliv verzi implementace proxy.
- `{nazev_proxy_projektu}:{verze_implementace}` v tomto případě je konfigurace platná pouze pro specifikovanou verzi implementace proxy.

### Adresa služby třetí strany
Adresa / URL služby třetí strany může být zadaná přímo v konfiguračním souboru nebo může být načítána ze *ServiceDiscovery*.
Preferovanou variantou je *ServiceDiscovery*.

### Příklad konfigurace v appsettings.json
```
"ExternalServices": {
    "AddressWhisperer:V1": {
        "ImplementationType": "Real",
        "UseServiceDiscovery": true,
        "Username": "my_user",
        "Password": "password"
    },
    "Eas": {
        "ImplementationType": "Real",
        "UseServiceDiscovery": true
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
public static TConfiguration AddExternalServiceConfiguration<TClient, TConfiguration>(
    this WebApplicationBuilder builder,
    string serviceName,
    string serviceImplementationVersion)
        where TClient : class, IExternalServiceClient
        where TConfiguration : class, IExternalServiceConfiguration<TClient>
```
- **TClient** je interface proxy klienta - z příkladu víše je to `V1.IEasClient`. Tento interface musí vždy dědit z marker interface `CIS.Infrastructure.ExternalServicesHelpers.IExternalServiceClient`.
- **TConfiguration** typ konfigurace platný pro danou registraci, odvozený z `CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration`.
- **serviceName** je název služby, např. "Eas".
- **serviceImplementationVersion** je verze implementace, např. "V1".

### AddExternalServiceRestClient
```
public static IHttpClientBuilder AddExternalServiceRestClient<TClient, TImplementation, TConfiguration>(
    this WebApplicationBuilder builder,  
    string serviceImplementationVersion,
    IExternalServiceConfiguration configuration,
    Action<IHttpClientBuilder, IExternalServiceConfiguration>? additionalHandlersRegistration = null)
        where TClient : class, IExternalServiceClient
        where TImplementation : class, TClient
        where TConfiguration : class, IExternalServiceConfiguration<TClient>
```
- **TClient** je interface proxy klienta - z příkladu víše je to `V1.IEasClient`. Tento interface musí vždy dědit z marker interface `CIS.Infrastructure.ExternalServicesHelpers.IExternalServiceClient`.
- **TImplementation** je implementace proxy klienta - z příkladu víše je to `V1.RealEasClient`.
- **TConfiguration** typ konfigurace platný pro danou registraci, odvozený z `CIS.Infrastructure.ExternalServicesHelpers.Configuration.IExternalServiceConfiguration`.
- **serviceName** je název služby, např. "Eas".
- **serviceImplementationVersion** je verze implementace, např. "V1".
- **additionalHandlersRegistration** je callback s možností přidat vlastní *HttpHandlery* do pipeline daného HttpClient-a.

### Flow akcí v AddExternalServiceRestClient
1. přidání nového typed *HttpClient* do *Services*
    * zjištění URL služby pokud se má dočítat z *ServiceDiscovery*
    * nastavení `Timeout` requestu z konfigurace
    * nastavení `BaseAddress` HttpClient-a
2. pokud je v konfiguraci *IgnoreServerCertificateErrors=true*, přidání HttpHandleru který ignoruje SSL certificate chyby
3. spuštění callbacku `additionalHandlersRegistration`
    * v rámci callbacku je možné přidat další *HttpHandler-y*
4. pokud je v konfiguraci *LogPayloads=true*, přidání HttpHandleru který loguje request / response HttpClienta

### Připravené HttpHandlery
V `CIS.Infrastructure.ExternalServicesHelpers` jsou již připravené tyto *HttpHandler*-y.

**BasicAuthenticationHttpHandler**  
Přidává Authorization HTTP header pro username a password v konfiguraci služby.
Nastavuje se automaticky, pokud `TConfiguration` is `IExternalServiceBasicAuthenticationConfiguration`.

**CorrelationIdForwardingHttpHandler**  
Přidává do HTTP hlavičky correlation Id.

**ErrorHandlingHttpHandler**  
Middleware, který zachycuje standardní vyjímky a mění je na CIS exceptions.  
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
        var configuration = builder.AddExternalServiceConfiguration<TClient, ExternalServiceConfiguration<TClient>>(ServiceName, version);

        switch (version, configuration.ImplementationType)
        {
            case (V1.IEasClient.Version, ServiceImplementationTypes.Mock):
                builder.Services.AddTransient<V1.IEasClient, V1.MockEasClient>();
                break;

            case (V1.IEasClient.Version, ServiceImplementationTypes.Real):
                builder.AddExternalServiceRestClient<V1.IEasClient, V1.RealEasClient, ExternalServiceConfiguration<V1.IEasClient>>(V1.IEasClient.Version, configuration, _addAdditionalHttpHandlers);
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

    private static Action<IHttpClientBuilder, IExternalServiceConfiguration> _addAdditionalHttpHandlers = (builder, configuration)
        => builder
            .AddExternalServicesCorrelationIdForwarding()
            .AddExternalServicesErrorHandling(StartupExtensions.ServiceName);
}
```

A následně registrace proxy projektu, např. v doménové službě:
```
builder.AddExternalService<IEasClient>();
```

## Jak implementovat proxy projekt (SOAP služba)?
TODO