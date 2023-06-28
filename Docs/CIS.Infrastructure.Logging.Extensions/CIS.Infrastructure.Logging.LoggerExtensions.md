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

<a name='CIS.Infrastructure.Logging.LoggerExtensions.LogValidationResults(thisMicrosoft.Extensions.Logging.ILogger,CIS.Core.Exceptions.CisValidationException)'></a>

## LoggerExtensions.LogValidationResults(this ILogger, CisValidationException) Method

Logování chyb zejména z FluentValidation.

```csharp
public static void LogValidationResults(this Microsoft.Extensions.Logging.ILogger logger, CIS.Core.Exceptions.CisValidationException ex);
```
#### Parameters

<a name='CIS.Infrastructure.Logging.LoggerExtensions.LogValidationResults(thisMicrosoft.Extensions.Logging.ILogger,CIS.Core.Exceptions.CisValidationException).logger'></a>

`logger` [Microsoft.Extensions.Logging.ILogger](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.Logging.ILogger 'Microsoft.Extensions.Logging.ILogger')

<a name='CIS.Infrastructure.Logging.LoggerExtensions.LogValidationResults(thisMicrosoft.Extensions.Logging.ILogger,CIS.Core.Exceptions.CisValidationException).ex'></a>

`ex` [CIS.Core.Exceptions.CisValidationException](https://docs.microsoft.com/en-us/dotnet/api/CIS.Core.Exceptions.CisValidationException 'CIS.Core.Exceptions.CisValidationException')

### Remarks
Do logu uloží seznam chyb (Errors kolekci) do kontextu pod klíčem "Errors".