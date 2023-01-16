# Popis startupu aplikace gRPC služby

## Nastavení WebApplicationBuilderu.
Načtení custom aplikační konfigurace a vložení do DI - pokud daná aplikace má vlastní konfiguraci.
```csharp
AppConfiguration appConfiguration = new();
builder.Configuration.GetSection("AppConfiguration").Bind(appConfiguration);
builder.Services.AddSingleton(appConfiguration);
```

Umožní registraci služeb do DI pomocí atributů z CIS.Core.Attributes.
```csharp
builder.Services.AddAttributedServices(typeof(Program));
```

Načtení sekce CisEnvironmentConfiguration z appsettings.json do DI.
```csharp
builder.AddCisEnvironmentConfiguration()
```

Společná konfigurace JSON middleware, vložení IHttpContextAssesor.
```csharp
builder.AddCisCoreFeatures()
```

Zapnutí health checků.
```csharp
builder.AddCisHealthChecks();
```

Zapnutí společného logování.
```csharp
builde.AddCisLogging().AddCisTracing();
```

Přidání autentizace prodle providera vybraného v appsettings.json.
```csharp
builder.AddCisServiceAuthentication();
```

Vložení Clients projektů jiných doménových nebo infrastrukturních služeb.
```csharp
builder.Services
    .AddCisServiceDiscovery()
    .AddCaseService()
```

Registrace gRPC služby.
```csharp
builder.Services.AddCisGrpcInfrastructure(typeof(Program));
builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add<CIS.Infrastructure.gRPC.GenericServerExceptionInterceptor>();
});
builder.Services.AddGrpcReflection();
```

Spustit app jako Windows service
```csharp
if (runAsWinSvc) builder.Host.UseWindowsService();
```

## Vložení middleware

Spustit automatický resolving adres z Service Discovery.
```csharp
app.UseServiceDiscovery();
```

Základní .NET middleware
```csharp
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
```

Middleware vkládající implementaci IServiceUserAccessor (zjištění aktuálního servisního uživatele)
```csharp
app.UseCisServiceUserContext();
```

Custom middleware pro logování služby.
```csharp
app.UseCisLogging();
```

Registrace gRPC služby, gRPC reflection a health checků
```csharp
app.MapCisHealthChecks();
app.MapGrpcService<DomainServices.HouseholdService.Api.Endpoints.HouseholdService>();
app.MapGrpcReflectionService();
```

Spuštění aplikace. Je v try-catch aby se uložili i poslední záznamy do logu po ukončení aplikace.
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