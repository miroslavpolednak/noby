# NOBY svět

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

## Témata
[Validace requestu a byznys logiky](./topics/validation.md)

[Autentizace](./topics/authentication.md)

[gRPC služby - technický popis](./topics/grpc-services.md)
