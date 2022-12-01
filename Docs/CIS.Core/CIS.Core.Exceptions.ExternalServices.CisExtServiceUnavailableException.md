#### [CIS.Core](index.md 'index')
### [CIS.Core.Exceptions.ExternalServices](CIS.Core.Exceptions.ExternalServices.md 'CIS.Core.Exceptions.ExternalServices')

## CisExtServiceUnavailableException Class

Služba třetí strany (ExternalServices) není dostupná - např. špatné URL volané služby, nebo volaná služba vůbec neběží.

```csharp
public sealed class CisExtServiceUnavailableException : CIS.Core.Exceptions.BaseCisException
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; [System.Exception](https://docs.microsoft.com/en-us/dotnet/api/System.Exception 'System.Exception') &#129106; [BaseCisException](CIS.Core.Exceptions.BaseCisException.md 'CIS.Core.Exceptions.BaseCisException') &#129106; CisExtServiceUnavailableException
### Constructors

<a name='CIS.Core.Exceptions.ExternalServices.CisExtServiceUnavailableException.CisExtServiceUnavailableException(string,string,string)'></a>

## CisExtServiceUnavailableException(string, string, string) Constructor

```csharp
public CisExtServiceUnavailableException(string serviceName, string requestUri, string message);
```
#### Parameters

<a name='CIS.Core.Exceptions.ExternalServices.CisExtServiceUnavailableException.CisExtServiceUnavailableException(string,string,string).serviceName'></a>

`serviceName` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Název služby, která selhala

<a name='CIS.Core.Exceptions.ExternalServices.CisExtServiceUnavailableException.CisExtServiceUnavailableException(string,string,string).requestUri'></a>

`requestUri` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

URI jehož volání selhalo

<a name='CIS.Core.Exceptions.ExternalServices.CisExtServiceUnavailableException.CisExtServiceUnavailableException(string,string,string).message'></a>

`message` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Textový popis chyby
### Properties

<a name='CIS.Core.Exceptions.ExternalServices.CisExtServiceUnavailableException.RequestUri'></a>

## CisExtServiceUnavailableException.RequestUri Property

URI jehož volání selhalo

```csharp
public string RequestUri { get; set; }
```

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

<a name='CIS.Core.Exceptions.ExternalServices.CisExtServiceUnavailableException.ServiceName'></a>

## CisExtServiceUnavailableException.ServiceName Property

Název služby, která selhala

```csharp
public string ServiceName { get; set; }
```

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')