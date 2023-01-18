# gRPC služby - Doménové služby a Infrastrukturní služby
gRPC služby jsou provozovány formou **Windows Service**.
Je to proto, že staré Windows servery neumí plný rozsah HTTP2 (hlavně Trailers), takže služby není možné provozovat standardně v IIS serveru.
Některé gRPC služby mohou poskytovat zároveň REST rozhraní pomocí *gRPC Transcoding*.

## Nastavení gRPC služby
Pro korektní spuštění služby je nutné správně nastavit konfiguraci *Kestrelu*. Pro to slouží zvláštní konfigurační soubor (**kestrel.json**).

**kestrel.json**  
Jedná se o JSON soubor v rootu aplikace. Struktura konfigurace je obrazem této třídy `CIS.Core.Configuration.KestrelConfiguration`.
Používáme vlastní konfigurační soubor, protože výchozí možnosti konfigurace *Kestrelu* v .NETu neumožňují vše, co jsme potřebovali - zejména nastavení SSL certifikátu.  

Ukázka konfigurace:
```json
{
  "CustomKestrel": {
    "Endpoints": [
      {
        "Port": 31001,
        "Protocol": 2
      }
    ],
    "Certificate": {
      "Location": "CertStore",
      "CertStoreName": "My",
      "CertStoreLocation": "LocalMachine",
      "Thumbprint": "2694C47172A2BB49985259915B747C2A2B3B8C1F"
    }
  }
}
```

Načtení konfigurace ze souboru a nastavení Kestrelu v apliaci se dělá pomocí extension metody během startupu aplikace:
```csharp
builder.UseKestrelWithCustomConfiguration();
```

## Registrace jako Windows service
```
sc.exe create FAT-ServiceDiscovery binPath= "d:\sluzby\discoveryservice\CIS.InternalServices.ServiceDiscovery.Api.exe winsvc"
```

## Projekty gRPC služby
Každá služba se skládá min. ze tří projektů:
- **Api**. Aplikace - implementace gRPC služby.  
[Detail projektu Api](grpc-services-api.md)
- **Contracts**. gRPC kontrakty služby. Obsahuje pouze *.proto soubory s definicí služby a zpráv.  
[Detail projektu Contracts](grpc-services-contracts.md)
- **Clients**. C# wrapper nad gRPC službou. Je zde proto, aby konzumenti nemuseli řešit jak volat gRPC, ale mohli ke službě přistupovat přímo přes C# interface.  
[Detail projektu Clients](grpc-services-clients.md)

## Startup gRPC služby
Standardní flow nastavení služby v Program.cs:

Načtení konfigurace ze sekce **CisEnvironmentConfiguration** appsettings.json a vložení do DI jako `ICisEnvironmentConfiguration`.
```csharp
builder.AddCisEnvironmentConfiguration()
```

Společné nastavení .NET fw - nastavení JsonOptions, přidání HttpContextAccessor, options API.
```csharp
builder..AddCisCoreFeatures()
```

Přidání logování a tracingu.
```csharp
builder.AddCisLogging().AddCisTracing()
```

Nastavení health checku.
```csharp
builder.AddCisHealthChecks()
```

Umožnění registrace tříd do DI pomocí atributů.
```csharp
builder.Services.AddAttributedServices(typeof(Program));
```

Basic authentication technickým uživatelem.
```csharp
builder.AddCisServiceAuthentication();
```

Registrace MediatR a pipeline pro validaci requestů.
```csharp
builder.Services.AddCisGrpcInfrastructure(typeof(Program));
```

.NET registrace gRPC služby a gRPC Reflection.
```csharp
builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add<CIS.Infrastructure.gRPC.GenericServerExceptionInterceptor>();
});
builder.Services.AddGrpcReflection();
```

Konfigurace Kestrelu.
```csharp
builder.UseKestrelWithCustomConfiguration();
```

Nastavení autentizace.
```csharp
app.UseAuthentication();
app.UseAuthorization();
```

Registrace technického uživatele do HttpContextu.
```csharp
app.UseCisServiceUserContext();
```

Middleware pro logování.
```csharp
app.UseCisLogging();
```

Registrace gRPC služby, health checku, gRPC reflection.
```csharp
app.UseEndpoints(endpoints =>
{
    endpoints.MapCisHealthChecks();

    endpoints.MapGrpcService<DomainServices.CaseService.Api.Services.CaseService>();

    endpoints.MapGrpcReflectionService();
});
```

Spuštění aplikace. Finally blok je zde proto, že zápis logu má zpoždění a kdyby se aplikace ukončovala okamžitě, pár posledních záznamů by se nezalogovalo.
```csharp
try
{
    app.Run();
}
finally
{
    LoggingExtensions.CloseAndFlush();
}
```
