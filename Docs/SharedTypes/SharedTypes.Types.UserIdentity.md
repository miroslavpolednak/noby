#### [SharedTypes](index.md 'index')
### [SharedTypes.Types](SharedTypes.Types.md 'SharedTypes.Types')

## UserIdentity Class

Identita uživatele NOBY aplikace

```csharp
public sealed class UserIdentity
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; UserIdentity
### Constructors

<a name='SharedTypes.Types.UserIdentity.UserIdentity(string,SharedTypes.Enums.UserIdentitySchemes)'></a>

## UserIdentity(string, UserIdentitySchemes) Constructor

```csharp
public UserIdentity(string identity, SharedTypes.Enums.UserIdentitySchemes scheme);
```
#### Parameters

<a name='SharedTypes.Types.UserIdentity.UserIdentity(string,SharedTypes.Enums.UserIdentitySchemes).identity'></a>

`identity` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

ID uživatele

<a name='SharedTypes.Types.UserIdentity.UserIdentity(string,SharedTypes.Enums.UserIdentitySchemes).scheme'></a>

`scheme` [SharedTypes.Enums.UserIdentitySchemes](https://docs.microsoft.com/en-us/dotnet/api/SharedTypes.Enums.UserIdentitySchemes 'SharedTypes.Enums.UserIdentitySchemes')

Identitní schéma

#### Exceptions

[CIS.Core.Exceptions.CisArgumentException](https://docs.microsoft.com/en-us/dotnet/api/CIS.Core.Exceptions.CisArgumentException 'CIS.Core.Exceptions.CisArgumentException')  
ID není zadáno

<a name='SharedTypes.Types.UserIdentity.UserIdentity(string,string)'></a>

## UserIdentity(string, string) Constructor

```csharp
public UserIdentity(string? identity, string? scheme);
```
#### Parameters

<a name='SharedTypes.Types.UserIdentity.UserIdentity(string,string).identity'></a>

`identity` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

ID uživatele

<a name='SharedTypes.Types.UserIdentity.UserIdentity(string,string).scheme'></a>

`scheme` [System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

Identitní schéma

#### Exceptions

[CIS.Core.Exceptions.CisArgumentException](https://docs.microsoft.com/en-us/dotnet/api/CIS.Core.Exceptions.CisArgumentException 'CIS.Core.Exceptions.CisArgumentException')  
ID nebo schéma není zadáno
### Properties

<a name='SharedTypes.Types.UserIdentity.Identity'></a>

## UserIdentity.Identity Property

ID uživatele

```csharp
public string Identity { get; set; }
```

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

<a name='SharedTypes.Types.UserIdentity.Scheme'></a>

## UserIdentity.Scheme Property

Identitní schéma

```csharp
public SharedTypes.Enums.UserIdentitySchemes Scheme { get; set; }
```

#### Property Value
[SharedTypes.Enums.UserIdentitySchemes](https://docs.microsoft.com/en-us/dotnet/api/SharedTypes.Enums.UserIdentitySchemes 'SharedTypes.Enums.UserIdentitySchemes')