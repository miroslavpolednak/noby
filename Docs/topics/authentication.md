# Autentizace

## Informace o přihlášeném uživateli
Instanci přihlášeného uživatele (tj. uživatele sedícího u NOBY aplikace) je možné získat z DI interfacem **ICurrentUserAccessor**.
Instance uživatele jako taková je reprezentována interfacem **ICurrentUser**.

Mezi službami se uživatel předává ve formě HTTP headeru "**mp-user-id**", který obsahuje v33id aktuálního uživatele.

Registrace *ICurrentUserAccessor* (do DI) probíhá během startupu:
```
var app = builder.Build();
...
app.UseCisServiceUserContext();
```


## Informace technickém uživateli, který volá službu
Každý klient doménových služeb se musí autentizovat AD technickým uživatelem. 
Autentizace probíhá pomocí standardní Basic authentication - tj. HTTP header "Authorization".

Informace o technickém uživateli je možné ve volané službě získat z DI interfacem **IServiceUserAccessor**.
Instance uživatele je potom implementací **IServiceUser**. 

Nastavení autentizace technickým uživatelem a registrace interface do DI probíhá během startupu:
```
builder.AddCisServiceAuthentication();
...
var app = builder.Build();
...
app.UseAuthentication();
app.UseAuthorization();
```

