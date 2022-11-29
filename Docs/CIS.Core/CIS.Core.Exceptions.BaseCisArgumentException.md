#### [CIS.Core](index.md 'index')
### [CIS.Core.Exceptions](CIS.Core.Exceptions.md 'CIS.Core.Exceptions')

## BaseCisArgumentException Class

Stejná chyba jako [System.ArgumentException](https://docs.microsoft.com/en-us/dotnet/api/System.ArgumentException 'System.ArgumentException'), ale obsahuje navíc CIS error kód

```csharp
public abstract class BaseCisArgumentException : System.ArgumentException
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; [System.Exception](https://docs.microsoft.com/en-us/dotnet/api/System.Exception 'System.Exception') &#129106; [System.SystemException](https://docs.microsoft.com/en-us/dotnet/api/System.SystemException 'System.SystemException') &#129106; [System.ArgumentException](https://docs.microsoft.com/en-us/dotnet/api/System.ArgumentException 'System.ArgumentException') &#129106; BaseCisArgumentException

Derived  
&#8627; [CisArgumentException](CIS.Core.Exceptions.CisArgumentException.md 'CIS.Core.Exceptions.CisArgumentException')  
&#8627; [CisInvalidApplicationKeyException](CIS.Core.Exceptions.CisInvalidApplicationKeyException.md 'CIS.Core.Exceptions.CisInvalidApplicationKeyException')  
&#8627; [CisInvalidEnvironmentNameException](CIS.Core.Exceptions.CisInvalidEnvironmentNameException.md 'CIS.Core.Exceptions.CisInvalidEnvironmentNameException')
### Properties

<a name='CIS.Core.Exceptions.BaseCisArgumentException.ExceptionCode'></a>

## BaseCisArgumentException.ExceptionCode Property

CIS error kód

```csharp
public int ExceptionCode { get; set; }
```

#### Property Value
[System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')