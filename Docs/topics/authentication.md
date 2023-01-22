# Autentizace FE a doménových služeb

## Přihlášený uživatel - fyzická osoba používající FE NOBY
Instanci přihlášeného uživatele (tj. uživatele sedícího u NOBY aplikace) je možné získat z DI interfacem `ICurrentUserAccessor`.
Instance uživatele jako taková je reprezentována interfacem `ICurrentUser` - je dostupná jako vlastnost interface `ICurrentUserAccessor.User`.

Mezi službami se uživatel předává ve formě HTTP headeru "**mp-user-id**", který obsahuje *v33id* aktuálního uživatele.

Registrace `ICurrentUserAccessor` (do DI) probíhá během startupu:
```csharp
var app = builder.Build();
...
app.UseCisServiceUserContext();
```


## Technický uživatel - interservisní komunikace
Každý klient doménových služeb se musí autentizovat technickým uživatelem.
Autentizace probíhá pomocí standardní **Basic authentication** - tj. HTTP header "**Authorization: Basic [Base64_login|heslo]**".

Informace o technickém uživateli je možné ve volané službě získat z DI interfacem `IServiceUserAccessor`.
Instance uživatele je potom implementací `IServiceUser` - je dostupná jako vlastnost `IServiceUserAccessor.User`.

Nastavení autentizace technickým uživatelem a registrace interface do DI probíhá během startupu:
```csharp
builder.AddCisServiceAuthentication();
...
var app = builder.Build();
...
app.UseAuthentication();
app.UseAuthorization();
```

### Nastavení technického uživatele pod kterým služba volá ostatní služby
Nastavení (login a heslo) pro technického uživatele pod kterým se daná služba autentizuje vůči ostatním službám v ekosystému NOBY je v *appsettings.json*, v sekci **CisEnvironmentConfiguration**.
V této sekci jsou vlastnosti `InternalServicesLogin` a `InternalServicePassword`, které identifikují technického uživatele.  
Automatická autentizace vůči ostatním službám funguje pouze v případě využití **Clients** projektu dané služby v **ServiceDiscovery** módu.  
Login a heslo technického uživatele by měla doplňovat CI/CD pipeline, nesmí být uvedeno v GITu.

```json
"CisEnvironmentConfiguration": {
  "DefaultApplicationKey": "DS:CaseService",
  "EnvironmentName": "FAT",
  "ServiceDiscoveryUrl": "https://172.30.35.51:31000",
  "InternalServicesLogin": "a",
  "InternalServicePassword": "a"
},
```

Forwardování credentials technického uživatele probíhá pomocí extension metody `CIS.Infrastructure.gRPC.AddCisCallCredentials()`.  
Tato metoda je volána automaticky v rámci registrace gRPC klienta pomocí `TryAddCisGrpcClientUsingServiceDiscovery()` nebo `TryAddCisGrpcClientUsingUrl()`.

### Autentizace technického uživatele při volání doménové služby
Doménová služba musí ověřit credentials technického uživatele příchozího requestu.
Infrastruktura služeb NOBY poskytuje možnost automatického ověření/nastavení technického uživatele registrací middleware pomocí extension metody `AddCisServiceAuthentication` během startupu aplikace.

```csharp
using CIS.Infrastructure.Security;
...
builder.AddCisServiceAuthentication();
```

Aby registrace mohla fungovat, musí být korektně nastavena sekce **CisSecurity** v *appsettings.json*.
Tato sekce má obraz v konfigurační třídě `CIS.Infrastructure.Security.Configuration.CisServiceAuthenticationConfiguration`, kde je i popis jednotlivých vlastností.

Aktuálně podporujeme dva režimy autentizace technického uživatele:
- **statická kolekce**: hardcoded kolekce uživatelů/hesel. Tento způsob používáme pro testování a na localhostu.
- **active directory**: ověření uživatele vůči zadanému AD.

```json
"CisSecurity": {
  "ServiceAuthentication": {
    "Validator": "StaticCollection"
  }
}
```
