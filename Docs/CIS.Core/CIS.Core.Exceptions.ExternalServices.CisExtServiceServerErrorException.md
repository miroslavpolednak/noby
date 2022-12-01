#### [CIS.Core](index.md 'index')
### [CIS.Core.Exceptions.ExternalServices](CIS.Core.Exceptions.ExternalServices.md 'CIS.Core.Exceptions.ExternalServices')

## CisExtServiceServerErrorException Class

```csharp
public sealed class CisExtServiceServerErrorException : CIS.Core.Exceptions.BaseCisException
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; [System.Exception](https://docs.microsoft.com/en-us/dotnet/api/System.Exception 'System.Exception') &#129106; [BaseCisException](CIS.Core.Exceptions.BaseCisException.md 'CIS.Core.Exceptions.BaseCisException') &#129106; CisExtServiceServerErrorException
### Constructors

<a name='CIS.Core.Exceptions.ExternalServices.CisExtServiceServerErrorException.CisExtServiceServerErrorException(string,string,string)'></a>

## CisExtServiceServerErrorException(string, string, string) Constructor

```csharp
public CisExtServiceServerErrorException(string serviceName, string requestUri, string message);
```
#### Parameters

<a name='CIS.Core.Exceptions.ExternalServices.CisExtServiceServerErrorException.CisExtServiceServerErrorException(string,string,string).serviceName'></a>

`serviceName` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Název služby, která selhala

<a name='CIS.Core.Exceptions.ExternalServices.CisExtServiceServerErrorException.CisExtServiceServerErrorException(string,string,string).requestUri'></a>

`requestUri` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

URI jehož volání selhalo

<a name='CIS.Core.Exceptions.ExternalServices.CisExtServiceServerErrorException.CisExtServiceServerErrorException(string,string,string).message'></a>

`message` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Textový popis chyby
### Properties

<a name='CIS.Core.Exceptions.ExternalServices.CisExtServiceServerErrorException.RequestUri'></a>

## CisExtServiceServerErrorException.RequestUri Property

URI jehož volání selhalo

```csharp
public string RequestUri { get; set; }
```

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

<a name='CIS.Core.Exceptions.ExternalServices.CisExtServiceServerErrorException.ServiceName'></a>

## CisExtServiceServerErrorException.ServiceName Property

Název služby, která selhala

```csharp
public string ServiceName { get; set; }
```

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')