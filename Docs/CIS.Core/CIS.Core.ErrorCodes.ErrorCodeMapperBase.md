#### [CIS.Core](index.md 'index')
### [CIS.Core.ErrorCodes](CIS.Core.ErrorCodes.md 'CIS.Core.ErrorCodes')

## ErrorCodeMapperBase Class

Base třída pro zadávání chybových hlášek zejména pro FluentValidation a Rpc exceptions.

```csharp
public abstract class ErrorCodeMapperBase
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; ErrorCodeMapperBase
### Properties

<a name='CIS.Core.ErrorCodes.ErrorCodeMapperBase.Messages'></a>

## ErrorCodeMapperBase.Messages Property

Slovník chybových hlášek [ExceptionCode, ExceptionMessage].

```csharp
public static CIS.Core.ErrorCodes.IErrorCodesDictionary Messages { get; set; }
```

#### Property Value
[IErrorCodesDictionary](CIS.Core.ErrorCodes.IErrorCodesDictionary.md 'CIS.Core.ErrorCodes.IErrorCodesDictionary')
### Methods

<a name='CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateNotFoundException(int,object)'></a>

## ErrorCodeMapperBase.CreateNotFoundException(int, object) Method

Vytvoří vyjímku typu NotFound s textem pro daný ExceptionCode.

```csharp
public static CIS.Core.Exceptions.CisNotFoundException CreateNotFoundException(int exceptionCode, object? parameter=null);
```
#### Parameters

<a name='CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateNotFoundException(int,object).exceptionCode'></a>

`exceptionCode` [System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

Kód chybové hlášky.

<a name='CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateNotFoundException(int,object).parameter'></a>

`parameter` [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object')

Volitelně ID entity, která nebyla nalezena.

#### Returns
[CisNotFoundException](CIS.Core.Exceptions.CisNotFoundException.md 'CIS.Core.Exceptions.CisNotFoundException')  
Instance vyjímky, která má být vyvolána.

<a name='CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateValidationException(int,object)'></a>

## ErrorCodeMapperBase.CreateValidationException(int, object) Method

Vytvoří vyjímku typu ValidationFound s textem pro daný ExceptionCode.

```csharp
public static CIS.Core.Exceptions.CisValidationException CreateValidationException(int exceptionCode, object? parameter=null);
```
#### Parameters

<a name='CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateValidationException(int,object).exceptionCode'></a>

`exceptionCode` [System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

Kód chybové hlášky.

<a name='CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateValidationException(int,object).parameter'></a>

`parameter` [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object')

Volitelně parametr, který má být vložen na placeholder {PropertyValue}.

#### Returns
[CisValidationException](CIS.Core.Exceptions.CisValidationException.md 'CIS.Core.Exceptions.CisValidationException')  
Instance vyjímky, která má být vyvolána.

<a name='CIS.Core.ErrorCodes.ErrorCodeMapperBase.GetMessage(int,object)'></a>

## ErrorCodeMapperBase.GetMessage(int, object) Method

Vrátí chybovou hláškou podle zadaného ExceptionCode - ten musí být uvedený v překladovém slovníku Messages.

```csharp
public static string GetMessage(int exceptionCode, object? parameter=null);
```
#### Parameters

<a name='CIS.Core.ErrorCodes.ErrorCodeMapperBase.GetMessage(int,object).exceptionCode'></a>

`exceptionCode` [System.Int32](https://docs.microsoft.com/en-us/dotnet/api/System.Int32 'System.Int32')

Kód chybové hlášky.

<a name='CIS.Core.ErrorCodes.ErrorCodeMapperBase.GetMessage(int,object).parameter'></a>

`parameter` [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object')

Volitelně parametr, který bude vložen do nalezené hlášky místo {PropertyValue}.

#### Returns
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

#### Exceptions

[System.NotImplementedException](https://docs.microsoft.com/en-us/dotnet/api/System.NotImplementedException 'System.NotImplementedException')  
ExceptionCode nebyl nalezen v Messages.