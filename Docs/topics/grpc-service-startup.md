# Popis startupu aplikace gRPC služby

## Nastavení WebApplicationBuilderu.
Načtení custom aplikační konfigurace a vložení do DI - pokud daná aplikace má vlastní konfiguraci.
```
AppConfiguration appConfiguration = new();
builder.Configuration.GetSection("AppConfiguration").Bind(appConfiguration);
builder.Services.AddSingleton(appConfiguration);
```

Umožní registraci služeb do DI pomocí atributů z CIS.Core.Attributes.
```
builder.Services.AddAttributedServices(typeof(Program));
```

Načtení sekce CisEnvironmentConfiguration z appsettings.json do DI.
```
builder.AddCisEnvironmentConfiguration()
```

Společná konfigurace JSON middleware, vložení IHttpContextAssesor.
```
builder.AddCisCoreFeatures()
```

Zapnutí health checků.
```
builder.AddCisHealthChecks();
```

Zapnutí společného logování.
```
builde.AddCisLogging().AddCisTracing();
```

Přidání autentizace prodle providera vybraného v appsettings.json.
```
builder.AddCisServiceAuthentication();
```

Vložení Clients projektů jiných doménových nebo infrastrukturních služeb.
```
builder.Services
    .AddCisServiceDiscovery()
    .AddCaseService()
```

Registrace gRPC služby.
```
builder.Services.AddCisGrpcInfrastructure(typeof(Program));
builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add<CIS.Infrastructure.gRPC.GenericServerExceptionInterceptor>();
});
builder.Services.AddGrpcReflection();
```

Spustit app jako Windows service
```
if (runAsWinSvc) builder.Host.UseWindowsService();
```

## Vložení middleware

Spustit automatický resolving adres z Service Discovery.
```
app.UseServiceDiscovery();
```

Základní .NET middleware
```
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
```

Middleware vkládající implementaci IServiceUserAccessor (zjištění aktuálního servisního uživatele)
```
app.UseCisServiceUserContext();
```

Custom middleware pro logování služby.
```
app.UseCisLogging();
```

Registrace gRPC služby, gRPC reflection a health checků
```
app.MapCisHealthChecks();
app.MapGrpcService<DomainServices.HouseholdService.Api.Endpoints.HouseholdService>();
app.MapGrpcReflectionService();
```

Spuštění aplikace. Je v try-catch aby se uložili i poslední záznamy do logu po ukončení aplikace.
```
try
{
    app.Run();
}
finally
{
    LoggingExtensions.CloseAndFlush();
}
```