# InternalServices.ServiceDiscovery
Služba poskytující URI ostatních služeb v perimetru MP - případně i jinde. Služba při prvním requestu načte všechny potřebné záznamy do cache a z té jsou následně poskytovány pro další volání.

## Prerequisites
Development
 - Visual Studio 2022

Požadavky na server:
 - runtime .NET 6
 - konektivita na SQL server

## Provoz služby
### Instalace na serveru
Služba běží jako Windows Service. Na pozadí WinSvc je spuštěný Kestrel na definovaném portu, který obsluhuje HTTP requesty. Uživatel pod kterým WinSvc běží nemusí mít žádná zvláštní oprávnění.  
Vytvoření Windows Service:

        sc.exe create FAT-ServiceDiscovery binPath= "d:\sluzby\discoveryservice\CIS.InternalServices.ServiceDiscovery.Api.exe winsvc"

Smazání Windows Service:

        sc.exe delete FAT-ServiceDiscovery

### Konfigurace Kestrelu
Konfigurace webserveru je možná souborem *kestrel.json*. Detail možností konfigurace viz. README projektu *CIS.Infrastructure.gRPC*.

### Konfigurace služby
Konfigurace služby je možná souborem appsettings.json.  

        {
          "Serilog": { ... },
        
          "ConnectionStrings": {
            "default": "..."
          },
        
          "CisEnvironmentConfiguration": {
            "DefaultApplicationKey": "CIS:ServiceDiscovery",
            "EnvironmentName": "FAT"
          },
        
          "AppConfiguration": {
            "CacheType": "InMemory"
          }
        }

 - **Serilog** custom nastavení sinku pro logování.
 - **ConnectionStrings** nastavení connection stringu na databázi
 - **CisEnvironmentConfiguration** nastavení CIS
 - **AppConfiguration** interní nastavení služby
   - **CacheType** (*InMemory* | *Redis*) nastavení typu cache, do které se ukládají adresy služeb.

### Konfigurace MS SQL serveru
Služba jako zdroj dat používá SQL server. Tabulka *ServiceDiscovery* obsahuje záznamy pro všechny dostupné služby a prostředí.  
Struktura tabulky je:

        [Id] [int] IDENTITY(1,1),
        [EnvironmentName] [varchar](50),
        [ServiceName] [varchar](50),
        [ServiceUrl] [varchar](250),
        [ServiceType] [tinyint],

 - **EnvironmentName** název prostředí pro které je daná adresa platná
 - **ServiceName** unikátní název služby napříč všemi prostředímy
 - **ServiceUrl** URL služby pro dané prostředí a typ
 - **ServiceType** (*1 = REST, 2 = GRPC*) typ služby

## Metody služby
### GetService()
Vrací informace o konrkétní službě identifikované jejím unikátním jménem, prostředím a typem.

### GetServices()
Vrácí všechny služby daného typu, dostupné v daném prostředí.

## grpcurl tests
        grpcurl -insecure 172.30.35.51:5002 list

        grpcurl -insecure -d "{\"Environment\":\"DEV\"}" -H "Authorization: Basic YTph" -H "mp-user-id: 267" 127.0.0.1:5005 CIS.InternalServices.ServiceDiscovery.v1.DiscoveryService/GetServices
        grpcurl -insecure -d "{\"Environment\":\"FAT\"}" 172.30.35.51:5002 CIS.InternalServices.ServiceDiscovery.v1.DiscoveryService/GetServices
