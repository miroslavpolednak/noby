# Jak funguje ServiceDiscovery
Service Discovery (SD) je interní gRPC služba sloužící jako databáze URI všech zdrojů v ekosystému NOBY.
Tato databáze obsahuje adresy všech doménových, interních a externích (třetích stran) služeb, zároveň adresy dalších zdrojů jako např. Redis Cache atd.
Adresy jsou v databázi uložené pro každé aplikační prostředí NOBY.

Ačkoliv to umožňujeme, tak ale nechceme, aby každá naše služba musela v konfiguraci držet adresy všech ostatních služeb, na kterých je závislá.
K tomu právě slouží SD, která konzumentovi poskytne adresář "kontaktů", které může volat.

Služba SD se dá volat explicitně - jedná se o standardní gRPC službu v našem systému, v budoucnu bude pravděpodobně poskytovat i REST rozhraní pro ostatní konzumenty.
Pro naše aplikace/služby je ale preferovanou variantou načítat URI automaticky na úrovni SDK Clients / ExternalServices projektů.

## Nastavení automatického resolvingu adres
Aby automatický resolving adres v aplikaci fungoval, musí být konzument správně nakonfigurován - tj. mít v DI instanci `ICisEnvironmentConfiguration`.

Konfigurace se načítá z *appsettings.json* a je popsána [zde](grpc-services-api.md).
```
"CisEnvironmentConfiguration": {
	"DefaultApplicationKey": "DS:CaseService",
	"EnvironmentName": "FAT",
	"ServiceDiscoveryUrl": "https://172.30.35.51:31000",
	"InternalServicesLogin": "a",
	"InternalServicePassword": "a"
}
```
Při startupu se pak načíta a vkládá do DI pomocí této extension metody:
```
builder.AddCisEnvironmentConfiguration();
```
Dále je potřeba při startupu aplikace spustit automatický resolving:
```
app.UseServiceDiscovery();
```
`UseServiceDiscovery()` prochází všechny singleton instance v DI, které implementují interface `CIS.Core.IIsServiceDiscoverable` a zároveň požadují automatický resolving (`UseServiceDiscovery`=true).
Pokud takové najde, zavolá SD (jejíž adresu zná z `ICisEnvironmentConfiguration`) a takto získané adresy doplní do nalezených singleton objektů.

Teoreticky může do DI vložit třídu `IIsServiceDiscoverable` cokoliv, nicméně v praxi tento postup budou využívat hlavně SDK doménových služeb a SDK služeb třetích stran.

### Resolving adres doménových služeb
Konzument referencuje SDK požadované služby (`Clients` projekt) a vloží ho při startupu do DI - např. `builder.Services.AddSalesArrangementService()`. 
SDK si následně z SD zjistí adresu své služby.  
Implementace `Clients` projektů je popsána [zde](grpc-services-clients.md).

### Resolving adres služeb třetích stran
Konzument referencuje SDK požadované služby (`ExternalServices.*` projekt) a vloží ho při startupu do DI - např. `builder.AddExternalService<IEas>()`. 
SDK si následně z SD zjistí adresu své služby.  
Implementace služeb třetích stran je popsána [zde](external-services.md).
