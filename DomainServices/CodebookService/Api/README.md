# DomainServices.CodebookService.Api
V tomto projektu by neměl mít nikdo kromě jeho tvůrce potřebu cokoliv měnit nebo upravovat (kromě konfiguračních souborů) !!!

Jedná se o službu poskytující číselníky v rámci ekosystému MP.  
Jednotlivé metody / číselníky si zakládají programátoři, kteří daný číselník potřebují sami.  
Návod na vytvoření nového číselníku je v README o úroveň výše.

Tento projekt pomocí Source Generators (*DomainServices.CodebookService.ApiGenerators*) analyzuje endpointy vytvořené v projektu *DomainServices.CodebookService.Endpoints* a na základě nalezených kontraktů automaticky generuje příslušné gRPC a REST endpointy.

## Provoz služby
### Instalace na serveru
Služba běží jako Windows Service. Na pozadí WinSvc je spuštěný Kestrel na definovaném portu, který obsluhuje HTTP requesty. Uživatel pod kterým WinSvc běží nemusí mít žádná zvláštní oprávnění.  
Vytvoření Windows Service:

        sc.exe create FAT-CodebookService binPath= "d:\sluzby\codebook\DomainServices.CodebookService.Api.exe winsvc"

Smazání Windows Service:

        sc.exe delete FAT-CodebookService

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
          },

          "EndpointsConfiguration": {
            "MyTestCodebook": { ... }
          }
        }

 - **Serilog** custom nastavení sinku pro logování.
 - **ConnectionStrings** nastavení výchozího connection stringu na databázi (na vlastní databázi Codebook service)
 - **CisEnvironmentConfiguration** nastavení CIS
 - **AppConfiguration** interní nastavení služby
   - **CacheType** (*InMemory* | *Redis*) výchozí cache provider.
 - **EndpointsConfiguration** struktura pro custom konfigurace jednotlivých číselníkových metod.

## grpcurl tests
        grpcurl -insecure 172.30.35.51:5007 list

        grpcurl -insecure -H "Authorization: Basic YTph" 127.0.0.1:5060 DomainServices.CodebookService/ProductTypes
        grpcurl -insecure -H "Authorization: Basic YTph" 127.0.0.1:5060 DomainServices.CodebookService/RelationshipCustomerProductTypes
        grpcurl -insecure -H "Authorization: Basic YTph" 172.30.35.51:5007 DomainServices.CodebookService/SalesArrangementStates
