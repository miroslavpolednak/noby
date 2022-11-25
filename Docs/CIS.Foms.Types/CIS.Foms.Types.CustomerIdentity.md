#### [CIS.Foms.Types](index.md 'index')
### [CIS.Foms.Types](CIS.Foms.Types.md 'CIS.Foms.Types')

## CustomerIdentity Class

Identita klienta

```csharp
public sealed class CustomerIdentity
```

Inheritance [System.Object](https://docs.microsoft.com/en-us/dotnet/api/System.Object 'System.Object') &#129106; CustomerIdentity
### Properties

<a name='CIS.Foms.Types.CustomerIdentity.Id'></a>

## CustomerIdentity.Id Property

ID klienta v danem schematu

```csharp
public long Id { get; set; }
```

#### Property Value
[System.Int64](https://docs.microsoft.com/en-us/dotnet/api/System.Int64 'System.Int64')

<a name='CIS.Foms.Types.CustomerIdentity.Scheme'></a>

## CustomerIdentity.Scheme Property

Schema ve kterem je klient ulozeny - Kb | Mp

```csharp
public CIS.Foms.Enums.IdentitySchemes Scheme { get; set; }
```

#### Property Value
[CIS.Foms.Enums.IdentitySchemes](https://docs.microsoft.com/en-us/dotnet/api/CIS.Foms.Enums.IdentitySchemes 'CIS.Foms.Enums.IdentitySchemes')