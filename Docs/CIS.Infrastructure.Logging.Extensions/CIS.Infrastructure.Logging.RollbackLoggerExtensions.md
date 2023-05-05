#### [CIS.Infrastructure.Logging.Extensions](index.md 'index')
### [CIS.Infrastructure.Logging](CIS.Infrastructure.Logging.md 'CIS.Infrastructure.Logging')

## RollbackLoggerExtensions Class

```csharp
public static class RollbackLoggerExtensions
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; RollbackLoggerExtensions
### Methods

<a name='CIS.Infrastructure.Logging.RollbackLoggerExtensions.RollbackHandlerStarted(thisMicrosoft.Extensions.Logging.ILogger,string)'></a>

## RollbackLoggerExtensions.RollbackHandlerStarted(this ILogger, string) Method

Spuštění rollabck handleru

```csharp
public static void RollbackHandlerStarted(this Microsoft.Extensions.Logging.ILogger logger, string handlerName);
```
#### Parameters

<a name='CIS.Infrastructure.Logging.RollbackLoggerExtensions.RollbackHandlerStarted(thisMicrosoft.Extensions.Logging.ILogger,string).logger'></a>

`logger` [Microsoft.Extensions.Logging.ILogger](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.Logging.ILogger 'Microsoft.Extensions.Logging.ILogger')

<a name='CIS.Infrastructure.Logging.RollbackLoggerExtensions.RollbackHandlerStarted(thisMicrosoft.Extensions.Logging.ILogger,string).handlerName'></a>

`handlerName` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

<a name='CIS.Infrastructure.Logging.RollbackLoggerExtensions.RollbackHandlerStepDone(thisMicrosoft.Extensions.Logging.ILogger,string,object)'></a>

## RollbackLoggerExtensions.RollbackHandlerStepDone(this ILogger, string, object) Method

Úspěšné projetí jednoho kroku v rollback handleru

```csharp
public static void RollbackHandlerStepDone(this Microsoft.Extensions.Logging.ILogger logger, string key, object value);
```
#### Parameters

<a name='CIS.Infrastructure.Logging.RollbackLoggerExtensions.RollbackHandlerStepDone(thisMicrosoft.Extensions.Logging.ILogger,string,object).logger'></a>

`logger` [Microsoft.Extensions.Logging.ILogger](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.Logging.ILogger 'Microsoft.Extensions.Logging.ILogger')

<a name='CIS.Infrastructure.Logging.RollbackLoggerExtensions.RollbackHandlerStepDone(thisMicrosoft.Extensions.Logging.ILogger,string,object).key'></a>

`key` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

<a name='CIS.Infrastructure.Logging.RollbackLoggerExtensions.RollbackHandlerStepDone(thisMicrosoft.Extensions.Logging.ILogger,string,object).value'></a>

`value` [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object')