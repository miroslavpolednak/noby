#### [CIS.Core](index.md 'index')
### [CIS.Core.Data](CIS.Core.Data.md 'CIS.Core.Data')

## IConnectionProvider<TRepository> Interface

Marker interface pro Dapper.

```csharp
public interface IConnectionProvider<TRepository> :
CIS.Core.Data.IConnectionProvider
```
#### Type parameters

<a name='CIS.Core.Data.IConnectionProvider_TRepository_.TRepository'></a>

`TRepository`

Implements [IConnectionProvider](CIS.Core.Data.IConnectionProvider.md 'CIS.Core.Data.IConnectionProvider')

### Remarks
TRepository je connection string (ve formě třídy/interface) pro který je daný marker uložený v DI. Existuje proto, aby bylo možné používat v jedné aplikaci více různých connection stringů.