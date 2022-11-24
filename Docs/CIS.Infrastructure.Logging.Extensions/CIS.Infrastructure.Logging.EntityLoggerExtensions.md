#### [CIS.Infrastructure.Logging.Extensions](index.md 'index')
### [CIS.Infrastructure.Logging](CIS.Infrastructure.Logging.md 'CIS.Infrastructure.Logging')

## EntityLoggerExtensions Class

Extension metody pro ILogger pro události týkající se entit.

```csharp
public static class EntityLoggerExtensions
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; EntityLoggerExtensions
### Methods

<a name='CIS.Infrastructure.Logging.EntityLoggerExtensions.EntityAlreadyExist(thisMicrosoft.Extensions.Logging.ILogger,CIS.Core.Exceptions.CisAlreadyExistsException)'></a>

## EntityLoggerExtensions.EntityAlreadyExist(this ILogger, CisAlreadyExistsException) Method

Entita již existuje (např. v databázi).

```csharp
public static void EntityAlreadyExist(this Microsoft.Extensions.Logging.ILogger logger, CIS.Core.Exceptions.CisAlreadyExistsException ex);
```
#### Parameters

<a name='CIS.Infrastructure.Logging.EntityLoggerExtensions.EntityAlreadyExist(thisMicrosoft.Extensions.Logging.ILogger,CIS.Core.Exceptions.CisAlreadyExistsException).logger'></a>

`logger` [Microsoft.Extensions.Logging.ILogger](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.Logging.ILogger 'Microsoft.Extensions.Logging.ILogger')

<a name='CIS.Infrastructure.Logging.EntityLoggerExtensions.EntityAlreadyExist(thisMicrosoft.Extensions.Logging.ILogger,CIS.Core.Exceptions.CisAlreadyExistsException).ex'></a>

`ex` [CIS.Core.Exceptions.CisAlreadyExistsException](https://docs.microsoft.com/en-us/dotnet/api/CIS.Core.Exceptions.CisAlreadyExistsException 'CIS.Core.Exceptions.CisAlreadyExistsException')

<a name='CIS.Infrastructure.Logging.EntityLoggerExtensions.EntityAlreadyExist(thisMicrosoft.Extensions.Logging.ILogger,string,long,System.Exception)'></a>

## EntityLoggerExtensions.EntityAlreadyExist(this ILogger, string, long, Exception) Method

Entita již existuje (např. v databázi).

```csharp
public static void EntityAlreadyExist(this Microsoft.Extensions.Logging.ILogger logger, string entityName, long entityId, System.Exception ex);
```
#### Parameters

<a name='CIS.Infrastructure.Logging.EntityLoggerExtensions.EntityAlreadyExist(thisMicrosoft.Extensions.Logging.ILogger,string,long,System.Exception).logger'></a>

`logger` [Microsoft.Extensions.Logging.ILogger](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.Logging.ILogger 'Microsoft.Extensions.Logging.ILogger')

<a name='CIS.Infrastructure.Logging.EntityLoggerExtensions.EntityAlreadyExist(thisMicrosoft.Extensions.Logging.ILogger,string,long,System.Exception).entityName'></a>

`entityName` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Název typu entity

<a name='CIS.Infrastructure.Logging.EntityLoggerExtensions.EntityAlreadyExist(thisMicrosoft.Extensions.Logging.ILogger,string,long,System.Exception).entityId'></a>

`entityId` [System.Int64](https://docs.microsoft.com/en-us/dotnet/api/System.Int64 'System.Int64')

ID entity

<a name='CIS.Infrastructure.Logging.EntityLoggerExtensions.EntityAlreadyExist(thisMicrosoft.Extensions.Logging.ILogger,string,long,System.Exception).ex'></a>

`ex` [System.Exception](https://docs.microsoft.com/en-us/dotnet/api/System.Exception 'System.Exception')

<a name='CIS.Infrastructure.Logging.EntityLoggerExtensions.EntityCreated(thisMicrosoft.Extensions.Logging.ILogger,string,long)'></a>

## EntityLoggerExtensions.EntityCreated(this ILogger, string, long) Method

Entita byla právě vytvořena.

```csharp
public static void EntityCreated(this Microsoft.Extensions.Logging.ILogger logger, string entityName, long entityId);
```
#### Parameters

<a name='CIS.Infrastructure.Logging.EntityLoggerExtensions.EntityCreated(thisMicrosoft.Extensions.Logging.ILogger,string,long).logger'></a>

`logger` [Microsoft.Extensions.Logging.ILogger](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.Logging.ILogger 'Microsoft.Extensions.Logging.ILogger')

<a name='CIS.Infrastructure.Logging.EntityLoggerExtensions.EntityCreated(thisMicrosoft.Extensions.Logging.ILogger,string,long).entityName'></a>

`entityName` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Název typu entity

<a name='CIS.Infrastructure.Logging.EntityLoggerExtensions.EntityCreated(thisMicrosoft.Extensions.Logging.ILogger,string,long).entityId'></a>

`entityId` [System.Int64](https://docs.microsoft.com/en-us/dotnet/api/System.Int64 'System.Int64')

ID nové entity

<a name='CIS.Infrastructure.Logging.EntityLoggerExtensions.EntityNotFound(thisMicrosoft.Extensions.Logging.ILogger,CIS.Core.Exceptions.CisNotFoundException)'></a>

## EntityLoggerExtensions.EntityNotFound(this ILogger, CisNotFoundException) Method

Entita nebyla nalezena (např. ID neexistuje v databázi)

```csharp
public static void EntityNotFound(this Microsoft.Extensions.Logging.ILogger logger, CIS.Core.Exceptions.CisNotFoundException ex);
```
#### Parameters

<a name='CIS.Infrastructure.Logging.EntityLoggerExtensions.EntityNotFound(thisMicrosoft.Extensions.Logging.ILogger,CIS.Core.Exceptions.CisNotFoundException).logger'></a>

`logger` [Microsoft.Extensions.Logging.ILogger](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.Logging.ILogger 'Microsoft.Extensions.Logging.ILogger')

<a name='CIS.Infrastructure.Logging.EntityLoggerExtensions.EntityNotFound(thisMicrosoft.Extensions.Logging.ILogger,CIS.Core.Exceptions.CisNotFoundException).ex'></a>

`ex` [CIS.Core.Exceptions.CisNotFoundException](https://docs.microsoft.com/en-us/dotnet/api/CIS.Core.Exceptions.CisNotFoundException 'CIS.Core.Exceptions.CisNotFoundException')

<a name='CIS.Infrastructure.Logging.EntityLoggerExtensions.EntityNotFound(thisMicrosoft.Extensions.Logging.ILogger,string,long,System.Exception)'></a>

## EntityLoggerExtensions.EntityNotFound(this ILogger, string, long, Exception) Method

Entita nebyla nalezena (např. ID neexistuje v databázi)

```csharp
public static void EntityNotFound(this Microsoft.Extensions.Logging.ILogger logger, string entityName, long entityId, System.Exception ex);
```
#### Parameters

<a name='CIS.Infrastructure.Logging.EntityLoggerExtensions.EntityNotFound(thisMicrosoft.Extensions.Logging.ILogger,string,long,System.Exception).logger'></a>

`logger` [Microsoft.Extensions.Logging.ILogger](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.Logging.ILogger 'Microsoft.Extensions.Logging.ILogger')

<a name='CIS.Infrastructure.Logging.EntityLoggerExtensions.EntityNotFound(thisMicrosoft.Extensions.Logging.ILogger,string,long,System.Exception).entityName'></a>

`entityName` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Název typu entity

<a name='CIS.Infrastructure.Logging.EntityLoggerExtensions.EntityNotFound(thisMicrosoft.Extensions.Logging.ILogger,string,long,System.Exception).entityId'></a>

`entityId` [System.Int64](https://docs.microsoft.com/en-us/dotnet/api/System.Int64 'System.Int64')

ID entity

<a name='CIS.Infrastructure.Logging.EntityLoggerExtensions.EntityNotFound(thisMicrosoft.Extensions.Logging.ILogger,string,long,System.Exception).ex'></a>

`ex` [System.Exception](https://docs.microsoft.com/en-us/dotnet/api/System.Exception 'System.Exception')