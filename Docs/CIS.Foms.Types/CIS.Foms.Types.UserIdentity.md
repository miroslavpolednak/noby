#### [CIS.Foms.Types](index.md 'index')
### [CIS.Foms.Types](CIS.Foms.Types.md 'CIS.Foms.Types')

## UserIdentity Class

Identita uživatele NOBY aplikace

```csharp
public sealed class UserIdentity
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; UserIdentity
### Constructors

<a name='CIS.Foms.Types.UserIdentity.UserIdentity(string,CIS.Foms.Enums.UserIdentitySchemes)'></a>

## UserIdentity(string, UserIdentitySchemes) Constructor

```csharp
public UserIdentity(string identity, CIS.Foms.Enums.UserIdentitySchemes scheme);
```
#### Parameters

<a name='CIS.Foms.Types.UserIdentity.UserIdentity(string,CIS.Foms.Enums.UserIdentitySchemes).identity'></a>

`identity` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

ID uživatele

<a name='CIS.Foms.Types.UserIdentity.UserIdentity(string,CIS.Foms.Enums.UserIdentitySchemes).scheme'></a>

`scheme` [CIS.Foms.Enums.UserIdentitySchemes](https://docs.microsoft.com/en-us/dotnet/api/CIS.Foms.Enums.UserIdentitySchemes 'CIS.Foms.Enums.UserIdentitySchemes')

Identitní schéma

#### Exceptions

[CIS.Core.Exceptions.CisArgumentException](https://docs.microsoft.com/en-us/dotnet/api/CIS.Core.Exceptions.CisArgumentException 'CIS.Core.Exceptions.CisArgumentException')  
ID není zadáno

<a name='CIS.Foms.Types.UserIdentity.UserIdentity(string,string)'></a>

## UserIdentity(string, string) Constructor

```csharp
public UserIdentity(string? identity, string? scheme);
```
#### Parameters

<a name='CIS.Foms.Types.UserIdentity.UserIdentity(string,string).identity'></a>

`identity` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

ID uživatele

<a name='CIS.Foms.Types.UserIdentity.UserIdentity(string,string).scheme'></a>

`scheme` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Identitní schéma

#### Exceptions

[CIS.Core.Exceptions.CisArgumentException](https://docs.microsoft.com/en-us/dotnet/api/CIS.Core.Exceptions.CisArgumentException 'CIS.Core.Exceptions.CisArgumentException')  
ID nebo schéma není zadáno
### Properties

<a name='CIS.Foms.Types.UserIdentity.Identity'></a>

## UserIdentity.Identity Property

ID uživatele

```csharp
public string Identity { get; set; }
```

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

<a name='CIS.Foms.Types.UserIdentity.Scheme'></a>

## UserIdentity.Scheme Property

Identitní schéma

```csharp
public CIS.Foms.Enums.UserIdentitySchemes Scheme { get; set; }
```

#### Property Value
[CIS.Foms.Enums.UserIdentitySchemes](https://docs.microsoft.com/en-us/dotnet/api/CIS.Foms.Enums.UserIdentitySchemes 'CIS.Foms.Enums.UserIdentitySchemes')