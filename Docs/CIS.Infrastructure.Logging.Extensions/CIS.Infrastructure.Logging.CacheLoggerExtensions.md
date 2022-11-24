#### [CIS.Infrastructure.Logging.Extensions](index.md 'index')
### [CIS.Infrastructure.Logging](CIS.Infrastructure.Logging.md 'CIS.Infrastructure.Logging')

## CacheLoggerExtensions Class

Extension metody pro ILogger v oblasti kešování.

```csharp
public static class CacheLoggerExtensions
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; CacheLoggerExtensions
### Methods

<a name='CIS.Infrastructure.Logging.CacheLoggerExtensions.ItemFoundInCache(thisMicrosoft.Extensions.Logging.ILogger,string)'></a>

## CacheLoggerExtensions.ItemFoundInCache(this ILogger, string) Method

Objekt byl nalezen v cache

```csharp
public static void ItemFoundInCache(this Microsoft.Extensions.Logging.ILogger logger, string key);
```
#### Parameters

<a name='CIS.Infrastructure.Logging.CacheLoggerExtensions.ItemFoundInCache(thisMicrosoft.Extensions.Logging.ILogger,string).logger'></a>

`logger` [Microsoft.Extensions.Logging.ILogger](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.Logging.ILogger 'Microsoft.Extensions.Logging.ILogger')

<a name='CIS.Infrastructure.Logging.CacheLoggerExtensions.ItemFoundInCache(thisMicrosoft.Extensions.Logging.ILogger,string).key'></a>

`key` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Klíč objektu v cache

<a name='CIS.Infrastructure.Logging.CacheLoggerExtensions.TryAddItemToCache(thisMicrosoft.Extensions.Logging.ILogger,string)'></a>

## CacheLoggerExtensions.TryAddItemToCache(this ILogger, string) Method

Přidání položky do cache

```csharp
public static void TryAddItemToCache(this Microsoft.Extensions.Logging.ILogger logger, string key);
```
#### Parameters

<a name='CIS.Infrastructure.Logging.CacheLoggerExtensions.TryAddItemToCache(thisMicrosoft.Extensions.Logging.ILogger,string).logger'></a>

`logger` [Microsoft.Extensions.Logging.ILogger](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.Logging.ILogger 'Microsoft.Extensions.Logging.ILogger')

<a name='CIS.Infrastructure.Logging.CacheLoggerExtensions.TryAddItemToCache(thisMicrosoft.Extensions.Logging.ILogger,string).key'></a>

`key` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Klíč objektu v cache