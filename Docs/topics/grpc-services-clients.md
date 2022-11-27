# Clients projekt v gRPC službách
Tento projekt slouží jako SDK pro mateřskou gRPC službu, ve finálním řešení bude distribuován jako NuGet package.  
Existuje proto, aby konzument služby nemusel mít znalost jak pracovat s gRPC.
Z pohledu konzumenta existuje pouze C# interface v DI reprezentující danou službu.
Zároveň, při správném nastavení **CIS** prostředí, konzument nemusí řešit autentizaci a umístění gRPC služby - to vše zajistí `Clients` NuGet.

## Technické řešení
Jedná se o implementaci gRPC klienta s použitím `Contracts` projektu dané služby.
NuGet vystavuje extension metodu pro registraci ve startupu volající aplikace.  
Tato extension metoda zaregistruje gRPC klienta a obecné Interceptory.
Zároveň se pokusí zavolat *ServiceDiscovery* a získat URL mateřské gRPC služby.

`Clients` projekt by neměl obsahovat žádnou byznys logiku, jedná se pouze o proxy mezi konzumentem a gRPC službou.
V některých případech ale může obsahovat např. vlastní kešování.

Jednotlivé metody rozhraní `Clients` projektu kopírují endpointy gRPC služby.
Metody v `Clients` projektu se musí jmenovat stejně, jako metody v gRPC službě. 
Nemusí ale přesně kopírovat jejich signatury - tj. pokud má request gRPC služby jednu nebo dvě vlastnosti, nemusí `Clients` metoda přijímat jako parametr daný request, ale může ho rozpadnout do dvou samostatných parametrů:
```
// v gRPC kontraktu
message MyRequest { int32 Id = 1; }

// v Clients metodě
Task MyEndpoint(int id) { }
// nebo
Task MyEndpoint(MyRequest request) { }
```

## Adresářová struktura
- adresář **Interfaces** : soubory s C# interfaces, které budou registrovány v DI. 
Kopírují 1:1 kontrakty gRPC služeb - jeden interface, jedna služba.
- adresář **Services** : implementace gRPC klienta a víše uvedeného interface.
- **xxxServiceExtensions.cs** : extension metoda pro registraci `Clients` projektu v aplikaci konzumenta.

## Použití v projektu konzumenta
Registrace v startupu aplikace:
```
builder.Services.AddHouseholdService();
```

Použití v endpoint handleru:
```
private readonly IHouseholdServiceClient _householdService;

public GetHouseholdHandler(IHouseholdServiceClient householdService)
{
    _householdService = householdService;
}

public async Task Handle(T request, CancellationToken cancellationToken)
{
    var household = await _householdService.GetHousehold(request.HouseholdId, cancellationToken);
}
```
