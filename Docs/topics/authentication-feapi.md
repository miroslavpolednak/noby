# Autentizace FE API

## Přihlášený uživatel - fyzická osoba používající FE NOBY
Instanci přihlášeného uživatele (tj. uživatele sedícího u NOBY aplikace) je možné získat z DI interfacem `ICurrentUserAccessor`.
Instance uživatele jako taková je reprezentována interfacem `ICurrentUser` - je dostupná jako vlastnost interface `ICurrentUserAccessor.User`.

Registrace `ICurrentUserAccessor` (do DI) probíhá během startupu:
```csharp
var app = builder.Build();
...
app.UseCisServiceUserContext();
```

## NEautentizovaný uživatel
Pokud není možné na FE API autentizovat uživatele (tj. chybí auth cookie, neplatný refresh token atd.), vrací jakýkoliv endpoit FE API chybu HTTP 401.  
V body tohoto response je následující struktura `NOBY.Infrastructure.Security.ApiAuthenticationProblemDetail`:

```json
{
	"RedirectUri": "string", 			// např. https://dev.noby.cz/auth/signin?redirect=
	"AuthenticationScheme": "string" 	// např. CaasAuthentication
}
```

- **AuthenticationScheme** je autentizační schéma, které je aktivní na aktuální instanci FE API. Může být: *CaasAuthentication*, *SimpleLoginAuthentication*.
- **RedirectUri** je URI na které by měl být uživatel přesměrován v případě schématu *CaasAuthentication*. Na konec tohoto stringu by měl frontend připojit URI na které se má uživatel přesměrovat po návratu z autentizační autority.

Předpokládáné chování frontendu:
- pro *CaasAuthentication*: přesměrování uživatele (location.href) na RedirectUri.
- pro *SimpleLoginAuthentication*: zobrazení přihlašovací dialogu s loginem uživatele.
