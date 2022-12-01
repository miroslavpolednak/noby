# Api projekt v gRPC službách
Implementace gRPC služby. Aplikace implementuje gRPC endpointy/metody z projektu `Contracts`.

## Technické řešení
Jedná se o konzolovou aplikaci běžící na serveru jako **Windows Service**. 
Aplikace na pozadí spouští **Kestrel**, který obsluhuje příchozí requesty.
Obsluha příchozích requestů je delegována pomocí [MediatR](https://github.com/jbogard/MediatR) na handlery - každý endpoint má vlastní request a handler.
Pokud to není z nějakého důvodu nutné, nejsou třídy v projektu vystavovány jako `public`, ale jsou `internal`.

## Verzování aplikace / kontraktu
Api projekt většinou obsahuje implementaci pouze jedné verze gRPC kontraktu. 
Pokud by to však bylo vyžadováno, může adresář Endpoints obsahovat obsluhu více verzí - v tom případě vznikají podadresáře V1, V2...

## Adresářová struktura projektu
Adresáře pro konkrétní endpointy obsahují vždy veškeré třídy potřebné pro daný endpoint.
Pokud existují společně sdílené třídy, např. společné DTO objekty, jsou tyto umístěny v adresářích *Dto/Services/Validators* v rootu aplikace.
Nikdy ale nereferencujeme třídy napříč endpointy. Je ale možné z jednoho handleru spustit jiný - tj. zřetězit endpointy za sebou.

Jmenná konvence pro adresáře s endpointy je následující:
- adresář: název metody z kontraktu v .proto souboru
- MediatR handler: `{název metody}Handler.cs`
- MediatR request: `{název metody}Request.cs`
- FluentValidation validátor: `{název metody}RequestValidation.cs`

```
[Configuration]				(adresář pro strongly typed konfigurace - pokud aplikace nemá speciální konfiguraci, adresář neexistuje)
  HouseholdConfiguration.cs     	(obraz konfigurace z appsettings.json)
[Database]                              (Entity Framework / Dapper)
  [Entities]                            (adresář s entitymi / tabulkami)
    Household.cs                        (EF entita)
    ...
  HouseholdServiceDbContext.cs          (EF DbContext, Dapper interface)
[Dto]                                   (společné DTO)
  Customer.cs
  ...
[Endpoints]                             (adresář s metodami gRPC služby)
  [GetHousehold]                        (gRPC metoda GetHousehold)
    [Dto]                               (DTO platné pouze pro tento endpoint)
      ...
    GetHouseholdRequest.cs              (MediatR request - v sobě má jako vlastnost referenci na gRPC request)
    GetHouseholdHandler.cs              (MediatR handler)
    GetHouseholdRequestValidator.cs     (FluentValidation validator pro request)
  [...]
[Extensions]
  StartupExtensions.cs	                (registrace komponent potřebných pro tuto službu do DI)
[Services]				(komponenty použitelné více endpointy)
  ...
[Validators]			        (globální FluentValidation validátory)
  ...
appsettings.json
kestrel.json                            (custom nastavení Kestrelu)
Program.cs
```

Ukázka Endpoints adresáře v případě implementace více verzí kontraktu:
```
[Endpoints]
  [V1]
    [GetHousehold]
      ...
  [V2]
    [GetHousehold]
      ...
```

## Konfigurace Kestrelu
Pro nastavení *Kestrelu* se používá vlastní konfigurační soubor `kestrel.json` - výchozí možnosti nastavení *Kestrelu* v *appsettings.json* nejsou (aspoň ne nyní) dostačující.
Tento konfigurační soubor obsahuje informace zejména o portu na kterém *Kestrel* naslouchá a o SSL certifikátu.
*Kestrel* může být nastaven na více portů a pro různé verze HTTP protokolu (1.1 a 2).

Příklad konfiguračního souboru:
```
{
  "CustomKestrel": {
    "Endpoints": [
      {
        "Port": 5005,           (port na kterém Kestrel naslouchá)
        "Protocol": 2           (protokol HTTP2)
      }
    ],
    "Certificate": {            (nastavení SSL certifikátu)
      "Location": "CertStore",
      "CertStoreName": "My",
      "CertStoreLocation": "LocalMachine",
      "Thumbprint": "2694C47172A2BB49985259915B747C2A2B3B8C1F"
    }
  }
}
```

## Konfigurace aplikace

### Nastavení logování
[Konfigurace logování je popsána zde.](logging.md)
```
"Serilog": { ... }
"CisTelemetry": { ... }
```

### Nastavení konzumace služeb třetích stran
[Konfigurace pro External Services je popsána zde.](external-services.md)
```
"ExternalServices": { ... }
```

### Nastavení databáze
Connection stringy do databází se konfigurují standardním .NET způsobem.
```
"ConnectionStrings": { ... }
```

### Nastavení autentizace služby
Příchozí requesty vždy procházejí autentizací. Druh autentizace lze nastavit na StaticCollectioni nebo ActiveDirectory.
```
"CisSecurity": {
    "ServiceAuthentication": {
        "Validator": "StaticCollection"
    }
}
```

### Nastavení ekosystému NOBY

```
"CisEnvironmentConfiguration": {
    "DefaultApplicationKey": "DS:HouseholdService",
    "EnvironmentName": "DEV",
    "ServiceDiscoveryUrl": "https://172.30.35.51:30000",
    "InternalServicesLogin": "a",
    "InternalServicePassword": "a"
}
```

### Vlastní konfigurace služby
Aplikace může obsahovat další, vlastní konfiguraci. V tom případě je tato konfigurace vždy v elementu **AppConfiguration**.
```
"AppConfiguration": { ... }
```