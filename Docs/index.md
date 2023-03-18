# NOBY svět

## Témata
[Psaní technické dokumentace](./topics/documentation.md)

[Databáze - čtení, zápis dat](./topics/database.md)

[Databáze - migrace](./topics/database-migrations.md)

[Konfigurace služeb / aplikací](./topics/configuration.md)

[Validace HTTP requestu a error handling](./topics/validation.md)

[Autentizace FE a doménových služeb](./topics/authentication.md)

[gRPC služby - technický popis](./topics/grpc-services.md)

[Implementace služeb třetích stran](./topics/external-services.md)

[Jak funguje ServiceDiscovery](./topics/service-discovery.md)

[Nastavení a použití Distributed cache](./topics/distributed-cache.md)

[Background services / periodické joby](./topics/background-services.md)

## Struktura solution - projekty a adresáře

### CIS
Obsahuje podpůrné projekty pro stavbu aplikací a služev v systému NOBY, např. společné interfaces, exceptions, atributy atd.

**Projekty:**

[CIS.Core](CIS.Core/index.md)  
Základní projekt ekosystému obsahující base interfaces, types, exceptions atd.

[CIS.Infrastructure.Security](CIS.Infrastructure.Security/index.md)  
Konfigurace a helpery pro autentizaci gRPC služeb.

[CIS.Infrastructure.Telemetry](CIS.Infrastructure.Telemetry/index.md)  
Loggování a telemetrie

[CIS.Infrastructure.MediatR](CIS.Infrastructure.MediatR/index.md)  
MediatR extensions, behaviors.

[CIS.Infrastructure.Logging.Extensions](CIS.Infrastructure.Logging.Extensions/index.md)  
Extension metody pro High Performance logging.

[CIS.Foms.Types](CIS.Foms.Types/index.md)  
Společné DTO, Interfaces, Enumy pro NOBY

[CIS.Infrastructure.gRPC](CIS.Infrastructure.gRPC/index.md)  
Podpora pro vytváření gRPC služeb v systému NOBY.

[CIS.Infrastructure.gRPC.CisTypes](CIS.Infrastructure.gRPC.CisTypes/index.md)  
Vlastní gRPC messages, které přepoužíváme v doménových službách.

[CIS.Infrastructure.WebApi](CIS.Infrastructure.WebApi/index.md)  
Podpora pro vytváření REST služeb.

[CIS.Infrastructure.ExternalServicesHelpers](CIS.Infrastructure.ExternalServicesHelpers/index.md)  
Podpora pro konzumaci REST a SOAP služeb třetích stran.

### DomainServices
Obsahuje byznysové služby. Každá služba obsluhuje vlastní byznys doménu, např. klient, domácnost, smlouva.
Jedná se o **gRPC** služby, které ale mohou v některých případech vystavovat i **REST** rozhraní pomocí *gRPC Transcoding*.
Doménové služby jsou poměrně "hloupé", nechceme do nich dávat např. pokročilé validace - to by měl řešit jejich konzument.

### InternalServices
Obsahuje podpůrné služby pro Doménové služby. Jedná se např. o Service discovery, notifikační služba, služba pro abstrakci filesystémového úložiště atd.
Jedná se o **gRPC** služby, které ale mohou v některých případech vystavovat i **REST** rozhraní pomocí gRPC Transcoding.

### ExternalServices
Obsahuje proxy projekty nad API třetích stran. V zásadě jde o to, aby konzument API třetí strany (tj. Doménová služba) v systému NOBY používal ExternalServices projekt místo přímé konzumace REST nebo WSDL rozhraní třetí strany. Tím zajistíme stejný způsob volání / logování / autentizace na dané API z kterékoliv naší služby.

### Playgrounds
Složka pro projekty "na hraní". Pokud je potřeba vytvoření testovací konzolové aplikace, ukázkové implementace atd. tak takové projekty patří sem.
