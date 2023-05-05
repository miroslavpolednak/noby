#### [CIS.Core](index.md 'index')
### [CIS.Core.Exceptions](CIS.Core.Exceptions.md 'CIS.Core.Exceptions')

## CisExceptionItem Class

Instance jednotlive chyby

```csharp
public class CisExceptionItem :
System.IEquatable<CIS.Core.Exceptions.CisExceptionItem>
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; CisExceptionItem

Implements [System.IEquatable&lt;](https://docs.microsoft.com/en-us/dotnet/api/System.IEquatable-1 'System.IEquatable`1')[CisExceptionItem](CIS.Core.Exceptions.CisExceptionItem.md 'CIS.Core.Exceptions.CisExceptionItem')[&gt;](https://docs.microsoft.com/en-us/dotnet/api/System.IEquatable-1 'System.IEquatable`1')
### Constructors

<a name='CIS.Core.Exceptions.CisExceptionItem.CisExceptionItem(string,string)'></a>

## CisExceptionItem(string, string) Constructor

Instance jednotlive chyby

```csharp
public CisExceptionItem(string ExceptionCode, string Message);
```
#### Parameters

<a name='CIS.Core.Exceptions.CisExceptionItem.CisExceptionItem(string,string).ExceptionCode'></a>

`ExceptionCode` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

CIS error kód

<a name='CIS.Core.Exceptions.CisExceptionItem.CisExceptionItem(string,string).Message'></a>

`Message` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

chybová zpráva
### Properties

<a name='CIS.Core.Exceptions.CisExceptionItem.ExceptionCode'></a>

## CisExceptionItem.ExceptionCode Property

CIS error kód

```csharp
public string ExceptionCode { get; set; }
```

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

<a name='CIS.Core.Exceptions.CisExceptionItem.Message'></a>

## CisExceptionItem.Message Property

chybová zpráva

```csharp
public string Message { get; set; }
```

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')