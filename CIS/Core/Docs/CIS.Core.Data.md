#### [CIS.Core](index.md 'index')

## CIS.Core.Data Namespace

Interfaces a implementace pro timestamp sloupce EF entit.

| Classes | |
| :--- | :--- |
| [BaseCreated](CIS.Core.Data.BaseCreated.md 'CIS.Core.Data.BaseCreated') | Implementace [ICreated](CIS.Core.Data.ICreated.md 'CIS.Core.Data.ICreated') |
| [BaseCreatedWithModifiedUserId](CIS.Core.Data.BaseCreatedWithModifiedUserId.md 'CIS.Core.Data.BaseCreatedWithModifiedUserId') | Implementace [IModifiedUser](CIS.Core.Data.IModifiedUser.md 'CIS.Core.Data.IModifiedUser') a [ICreated](CIS.Core.Data.ICreated.md 'CIS.Core.Data.ICreated') |
| [BaseIsActual](CIS.Core.Data.BaseIsActual.md 'CIS.Core.Data.BaseIsActual') | Implementace [IIsActual](CIS.Core.Data.IIsActual.md 'CIS.Core.Data.IIsActual') |
| [BaseModifiedUser](CIS.Core.Data.BaseModifiedUser.md 'CIS.Core.Data.BaseModifiedUser') | Implementace [IModifiedUser](CIS.Core.Data.IModifiedUser.md 'CIS.Core.Data.IModifiedUser') |

| Interfaces | |
| :--- | :--- |
| [IConnectionProvider](CIS.Core.Data.IConnectionProvider.md 'CIS.Core.Data.IConnectionProvider') | Marker interface pro Dapper. |
| [IConnectionProvider&lt;TRepository&gt;](CIS.Core.Data.IConnectionProvider_TRepository_.md 'CIS.Core.Data.IConnectionProvider<TRepository>') | Marker interface pro Dapper. |
| [ICreated](CIS.Core.Data.ICreated.md 'CIS.Core.Data.ICreated') | EF entita obsahuje sloupce s informací o uživateli, který ji vytvořil. |
| [IIsActual](CIS.Core.Data.IIsActual.md 'CIS.Core.Data.IIsActual') | EF entita obsahuje sloupec IsActual. |
| [IModifiedUser](CIS.Core.Data.IModifiedUser.md 'CIS.Core.Data.IModifiedUser') | EF entita obsahuje sloupce s informací o uživateli, který ji naposledy updatoval. |
