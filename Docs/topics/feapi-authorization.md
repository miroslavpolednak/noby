# Autorizace uživatele FE API
Každý uživatel aplikace *NOBY* má definovánu sadu práv (**permissions**), která je reprezentována jako `int[]`.
Tato sada práv se vrací ve vlastnosti `UserPermissions` doménové služby `UserService.GetUser()`.

Každé právo je identifikováno číslem a názvem. 
Na FE API používáme enum `DomainServices.UserService.Clients.Authorization.UserPermissions`, který reprezentuje čísla jednotlivých práv a zároveň jejich název.
S každým nově definovaným právem je třeba tento enum rozšířit o novou položku.

Pokud nemá na danou akci právo, FE API vrátí HTTP 403.

```csharp
namespace DomainServices.UserService.Clients.Authorization;

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
Pokud je použito více atributů na jeden endpoint, je vyžadováno alespoň jedno právo z každého atributu.

```csharp
// je vyžadováno právo UC_getWflSigningAttachments
[NobyAuthorize(UserPermissions.UC_getWflSigningAttachments)]
public async Task T1() { }

// je vyžadováno právo UC_getWflSigningAttachments NEBO CASEDETAIL_APPLICANT_ViewPersonInfo
[NobyAuthorize(UserPermissions.UC_getWflSigningAttachments, UserPermissions.CASEDETAIL_APPLICANT_ViewPersonInfo)]
public async Task T1() { }

// je vyžadováno právo UC_getWflSigningAttachments A CASEDETAIL_APPLICANT_ViewPersonInfo
[NobyAuthorize(UserPermissions.UC_getWflSigningAttachments)]
[NobyAuthorize(UserPermissions.CASEDETAIL_APPLICANT_ViewPersonInfo)]
public async Task T1() { }
```

Požadavek na permission na úrovni endopointu je automaticky generován do Swagger UI jako část popisu endpointu.

## 3. permission check v byznys logice
`DomainService.UserService.Clients` obsahuje extension metody, které umožňují snadný permission check.
Tyto metody jsou použitelné jak z FE API, tak z doménových služeb.

```csharp
static bool HasPermission(this Contracts.User user, Authorization.UserPermissions permission);
static bool HasPermission(this Contracts.User user, int permission);
```

Specificky pro FE API existují další možnosti ověření permissions, extension metody v namespace `NOBY.Infrastructure.Security`.

```csharp
static bool HasPermission(this ClaimsPrincipal principal, DomainServices.UserService.Clients.Authorization.UserPermissions permission);
static bool HasPermission(this ClaimsPrincipal principal, int permission);
static bool HasPermission(this ICurrentUserAccessor userAccessor, DomainServices.UserService.Clients.Authorization.UserPermissions permission);
static bool HasPermission(this ICurrentUserAccessor userAccessor, int permission);

// metody, které ověří práva a v případě neexistujícího oprávnění vyvolají vyjímku CisAuthorizationException
static void CheckPermission(this ClaimsPrincipal principal, DomainServices.UserService.Clients.Authorization.UserPermissions permission);
static void CheckPermission(this ClaimsPrincipal principal, int permission);
static void CheckPermission(this ICurrentUserAccessor userAccessor, DomainServices.UserService.Clients.Authorization.UserPermissions permission);
static void CheckPermission(this ICurrentUserAccessor userAccessor, int permission);
```
