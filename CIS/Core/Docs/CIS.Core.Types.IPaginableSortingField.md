#### [CIS.Core](index.md 'index')
### [CIS.Core.Types](CIS.Core.Types.md 'CIS.Core.Types')

## IPaginableSortingField Interface

Nastavení jednoho pole pro řazení stránkovacích requestů.

```csharp
public interface IPaginableSortingField
```
### Properties

<a name='CIS.Core.Types.IPaginableSortingField.Descending'></a>

## IPaginableSortingField.Descending Property

Nastavení řazení sestupně nebo vzestupně.

```csharp
bool Descending { get; }
```

#### Property Value
[System.Boolean](https://docs.microsoft.com/en-us/dotnet/api/System.Boolean 'System.Boolean')

<a name='CIS.Core.Types.IPaginableSortingField.Field'></a>

## IPaginableSortingField.Field Property

Název pole.

```csharp
string Field { get; }
```

#### Property Value
[System.String](https://docs.microsoft.com/en-us/dotnet/api/System.String 'System.String')

### Remarks
Nemusí se shodovat s názvem databázového pole.