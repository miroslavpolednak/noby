# Implementace soap klienta / integrace na soap službu

Při integraci na soap službu se vždy snažíme klienta vygenerovat z wsdl souboru nebo z adresy, kde je soap služba hostovaná, pokud to z nějakého důvodu není možné (nevalidní wsdl atd.), tak lze použít http klienta a soap request(y) si skládat ručně, toto ale není doporučovaný způsob.

Následný postup implementace soap klienta je pouze pro generované klienty z wsdl/url

## Generovaní soap klienta z wsdl/url
 Pro vygenerování klienta použijeme nástroj [dotnet-svcutil](https://learn.microsoft.com/en-us/dotnet/core/additional-tools/dotnet-svcutil-guide?tabs=dotnetsvcutil2x) 

## Implementace Soap klienta

Pro unifikaci implementace soap klienta vznikla base třída:  

```csharp
namespace CIS.Infrastructure.ExternalServicesHelpers.BaseClasses;
public abstract class SoapClientBase<TSoapClient, TSoapClientChannel> : IDisposable
    where TSoapClient : ClientBase<TSoapClientChannel>, new()
    where TSoapClientChannel : class
{
     public SoapClientBase(
        IExternalServiceConfiguration configuration,
        ILogger logger)
    {
        ...
    }
    ...
}
 ```
kde tato base třída zajistí:

- Vytvoření klienta dle generických parametrů a konfigurace
- Postará se o správný dispose soap klienta
- U potomků vynutí konfiguraci soap Binding
- Zajistí logování soap requestů a responsů dle konfigurace
- Dle konfigurace nastaví basic authentication
- Lze vypnout validaci serverového certifikátu pomocí konfigurace    

**Konfigurace všech externích služeb by měla vycházet ze základní sady konfiguračních parametrů a u soap služeb tomu není jinak**

```csharp
/// <summary>
/// Základní konfigurace externí služby (služby třetí strany).
/// </summary>
/// <remarks>Pro registraci HTTP klienta by se vždy měla používat generická verze interface.</remarks>
public interface IExternalServiceConfiguration
    : CIS.Core.IIsServiceDiscoverable
{
    /// <summary>
    /// Zapne logovani request a response payloadu a hlavicek. Default: true
    /// </summary>
    /// <remarks>Je v konfiguraci, aby bylo možné měnit nastavení na úrovni CI/CD.</remarks>
    bool UseLogging { get; set; }

    /// <summary>
    /// True = do logu se ulozi plny payload odpovedi externi sluzby
    /// </summary>
    bool LogRequestPayload { get; set; }

    /// <summary>
    /// True = do logu se ulozi plny request poslany do externi sluzby
    /// </summary>
    bool LogResponsePayload { get; set; }

    /// <summary>
    /// Default request timeout in seconds
    /// </summary>
    /// <remarks>Default is set to 10 seconds</remarks>
    int? RequestTimeout { get; set; }

    /// <summary>
    /// Pokud =true, ignoruje HttpClient problem s SSL certifikatem remote serveru.
    /// </summary>
    bool IgnoreServerCertificateErrors { get; set; }

    /// <summary>
    /// Type of http client implementation - can be mock or real client or something else.
    /// </summary>
    ServiceImplementationTypes ImplementationType { get; set; }

    /// <summary>
    /// Typ pouzite autentizace na sluzbu treti strany
    /// </summary>
    ExternalServicesAuthenticationTypes Authentication { get; set; }

    /// <summary>
    /// Autentizace - Username
    /// </summary>
    string? Username { get; set; }

    /// <summary>
    /// Autentizace - Heslo
    /// </summary>
    string? Password { get; set; }
}

/// <summary>
/// Generická verze konfigurace.
/// </summary>
/// <typeparam name="TClient">Typ HTTP klienta</typeparam>
public interface IExternalServiceConfiguration<TClient>
    : IExternalServiceConfiguration
    where TClient : class, IExternalServiceClient
{ }
 ```
## Implementace bázové třídy (SoapClientBase<TSoapClient, TSoapClientChannel>)

Aby jsme mohli implementovat SoapClientBase musíme dohledat typ klienta a typ channelu ve vegenerované třídě z wsdl/url, každý vygenerovaný klient má následující signaturu

```csharp
public partial class MyCustomClient : System.ServiceModel.ClientBase<IMyCustomChannel> IMyCustomChannel
// Tedy MyCustomClient = TSoapClient a IMyCustomChannel = TSoapClientChannel 
```
a pak už jen stačí implementovat nějakého specifického klienta 

```csharp
internal class ExampleClient : SoapClientBase<MyCustomClient, IMyCustomChannel>
{
    public ExampleClient(
            ILogger<RealSdfClient> logger,
            IExternalServiceConfiguration<ISdfClient> configuration)
            : base(configuration, logger)
    {
       ...
    }

    protected override string ServiceName => "SomeServiceName";

    public async Task<FindDocumentsOutput> FindDocuments(FindSdfDocumentsQuery query, CancellationToken cancellationToken)
    {
         // Client pochází z bázové třídy
         return await Client.FindDocumentsAsync(user, searchQuery, options)
    }

    protected override Binding CreateBinding()
    {
        var binding = new BasicHttpBinding(BasicHttpSecurityMode.Transport);
        binding.MessageEncoding = WSMessageEncoding.Mtom;
        binding.MaxBufferSize = 2147483647;
        binding.MaxReceivedMessageSize = 2147483647;
        // Configuration pochází z bázové třídy
        binding.SendTimeout = TimeSpan.FromSeconds(Configuration.RequestTimeout!.Value!);
        binding.ReceiveTimeout = TimeSpan.FromSeconds(Configuration.RequestTimeout!.Value!);
        return binding;
    }
}
```
Pokud se budete chtít inspirovat v nějaké existující implementaci, tak se lze podívat např. na:

- ExternalServices.Eas
- ExternalServices.EasSimulationHT
- DomainServices.DocumentArchiveService.ExternalServices.Sdf 
