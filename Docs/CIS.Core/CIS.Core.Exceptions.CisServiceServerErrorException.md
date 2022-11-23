#### [CIS.Core](index.md 'index')
### [CIS.Core.Exceptions](CIS.Core.Exceptions.md 'CIS.Core.Exceptions')

## CisServiceServerErrorException Class

HTTP 500. Vyhazuje se pokud naše doménová nebo infrastrkuturní služba vrátí server error - 500.

```csharp
public sealed class CisServiceServerErrorException : CIS.Core.Exceptions.BaseCisException
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; [System.Exception](https://docs.microsoft.com/en-us/dotnet/api/System.Exception 'System.Exception') &#129106; [BaseCisException](CIS.Core.Exceptions.BaseCisException.md 'CIS.Core.Exceptions.BaseCisException') &#129106; CisServiceServerErrorException
### Constructors

<a name='CIS.Core.Exceptions.CisServiceServerErrorException.CisServiceServerErrorException(string,string,string)'></a>

## CisServiceServerErrorException(string, string, string) Constructor

```csharp
public CisServiceServerErrorException(string serviceName, string methodName, string message);
```
#### Parameters

<a name='CIS.Core.Exceptions.CisServiceServerErrorException.CisServiceServerErrorException(string,string,string).serviceName'></a>

`serviceName` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Název služby, která selhala

<a name='CIS.Core.Exceptions.CisServiceServerErrorException.CisServiceServerErrorException(string,string,string).methodName'></a>

`methodName` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Metoda / endpoint jehož volání selhalo

<a name='CIS.Core.Exceptions.CisServiceServerErrorException.CisServiceServerErrorException(string,string,string).message'></a>

`message` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Textový popis chyby
### Properties

<a name='CIS.Core.Exceptions.CisServiceServerErrorException.MethodName'></a>

## CisServiceServerErrorException.MethodName Property

Metoda / endpoint jehož volání selhalo

```csharp
public string MethodName { get; set; }
```

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

<a name='CIS.Core.Exceptions.CisServiceServerErrorException.ServiceName'></a>

## CisServiceServerErrorException.ServiceName Property

Název služby, která selhala

```csharp
public string ServiceName { get; set; }
```

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')