# InternalServices.ServiceDiscovery.Abstraction
NuGet poskytující abstrakci nad službou ServiceDiscovery. Umožňuje volání služby přímo z C# kódu bez znalosti jejích API.

## Použití
NuGet obsahuje extension metodu pro registraci během startupu aplikace:

        ...
        services.AddCisServiceDiscovery();
        ...

Tato metoda má několik přetížení akceptujících tyto parametry:
 - **discoveryServiceUrl** umožňuje explicitně zadat adresu *ServiceDiscovery* služby
 - **isInvalidCertificateAllowed** umožňuje volat službu, jejíž SSL certifikát je self-signed

Pokud konzumující aplikace používá nastavení *CIS environment*, není potřeba zadávat explicitně *discoveryServiceUrl* parametr, protože URL ServiceDiscovery služby je obsaženo v konfiguraci (v appsettings.json):

        "CisEnvironmentConfiguration": {
            ...
            "ServiceDiscoveryUrl": "https://172.30.35.51:5002",
            ...
        }

Následně kdekoliv v kódu konzumující aplikace je možné si pomocí DI vyžádat instanci **IDiscoveryServiceAbstraction**, která má implementované metody *GetServices* a *GetService*.
