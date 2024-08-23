# Kešování response gRPC služeb v Client projektech
U často volaných endpointů doménových služeb u kterých se příliš nemění response je možné zvážit jejich kešování.
Kešování response z gRPC služeb je vždy součástí implementace jejich **Client** projektu, nikdy není přímo v kódu konzumenta (např. v FE API).

Kešování je implementováno pomocí `IGrpcClientResponseCache` z projektu `CIS.Infrastructure.Caching`.
Kešování objektů je ve dvou úrovních:
1) in memory cache - jedná se o scoped keš, která funguje v rámci jednoho requestu
2) distributed cache - keš, která je sdílená v celém prostředí NOBY

Do distribuované keše se objekty ukládají s klíčem ve tvaru: `GDCC:{serviceName}:{methodName}-{key}`.
Tj. v případe klienta *CaseService* a metody *ValidateCaseId* pro *CaseId*=123 bude klíč ve tvaru `GDCC:CaseService:ValidateCaseId-123`.

> Konzument kešovaného **Client** projektu doménové služby musí podporovat distribuovanou keš, tj. ve startupu konzumenta musí být zaregistrována pomocí `GrpcServiceBuilder.AddDistributedCache()`. Zároveň musí mít daný konzument správně nakonfigurovanou disrtribuovanou keš v **appsettings.json**.

Interface `IGrpcClientResponseCache` poskytuje několik metod pro kešování objektů:
- **GetLocalOrDistributed**: standardní kešování, používá jak in memory tak distribuovanou keš
- **GetDistributedOnly**: pouze distribuovaná keš
- **GetLocalOnly**: pouze in memory keš

> Standardní serializace kešovaných objektů je nastavena na JSON. Nicméně existují přetížené metody pro nastavení vlastní serializace (např Protobuf).

```csharp
class CaseServiceClient(
    Contracts.v1.CaseService.CaseServiceClient _service,
    IGrpcClientResponseCache<CaseServiceClient> _cache)

public async Task<ValidateCaseIdResponse> ValidateCaseId(long caseId, CancellationToken cancellationToken = default)
{
    return await _cache.GetLocalOrDistributed(
        caseId, 
        async (c) => await ValidateCaseIdWithoutCache(caseId, c), 
        new Microsoft.Extensions.Caching.Distributed.DistributedCacheEntryOptions
        {
            SlidingExpiration = TimeSpan.FromMinutes(30)
        },
        cancellationToken);
}

// pro extrémní případy je dobré, aby Client projekt měl metodu umožňující zavolat daný endpoint bez použítí keše
public async Task<ValidateCaseIdResponse> ValidateCaseIdWithoutCache(long caseId, CancellationToken cancellationToken = default)
    {
        return await _service.ValidateCaseIdAsync(new ValidateCaseIdRequest
        {
            CaseId = caseId
        }, cancellationToken: cancellationToken);
    }
```

## Invalidace keše v API / ve chvíli změny podkladových dat
Pokud se změní data ze kterých je složen kešovaný response objekt, je potřeba tuto keš invalidovat.
Invalidace keše je vždy na straně poskytovatele dat, tj. v gRPC službě.

Pro invalidaci objektu v keši je potřeba znát klíč pod kterým je objekt uložen.
Pro zjednodušení a centralizaci tohoto procesu existuje interface `IGrpcServerResponseCache`, který poskytuje metodu `InvalidateEntry`.

```csharp
class MyClass (IGrpcServerResponseCache _responseCache)

// v tomto případě je ValidateCaseId název endpointu / metody v Client projektu.
// request.CaseId je klíč pod kterým je uložený objekt v keši
await _responseCache.InvalidateEntry(nameof(ValidateCaseId), request.CaseId);

// v předchozím případě je část klíče s názvem služby automaticky doplněna z ICisEnvironmentConfiguration
// pokud je potřeba specifikovat službu ručně, je možné použít následující zápis
await _responseCache.InvalidateEntry("CaseService", nameof(ValidateCaseId), request.CaseId);
```
