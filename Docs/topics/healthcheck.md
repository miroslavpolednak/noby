# Implementace Health Check endpointů

## Implementace Health Check endpointů pro gRPC služby
Každá služba musí implementovat HC endpointy.
Kvůli infrastruktuře KB je třeba i u gRPC služeb mít HC endpoint jak pro standardní gRPC Health check, tak i pro HTTP 1.1 (REST) Health Check.

Podpora pro Health Check v rámci CIS aplikací je v projektu `CIS.Infrastructure.gRPC`.

Vlastní implementace je standardní, používáme `AddGrpcHealthChecks()` a `AddHealthChecks()`, které jsou ale zabalené do vlastní extension metody `AddCisGrpcHealthChecks()`.
Tato extension metoda navíc automaticky doplňuje HC na databáze služby podle connection stringů uvedených v *appsettings.json*.
Connection string na distribuovanou cache *cisDistributedCache* (pokud je uveden), je z tohoto checku vynechán. (TODO dodělat? Může to být Redis, SQL...)

Přidání HC během startupu aplikace:
```csharp
builder.AddCisGrpcHealthChecks();
...
app.MapCisGrpcHealthChecks();
```

### Globální Health Check endpoint
**ServiceDiscovery** obsahuje globální HC endpoint, který sdružuje HC všech služeb, které jsou v databázi *ServiceDiscovery* označené jako `AddToGlobalHealthCheck=1`.  
Endpoint je vystavený přes HTTP 1.1 a vrací JSON response ve formátu požadovaném toolem v KB, který dělá sledování aplikací.

Funguje tak, že načte při startu aplikace všechna URL gRPC služeb zařazených do sledování a v případě vlastního provolání pak volá gRPC HC endpointy těchto služeb a do resultu vkládá jejich výsledek.

## Implementace Health Check endpointů pro FE API
FE API implementuje HC pomocí extension metod z projektu CIS.Infrastructure.WebApi, což je pouze zabalení standardních .NET HC.

```csharp
builder.AddCisHealthChecks();
...
app.UseNobyHealthChecks();
```
