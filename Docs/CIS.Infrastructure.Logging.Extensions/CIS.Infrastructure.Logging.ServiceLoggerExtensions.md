#### [CIS.Infrastructure.Logging.Extensions](index.md 'index')
### [CIS.Infrastructure.Logging](CIS.Infrastructure.Logging.md 'CIS.Infrastructure.Logging')

## ServiceLoggerExtensions Class

Extension metody pro ILogger v týkající se handlingu webových služeb třetích stran.

```csharp
public static class ServiceLoggerExtensions
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; ServiceLoggerExtensions
### Methods

<a name='CIS.Infrastructure.Logging.ServiceLoggerExtensions.ExtServiceAuthenticationFailed(thisMicrosoft.Extensions.Logging.ILogger,System.Exception)'></a>

## ServiceLoggerExtensions.ExtServiceAuthenticationFailed(this ILogger, Exception) Method

Nepodařilo se autentizovat do služby.

```csharp
public static void ExtServiceAuthenticationFailed(this Microsoft.Extensions.Logging.ILogger logger, System.Exception ex);
```
#### Parameters

<a name='CIS.Infrastructure.Logging.ServiceLoggerExtensions.ExtServiceAuthenticationFailed(thisMicrosoft.Extensions.Logging.ILogger,System.Exception).logger'></a>

`logger` [Microsoft.Extensions.Logging.ILogger](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.Logging.ILogger 'Microsoft.Extensions.Logging.ILogger')

<a name='CIS.Infrastructure.Logging.ServiceLoggerExtensions.ExtServiceAuthenticationFailed(thisMicrosoft.Extensions.Logging.ILogger,System.Exception).ex'></a>

`ex` [System.Exception](https://docs.microsoft.com/en-us/dotnet/api/System.Exception 'System.Exception')

<a name='CIS.Infrastructure.Logging.ServiceLoggerExtensions.ExtServiceUnavailable(thisMicrosoft.Extensions.Logging.ILogger,string,System.Exception)'></a>

## ServiceLoggerExtensions.ExtServiceUnavailable(this ILogger, string, Exception) Method

Služba třetí strany není dostupná.

```csharp
public static void ExtServiceUnavailable(this Microsoft.Extensions.Logging.ILogger logger, string parentServiceName, System.Exception ex);
```
#### Parameters

<a name='CIS.Infrastructure.Logging.ServiceLoggerExtensions.ExtServiceUnavailable(thisMicrosoft.Extensions.Logging.ILogger,string,System.Exception).logger'></a>

`logger` [Microsoft.Extensions.Logging.ILogger](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.Logging.ILogger 'Microsoft.Extensions.Logging.ILogger')

<a name='CIS.Infrastructure.Logging.ServiceLoggerExtensions.ExtServiceUnavailable(thisMicrosoft.Extensions.Logging.ILogger,string,System.Exception).parentServiceName'></a>

`parentServiceName` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Název doménové služby, ze které je služba třetí strany volaná.

<a name='CIS.Infrastructure.Logging.ServiceLoggerExtensions.ExtServiceUnavailable(thisMicrosoft.Extensions.Logging.ILogger,string,System.Exception).ex'></a>

`ex` [System.Exception](https://docs.microsoft.com/en-us/dotnet/api/System.Exception 'System.Exception')

<a name='CIS.Infrastructure.Logging.ServiceLoggerExtensions.ServiceUnavailable(thisMicrosoft.Extensions.Logging.ILogger,string,System.Exception)'></a>

## ServiceLoggerExtensions.ServiceUnavailable(this ILogger, string, Exception) Method

Doménová služba není dostupná.

```csharp
public static void ServiceUnavailable(this Microsoft.Extensions.Logging.ILogger logger, string serviceName, System.Exception ex);
```
#### Parameters

<a name='CIS.Infrastructure.Logging.ServiceLoggerExtensions.ServiceUnavailable(thisMicrosoft.Extensions.Logging.ILogger,string,System.Exception).logger'></a>

`logger` [Microsoft.Extensions.Logging.ILogger](https://docs.microsoft.com/en-us/dotnet/api/Microsoft.Extensions.Logging.ILogger 'Microsoft.Extensions.Logging.ILogger')

<a name='CIS.Infrastructure.Logging.ServiceLoggerExtensions.ServiceUnavailable(thisMicrosoft.Extensions.Logging.ILogger,string,System.Exception).serviceName'></a>

`serviceName` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Název služby

<a name='CIS.Infrastructure.Logging.ServiceLoggerExtensions.ServiceUnavailable(thisMicrosoft.Extensions.Logging.ILogger,string,System.Exception).ex'></a>

`ex` [System.Exception](https://docs.microsoft.com/en-us/dotnet/api/System.Exception 'System.Exception')