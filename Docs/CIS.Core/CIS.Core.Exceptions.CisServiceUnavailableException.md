#### [CIS.Core](index.md 'index')
### [CIS.Core.Exceptions](CIS.Core.Exceptions.md 'CIS.Core.Exceptions')

## CisServiceUnavailableException Class

Doménová nebo infrastrukturní služba není k dispozici - např. špatné URL volané služby, nebo volaná služba vůbec neběží.

```csharp
public sealed class CisServiceUnavailableException : CIS.Core.Exceptions.BaseCisException
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; [System.Exception](https://docs.microsoft.com/en-us/dotnet/api/System.Exception 'System.Exception') &#129106; [BaseCisException](CIS.Core.Exceptions.BaseCisException.md 'CIS.Core.Exceptions.BaseCisException') &#129106; CisServiceUnavailableException
### Constructors

<a name='CIS.Core.Exceptions.CisServiceUnavailableException.CisServiceUnavailableException(string,string,string)'></a>

## CisServiceUnavailableException(string, string, string) Constructor

```csharp
public CisServiceUnavailableException(string serviceName, string methodName, string message);
```
#### Parameters

<a name='CIS.Core.Exceptions.CisServiceUnavailableException.CisServiceUnavailableException(string,string,string).serviceName'></a>

`serviceName` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Název služby, která selhala

<a name='CIS.Core.Exceptions.CisServiceUnavailableException.CisServiceUnavailableException(string,string,string).methodName'></a>

`methodName` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Metoda / endpoint jehož volání selhalo

<a name='CIS.Core.Exceptions.CisServiceUnavailableException.CisServiceUnavailableException(string,string,string).message'></a>

`message` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Textový popis chyby
### Properties

<a name='CIS.Core.Exceptions.CisServiceUnavailableException.MethodName'></a>

## CisServiceUnavailableException.MethodName Property

Metoda / endpoint jehož volání selhalo

```csharp
public string MethodName { get; set; }
```

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

<a name='CIS.Core.Exceptions.CisServiceUnavailableException.ServiceName'></a>

## CisServiceUnavailableException.ServiceName Property

Název služby, která selhala

```csharp
public string ServiceName { get; set; }
```

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')