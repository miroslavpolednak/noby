#### [CIS.Core](index.md 'index')

## CIS.Core.Exceptions Namespace

Custom vyjímky nahrazující standardní exceptions v .NETu. Snažíme se používat vlastní vyjímky, abychom dokázali rozeznat o jakou chybu se jedná a jak na ni reagovat.

| Classes | |
| :--- | :--- |
| [BaseCisException](CIS.Core.Exceptions.BaseCisException.md 'CIS.Core.Exceptions.BaseCisException') | Base třída pro CIS vyjímky. Obsahuje vlastnost ExceptionCode, která určuje o jakou vyjímku se jedná. |
| [CisAlreadyExistsException](CIS.Core.Exceptions.CisAlreadyExistsException.md 'CIS.Core.Exceptions.CisAlreadyExistsException') | Objekt již existuje. |
| [CisAuthenticationException](CIS.Core.Exceptions.CisAuthenticationException.md 'CIS.Core.Exceptions.CisAuthenticationException') | Chyba, která označuje problém s validací koncového uživatele. |
| [CisConfigurationException](CIS.Core.Exceptions.CisConfigurationException.md 'CIS.Core.Exceptions.CisConfigurationException') | Pokud nastavení konfigurace v appsettings.json pro danou konfigurační sekci nelze zvalidovat - tj. chybí nastavit některá z required props atd. |
| [CisConfigurationNotFound](CIS.Core.Exceptions.CisConfigurationNotFound.md 'CIS.Core.Exceptions.CisConfigurationNotFound') | Pokud chybí požadované nastavení konfigurace v appsettings.json |
| [CisConfigurationNotRegisteredException](CIS.Core.Exceptions.CisConfigurationNotRegisteredException.md 'CIS.Core.Exceptions.CisConfigurationNotRegisteredException') | Chyba v pripade, kdy nelze v DI najit instanci konfigurace CIS |
| [CisConflictException](CIS.Core.Exceptions.CisConflictException.md 'CIS.Core.Exceptions.CisConflictException') | HTTP 409. Vyhazovat pokud prováděná akce je v konfliktu s existující byznys logikou. Podporuje kolekci chybových hlášení. |
| [CisException](CIS.Core.Exceptions.CisException.md 'CIS.Core.Exceptions.CisException') | Stejná chyba jako [System.Exception](https://docs.microsoft.com/en-us/dotnet/api/System.Exception 'System.Exception'), ale obsahuje navíc CIS error kód |
| [CisExceptionItem](CIS.Core.Exceptions.CisExceptionItem.md 'CIS.Core.Exceptions.CisExceptionItem') | Instance jednotlive chyby |
| [CisInvalidApplicationKeyException](CIS.Core.Exceptions.CisInvalidApplicationKeyException.md 'CIS.Core.Exceptions.CisInvalidApplicationKeyException') | Chyba validace názvu aplikace - vyvoláno z konstruktoru value type ApplicationKey |
| [CisInvalidEnvironmentNameException](CIS.Core.Exceptions.CisInvalidEnvironmentNameException.md 'CIS.Core.Exceptions.CisInvalidEnvironmentNameException') | Chyba validace názvu prostředí - vyvoláno z konstruktoru value type EnvironmentName |
| [CisNotFoundException](CIS.Core.Exceptions.CisNotFoundException.md 'CIS.Core.Exceptions.CisNotFoundException') | Objekt nebyl nalezen. |
| [CisServiceServerErrorException](CIS.Core.Exceptions.CisServiceServerErrorException.md 'CIS.Core.Exceptions.CisServiceServerErrorException') | HTTP 500. Vyhazuje se pokud naše doménová nebo infrastrkuturní služba vrátí server error - 500. |
| [CisServiceUnavailableException](CIS.Core.Exceptions.CisServiceUnavailableException.md 'CIS.Core.Exceptions.CisServiceUnavailableException') | Doménová nebo infrastrukturní služba není k dispozici - např. špatné URL volané služby, nebo volaná služba vůbec neběží. |
| [CisValidationException](CIS.Core.Exceptions.CisValidationException.md 'CIS.Core.Exceptions.CisValidationException') | Validační chyba. |
### Structs

<a name='CIS.Core.Exceptions.ExceptionHandlingConstants'></a>

## ExceptionHandlingConstants Struct

```csharp
public struct ExceptionHandlingConstants
```
### Fields

<a name='CIS.Core.Exceptions.ExceptionHandlingConstants.GrpcTrailerCisArgumentKey'></a>

## ExceptionHandlingConstants.GrpcTrailerCisArgumentKey Field

Klíč v gRPC Trailers kolekci, který označuje záznam s názvem argumentu, který vyvolal vyjímku (CisArgumentException)

```csharp
public const string GrpcTrailerCisArgumentKey = argument;
```

#### Field Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

<a name='CIS.Core.Exceptions.ExceptionHandlingConstants.GrpcTrailerCisCodeKey'></a>

## ExceptionHandlingConstants.GrpcTrailerCisCodeKey Field

Klíč v gRPC Trailers kolekci, který označuje záznam s CIS error kódy

```csharp
public const string GrpcTrailerCisCodeKey = ciscode;
```

#### Field Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')