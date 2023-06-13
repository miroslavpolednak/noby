# Autorizace uživatele FE API
Každý uživatel aplikace *NOBY* má definovánu sadu práv (**permissions**), která je reprezentována jako `int[]`.
Tato sada práv se vrací ve vlastnosti `UserPermissions` doménové služby `UserService.GetUser()`.

Každé právo je identifikováno číslem a názvem. 
Na FE API používáme enum `DomainServices.UserService.Clients.Authorization.UserPermissions`, který reprezentuje čísla jednotlivých práv a zároveň jejich název.
S každým nově definovaným právem je třeba tento enum rozšířit o novou položku.

```csharp
namespace DomainServices.UserService.Clients.Authorization;
[Flags]
public enum UserPermissions : int
{
    APPLICATION_BasicAccess = 201,
    ...
}
```

### Jak získat seznam práv uživatele?
1) v doménové službě zavoláním `UserService.GetUser()`
2) v FE API jsou všechna práva uživatele uložena jako *Claims* s názvem "**NP**"

```csharp
// kontrola existence práva v Controlleru
this.User.HasClaim(t => 
    t.Type == NOBY.Infrastructure.Security.AuthenticationConstants.NobyPermissionClaimType 
    && t.Value == "101"
);

// kontrola existence práva v Handleru
private readonly IHttpContextAccessor _context;
...
_context.HttpContext.User.HasClaim(t => 
    t.Type == NOBY.Infrastructure.Security.AuthenticationConstants.NobyPermissionClaimType 
    && t.Value == "101"
);
```

### Autorizace uživatele na FE API probíhá na třech úrovních:
1) globální permission check na všech endpointech, který kontroluje zda má uživatel právo na aplikaci.
2) permission check na úrovni endpointu.
3) permission check v byznys logice handleru.

## 1. globální permission check
Aby uživatel mohl pracovat s aplikací *NOBY*, musí mít minimálně právo **201** (APPLICATION_BasicAccess). 
Pokud toto právo nemá, jakýkoliv endpoint FE API musí vrátit HTTP 403.

Kontrola na toto právo probíhá v middleware `NOBY.Infrastructure.Security.NobySecurityMiddleware`.

## 2. permission check na úrovni endpointu
Pokud je endpoint FE API dostupný pouze pro uživatele s definovanými permissions, je nutné daný endpoint odekorovat atributem `NOBY.Infrastructure.Security.NobyAuthorizeAttribute` společně s informací o tom, jaké právo/a je/jsou vyžadováno/a. 
Atributů `NobyAuthorizeAttribute` může být pro jeden endpoint použito více pokud je vyžádováno více oprávnění najednou.
