#### [CIS.Core](index.md 'index')

## CIS.Core.Security Namespace

Interfaces pro získání informací o přihlášeném uživatel a technickém uživateli služby.

| Classes | |
| :--- | :--- |
| [SecurityConstants](CIS.Core.Security.SecurityConstants.md 'CIS.Core.Security.SecurityConstants') | Konstanty pro nastavení auth providerů atd. |

| Interfaces | |
| :--- | :--- |
| [ICurrentUser](CIS.Core.Security.ICurrentUser.md 'CIS.Core.Security.ICurrentUser') | Instance aktuálně přihlášeného fyzického uživatele |
| [ICurrentUserAccessor](CIS.Core.Security.ICurrentUserAccessor.md 'CIS.Core.Security.ICurrentUserAccessor') | Helper pro ziskani akltuálně přihlášeného fyzickeho uzivatele, ktery aplikaci/sluzbu vola |
| [ICurrentUserDetails](CIS.Core.Security.ICurrentUserDetails.md 'CIS.Core.Security.ICurrentUserDetails') | Další informace o uživateli. |
| [IServiceUser](CIS.Core.Security.IServiceUser.md 'CIS.Core.Security.IServiceUser') | Informace o technickem uzivateli pod kterym je vytvoren request na interni sluzbu |
| [IServiceUserAccessor](CIS.Core.Security.IServiceUserAccessor.md 'CIS.Core.Security.IServiceUserAccessor') | Helper pro ziskani instance technickeho uzivatele, pod kterym je spusten request na interni sluzbu. |
