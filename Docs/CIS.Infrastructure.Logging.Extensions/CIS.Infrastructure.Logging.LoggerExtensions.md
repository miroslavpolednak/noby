#### [CIS.Infrastructure.Logging.Extensions](index.md 'index')
### [CIS.Infrastructure.Logging](CIS.Infrastructure.Logging.md 'CIS.Infrastructure.Logging')

## LoggerExtensions Class

```csharp
public static class LoggerExtensions
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; LoggerExtensions
### Methods

<a name='CIS.Infrastructure.Logging.LoggerExtensions.FoundItems(thisMicrosoft.Extensions.Logging.ILogger,int,string)'></a>

## LoggerExtensions.FoundItems(this ILogger, int, string) Method

Nalezeno záznamů / entit / objektů.

```csharp
public static void FoundItems(this Microsoft.Extensions.Logging.ILogger logger, int count, string entityName);
```
#### Parameters

<a name='CIS.Infrastructure.Logging.LoggerExtensions.FoundItems(thisMicrosoft.Extensions.Logging.ILogger,int,string).logger'></a>

`logger` [Microsoft.Extensions.Logging.ILogger](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.Logging.ILogger 'Microsoft.Extensions.Logging.ILogger')

<a name='CIS.Infrastructure.Logging.LoggerExtensions.FoundItems(thisMicrosoft.Extensions.Logging.ILogger,int,string).count'></a>

`count` [System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

Počet nalezených záznamů.

<a name='CIS.Infrastructure.Logging.LoggerExtensions.FoundItems(thisMicrosoft.Extensions.Logging.ILogger,int,string).entityName'></a>

`entityName` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Typ nalezené entity

### Remarks
Např. entit v databázi nebo položek v keši.

<a name='CIS.Infrastructure.Logging.LoggerExtensions.FoundItems(thisMicrosoft.Extensions.Logging.ILogger,int)'></a>

## LoggerExtensions.FoundItems(this ILogger, int) Method

Nalezeno záznamů / entit / objektů.

```csharp
public static void FoundItems(this Microsoft.Extensions.Logging.ILogger logger, int count);
```
#### Parameters

<a name='CIS.Infrastructure.Logging.LoggerExtensions.FoundItems(thisMicrosoft.Extensions.Logging.ILogger,int).logger'></a>

`logger` [Microsoft.Extensions.Logging.ILogger](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.Logging.ILogger 'Microsoft.Extensions.Logging.ILogger')

<a name='CIS.Infrastructure.Logging.LoggerExtensions.FoundItems(thisMicrosoft.Extensions.Logging.ILogger,int).count'></a>

`count` [System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

Počet nalezených záznamů.

### Remarks
Např. entit v databázi nebo položek v keši.

<a name='CIS.Infrastructure.Logging.LoggerExtensions.LogSerializedObject(thisMicrosoft.Extensions.Logging.ILogger,string,object,Microsoft.Extensions.Logging.LogLevel)'></a>

## LoggerExtensions.LogSerializedObject(this ILogger, string, object, LogLevel) Method

TODO: odstranit? Logovat do log contextu?

```csharp
public static void LogSerializedObject(this Microsoft.Extensions.Logging.ILogger logger, string name, object objectToLog, Microsoft.Extensions.Logging.LogLevel logLevel=Microsoft.Extensions.Logging.LogLevel.Debug);
```
#### Parameters

<a name='CIS.Infrastructure.Logging.LoggerExtensions.LogSerializedObject(thisMicrosoft.Extensions.Logging.ILogger,string,object,Microsoft.Extensions.Logging.LogLevel).logger'></a>

`logger` [Microsoft.Extensions.Logging.ILogger](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.Logging.ILogger 'Microsoft.Extensions.Logging.ILogger')

<a name='CIS.Infrastructure.Logging.LoggerExtensions.LogSerializedObject(thisMicrosoft.Extensions.Logging.ILogger,string,object,Microsoft.Extensions.Logging.LogLevel).name'></a>

`name` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

<a name='CIS.Infrastructure.Logging.LoggerExtensions.LogSerializedObject(thisMicrosoft.Extensions.Logging.ILogger,string,object,Microsoft.Extensions.Logging.LogLevel).objectToLog'></a>

`objectToLog` [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object')

<a name='CIS.Infrastructure.Logging.LoggerExtensions.LogSerializedObject(thisMicrosoft.Extensions.Logging.ILogger,string,object,Microsoft.Extensions.Logging.LogLevel).logLevel'></a>

`logLevel` [Microsoft.Extensions.Logging.LogLevel](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.Logging.LogLevel 'Microsoft.Extensions.Logging.LogLevel')