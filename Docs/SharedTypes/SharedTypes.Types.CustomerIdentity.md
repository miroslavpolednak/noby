#### [SharedTypes](index.md 'index')
### [SharedTypes.Types](SharedTypes.Types.md 'SharedTypes.Types')

## CustomerIdentity Class

Identita klienta

```csharp
public sealed class CustomerIdentity
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; CustomerIdentity
### Properties

<a name='SharedTypes.Types.CustomerIdentity.Id'></a>

## CustomerIdentity.Id Property

ID klienta v danem schematu

```csharp
public long Id { get; set; }
```

#### Property Value
[System.Int64](https://docs.microsoft.com/en-us/dotnet/api/System.Int64 'System.Int64')

<a name='SharedTypes.Types.CustomerIdentity.Scheme'></a>

## CustomerIdentity.Scheme Property

Schema ve kterem je klient ulozeny - Kb | Mp

```csharp
public SharedTypes.Enums.IdentitySchemes Scheme { get; set; }
```

#### Property Value
[SharedTypes.Enums.IdentitySchemes](https://docs.microsoft.com/en-us/dotnet/api/SharedTypes.Enums.IdentitySchemes 'SharedTypes.Enums.IdentitySchemes')