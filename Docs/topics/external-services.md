# Vytváření proxy klientů nad službami třetích stran
Žádnou službu třetí strany (KB, MpHome...) nevoláme přímo, nad každou takovou službou vytvoříme proxy/wrapper, který má podobnou funkci jako `Clients` projekt u doménových služeb.
Jde o to, že nikde v kódu služby/aplikace nebudeme řešit jak vytvářet *HttpClient*, jak dělat logování request/response atd. - celá tato funkčnost bude zapouzdřena do proxy projektu, který pak přinese unifikované chování pro všechny konzumenty.

Proxy projekty vznikají pro Json služby (**REST**) i **SOAP** (WCF) služby. 
Konzument by neměl na interface proxy projektu poznat rozdíl mezi REST nebo SOAP službou, rozdílná je pouze vnitřní implementace, případně konfigurace.

Proxy projekty nad službami třetích stran vznikají na dvou místech:
- **Služba třetí strany, která může být volána z více projektů NOBY.**  
Projekt bude založen v adresáři */ExternalServices*, název projektu (namespace) je `ExternalServices.{nazev_sluzby_treti_strany}`.
Ve výsledku se jedná o samostatný NuGet balíček.
- **Služba třetí strany, která je vždy pevně spjata pouze s jedním projektem/službou NOBY.**  
Projekt bude založen v adresáři dané služby, v namespace `{název DS}.ExternalServices.{název proxy}`.

## Registrace proxy projektu v aplikaci konzumenta
Proxy projekt vystavuje vždy jeden public interface pro každou verzi implementace. Tento interface musí dědit z `CIS.Infrastructure.ExternalServicesHelpers.IExternalServiceClient`.  
Dále vystavuje extension metodu, kterou zajišťuje vložení proxy do DI konzumenta. Tato metoda má vždy stejnou signaturu:
```csharp
public static WebApplicationBuilder AddExternalService<TClient>(this WebApplicationBuilder builder)
    where TClient : class, IExternalServiceClient
```
TClient je interface proxy projektu pro danou verzi implementace. Tj. např.:
```csharp
builder.AddExternalService<IEasClient>();
```
Pokud je konzument správně nakonfigurován (tj. má správně sekci `ExternalServices` v *appsettings.json*), proxy projekt si sám dle aktuálního aplikačního prostředí zjistít adresu služby, na kterou se má připojovat (ze *ServiceDiscovery*).

## Konfigurace proxy projektu
Všechny proxy externích služeb mají společnou strukturu a umístění konfigurace. 
V *appsettings.json* souboru služby (aplikace), která proxy používá, existuje klíč `ExternalServices`, který obsahuje seznam objektů / konfigurací proxy projektů v této službě použitých.
C# reprezentace konfigurace je interface v namespace `CIS.Infrastructure.ExternalServicesHelpers.Configuration`.

Proxy projekt je v konfiguraci vždy ve dvou úrovních.
Na první je název proxy projektu, na druhé jsou jednotlivé verze implementace.
```json
    "nazev_projektu": {
        "verze_implementace_1": { ... },
        "verze_implementace_2": { ... }
    }
```

### Adresa služby třetí strany
Adresa / URL služby třetí strany může být zadaná přímo v konfiguračním souboru nebo může být načítána ze *ServiceDiscovery*.
Preferovanou variantou je *ServiceDiscovery*.

### Příklad konfigurace v appsettings.json
```json
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

**Kompletní definice konfiguračního objektu:**
```csharp
    /// <summary>
    /// Zapne logovani request a response payloadu a hlavicek. Default: true
    /// </summary>
    /// <remarks>Je v konfiguraci, aby bylo možné měnit nastavení na úrovni CI/CD.</remarks>
    public bool UseLogging { get; set; } = true;

    /// <summary>
    /// True = do logu se ulozi plny payload odpovedi externi sluzby
    /// </summary>
    public bool LogRequestPayload { get; set; } = true;

    /// <summary>
    /// True = do logu se ulozi plny request poslany do externi sluzby
    /// </summary>
    public bool LogResponsePayload { get; set; } = true;

    /// <summary>
    /// Default request timeout in seconds
    /// </summary>
    /// <remarks>Default is set to 10 seconds</remarks>
    public int? RequestTimeout { get; set; } = 10;

    /// <summary>
    /// Service URL when ServiceDiscovery is not being used. Use only when UseServiceDiscovery=false.
    /// </summary>
    public Uri? ServiceUrl { get; set; }

    /// <summary>
    /// If True, then library will try to obtain all needed service URL's from ServiceDiscovery.
    /// </summary>
    /// <remarks>Default is set to True</remarks>
    public bool UseServiceDiscovery { get; set; } = true;

    /// <summary>
    /// Pokud =true, ignoruje HttpClient problem s SSL certifikatem remote serveru.
    /// </summary>
    public bool IgnoreServerCertificateErrors { get; set; } = true;

    /// <summary>
    /// Type of http client implementation - can be mock or real client or something else.
    /// </summary>
    public ServiceImplementationTypes ImplementationType { get; set; } = ServiceImplementationTypes.Unknown;

    /// <summary>
    /// Typ pouzite autentizace na sluzbu treti strany
    /// </summary>
    public ExternalServicesAuthenticationTypes Authentication { get; set; } = ExternalServicesAuthenticationTypes.None;
```

## Adresářová struktura proxy projektu
Příklad pro proxy projekt **Eas**:
```
[Dto]
    GeneralIdRequest.cs     (adresář pro společné Dto objekty)
[V1]                        (verze implementace proxy klienta)
    [OpenApi]
        swagger.json        (REST - Swagger kontrakt služby)
        service.wsdl        (SOAP - wsdl popis služby)
    IEasClient.cs           (interface reprezentující proxy klienta v DI)
    RealEasClient.cs        (implementace proxy klienta)
    MockEasClient.cs        (mock implementace proxy klienta)
    ...                     (další soubory, DTO, nastavení NSwag atd.)
[V2]                        (další verze klienta)
    ...                     
StartupExtensions.cs        (extension metoda pro zapojení proxy projektu do DI konzumující aplikace)
```

## Jak implementovat proxy projekt - společná část pro REST a SOAP služby
Pro vlastní implementaci máme připravenou společnou infrastrukturu v projektu `CIS.Infrastructure.ExternalServicesHelpers`.

### Metoda AddExternalServiceConfiguration()
Tato metoda načte konfigurace pro příslušnou službu z *appsettings.json*, vloží instanci konfigurace do DI a vrátí ji jako výsledek.
Pokud je konfigurace nastavena na zjištění adresy služby ze *ServiceDiscovery*, vlastní proces zjištění URL nastává až později v pipeline WebApplicationBuilder-u.
```csharp
static IExternalServiceConfiguration<TClient> AddExternalServiceConfiguration<TClient>(
    this WebApplicationBuilder builder,
    string serviceName,
    string serviceImplementationVersion)
        where TClient : class, IExternalServiceClient
```
- **TClient** je interface proxy klienta - z příkladu víše je to `V1.IEasClient`. Tento interface musí vždy dědit z marker interface `CIS.Infrastructure.ExternalServicesHelpers.IExternalServiceClient`.
- **serviceName** je název služby, např. "Eas".
- **serviceImplementationVersion** je verze implementace, např. "V1".

## Implementace proxy projektů
[Jak implementovat proxy projekt (REST služba)?](./external-services-rest.md)

[Jak implementovat proxy projekt (SOAP služba)?](./external-services-soap.md)
