# Nastavení a použití Distributed cache
Podpora pro Distributed cache je v projektu `CIS.Infrastructure.Caching`.
Podporujeme tři cache providery:
- MsSql
- Redis
- InMemory

## Registrace cache
Pro zaregistrování cache existuje extension metoda do startupu aplikace:
```csharp
using CIS.Infrastructure.StartupExtensions;
...
builder.AddCisDistributedCache();
```

Nastavení cache je v *appsettings.json*, struktura je dle `CIS.Core.Configuration.ICisDistributedCacheConfiguration`:
```json
"DistributedCache": {
	"CacheType": "MsSql",
	"SerializationType": "Json",
	"KeyPrefix": ""
}
```

**CacheType**  
Druh cache - enum `ICisDistributedCacheConfiguration.CacheTypes`.

**SerializationType**  
Způsob uložení objektu do cache - enum `ICisDistributedCacheConfiguration.SerializationTypes`.

**KeyPrefix**  
Prefix klíče při nastavení Redis provider.

## Použití Distributed cache v aplikaci
Cache se používá standardním způsobem, tj. instancí `IDistributedCache` z DI.  
Nad tímto interfacem existují pomocné extension metody, které umožňují jednoduché ukládání objektů z a do cache.
Tyto extension metody jsou:

```csharp
async Task<TModel?> GetObjectAsync<TModel>(this IDistributedCache cache, string key, CancellationToken cancellationToken = default(CancellationToken))
async Task SetObjectAsync<TModel>(this IDistributedCache cache, string key, TModel value, DistributedCacheEntryOptions options, CancellationToken cancellationToken = default(CancellationToken))
```

Příklad použití:
```csharp
using CIS.Infrastructure.Caching;
...
await _distributedCache.SetObjectAsync("my_key", new
{
    SomeValue = 1
}, new DistributedCacheEntryOptions
{
    AbsoluteExpiration = DateTime.Now.AddHours(1),
}, cancellationToken);
```
