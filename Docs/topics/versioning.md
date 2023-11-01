# Versioning & GIT branching

## GIT branching

### Production flow
Existuje pouze jeden branch s produkční verzí aplikace - `production`.

V této větvy vznikají *tagy*, které označují jednotlivé fix version aplikace. Je tedy možné se kdykoliv vrátit / nasadit ke konkrétní fix version.
Ve chvíli nasazení nové verze se poslední commit označí tagem předchozí fix version, tím tedy fakticky vzniká historie nasazovaných změn.

**Hotfix bugu:**
1) vytvoření bugfix branch z `production` branch - `bugfix-prod/HFICH-XXXX`
2) oprava chyby / vytvoření nové feature
3) otestování aplikace nasazením branch `bugfix-prod/HFICH-XXXX` na testovací prostředí
4) merge branch `bugfix-prod/HFICH-XXXX` do `production`
5) nasazení `production` branch na server

Pokud se má stejná chyba opravit i v budoucí / dev verzi, je na zvážení programátora zda udělat cherry pick `bugfix-prod/HFICH-XXXX` do `master` nebo daný bug opravit v `master` samostatně.

### Development flow
Existuje pouze jeden branch do kterého se vyvíjí - `master`.

Ve chvíli, kdy je kód připraven na release do produkce, vytvoří se z master nový branch - `release`.
`release` branch se testuje na vlastním prostředí.
Pokud budou nalezeny chyby, opravují se jak do `release`, tak do `master` branch.
Pokud by se stalo, že oprava chyby není triviální nebo bude potřeba dodělat novou funkčnost, je na zvážení, zda aktuální `release` nezahodit a vytvořit nový `release` z aktuální verze `master` kde již daná funkčnost/bug bude opravený.

**Vývoj nového JIRA tasku:**
1) vytvoření feature branch z `master` - `feature/HFICH-XXXX`
2) vývoj v nově vytvořené branch
3) merge `feature/HFICH-XXXX` do `master`

**Bugfix na `release`**
1) oprava chyby na `master` branch formou commitu - v commit message bude ID bugu
2) cherry pick daného commitu do `release`

**Nasazení `release` do produkce**
1) *[optional]* nastavení tagu fix version na poslední commit v `production`
2) merge `release` branch do `production` branch
3) vyřešení případných merge conflicts
4) nasazení nové verze `production` na testovací prostředí
5) nasazení otestovaného buildu na produkci

![NOBY GIT FLOW](./NOBY-GIT-FLOW.png)

## Verzování endpointů FE API (NOBY.Api)
Na FE API používáme verzování jednotlivých endpointů místo verze celého API. 
To znamená, že jednotlivé endpointy mohou mít v jednu chvíli každý jinou verzi, nicméně spolu budou fungovat.
Novou verzi vytváříme pouze v případě, že se mění byznys logika za endpointem nebo se mění kontrakt takovým způsobem, že FE danou změnu nedokáže zpracovat.

> Snažíme se pokud možno vytvářet co nejméně nových verzí endpointů.

Tento způsob verzování jsme zvolili protože:
a) potřebujeme umožnit nezávislý vývoj BE a FE
b) z praktických důvodu nelze s každou změnou vytvářet novou verzi celého API

Staré verze endpointů nemažeme dokud nebude nasazena kompaktibilní verze FE na produkci a nebude možnost smazání potvrzena ITA. 
Do té doby jsou staré verze endpointů odekorovány atributem `[Obsolete]`.

Ukázka side-by-side verzí endpointů:
```
v1/GetCase
...
v2/GetCustomer              [Obsolete]
v3/GetCustomer              [Obsolete]
v4/GetCustomer
(v1 already deleted)
...
v1/UpdateCustomer           [Obsolete]
v2/UpdateCustomer
```

### Technické zpracování verzování na FE API
Infrastruktura verzování je zajištěna knihovnou [Microsoft.AspNetCore.Mvc.Versioning](https://github.com/dotnet/aspnet-api-versioning).
Požadovaná verze konkrétního endpointu je při requestu specifikovaná v HTTP headeru **X-Api-Version**.

### Nastavení verzí v controllerech
```csharp
[ApiController]
public class WeatherController
{
    [ApiVersion("v1", Deprecated = true)]
    public async Task Get() { ... }

    [ApiVersion("v2")]
    public async Task GetV2() { ... }
}
```

### Přidání nové verze do projektu
TODO

## Verzování endpointů doménových služeb
U doménových služeb se s každou změnou i jediného endpointu vytváří nová verze celého API.
Tj. pokud změním kontrakt endpointu GetDetail a vytvořím *v2*, tak automaticky vystavujeme všechny ostatní endpointy v nové verzi (*v2*).
Technicky to znamená pouze provolání stejných *Mediatr* handlerů, takže overhead s vytvořením nové verze celého API není tak velký.

### Vytvoření nové verze API
Vytvoření nové verze API znamená vytvoření nové verze gRPC služby.

**Api project**

1) přidání nové controlleru nové služby
```
DomainServices.CaseService.Api      (project)
  Endpoints                         (endpoints folder)
    CaseServiceV1.cs                (old service version)
    CaseServiceV2.cs                (new service version)
```

2) registrace endpointů v `Program.cs`
```csharp
SharedComponents.GrpcServiceBuilder
...
.MapGrpcServices(app =>
{
    app.MapGrpcService<DomainServices.CaseService.Api.Endpoints.CaseServiceV1>();
    app.MapGrpcService<DomainServices.CaseService.Api.Endpoints.CaseServiceV2>();
})
...
```

**Clients project**

TODO

**Contracts project**
```
Messages
  V2
    GetCaseV2.proto         (V2 specific request and response contracts)
    ...
  GetCase.proto             (default - V1 - request and response contracts)
  ...
CaseService.v1.proto        (V1 service descriptor)
CaseService.v2.proto        (V2 service descriptor)
```

## Adresářová struktura multi version endpointů
V NOBY.API i v doménových službách jsou jednotlivé verze vytvořené jako podadresáře vždy v hlavním adresáři daného endpointu.
Zároveň v hlavním adresáři endpointu mohou být umístěné sdílené komponenty daného endpointu (pokud nějaké jsou).

```
Endpoints                               (all endpoints folder)
  Customer				(tag folder - endpoints group)
    GetCustomer			        (feature folder for multi-version endpoint)
      GetCustomerRequest.cs	        (shared request contract)
      V1                                (version folder)
        GetCustomerHandler.cs
        GetCustomerResponse.cs
      V2                                (version folder)
        GetCustomerHandler.cs
        GetCustomerResponse.cs
    ...
    UpdateCustomer                      (feature folder for single version endpoint)
      UpdateCustomerHandler.cs
      UpdateCustomerRequest.cs
```
