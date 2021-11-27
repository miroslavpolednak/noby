# ExternalServices.Eas
Wrapper pro SB **EAS** WCF službu. Projekt/NuGet může obsahovat více verzí implementace EAS.  
Jednotlivé verze EAS jsou rozdělené do namespaces podle Release ve kterém byly nasazeny. Tj. např. pro *Release 21* je namespace **ExternalServices.Eas.R21**.

## Registrace service client-a
Registrace do DI během startu aplikace:

        services.AddExternalServiceEas(...);

Jako jediný parametr přijímá metoda *AddExternalServiceEas* instanci konfigurace služby - třídu **ExternalServices.Eas.EasConfiguration**. 
V konfiguraci je nutné zadat verzi implementace a URL EAS instance nebo nastavit **UseServiceDiscovery**=true.  
Pokud je *UseServiceDiscovery=true*, pokusí se knihovna zeptat na URL EAS instance pro dané prostředí do Service Discovery služby. V tomto případě je ale nutné, aby konzument zaregistroval Service Discovery službu a nastavil CIS environment.

Příklad registrace hodnotami z konfiguračního souboru:

        services.AddExternalServiceEas(appConfiguration.EAS);

## Použití service client-a
Pro použití vybrané implementace si stačí z DI vyžádat zaregistrovanou verzi clienta.

        class MyHandler {
          public MyHandler(xternalServices.Eas.R21.IEasClient easClient) { ... }
        }

## Interní nastavení
### Vygenerování EAS wrapperu
        dotnet-svcutil "d:\Visual Studio Projects\MPSS-FOMS\CIS\ExternalServices\Eas\EAS_WS_SB_Services.xml" -o "EasWrapper.cs" -i -n *,ExternalServices.Eas.R21.EasWrapper

### ServiceDiscovery
Klíč pro uložení v ServiceDiscovery tabulce je **ES:EAS**, typ je **Proprietary**.