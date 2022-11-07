

# DomainServices.CodebookService
Služba poskytující centralizované číselníky ze všech zdrojů v rámci MP a KB. Služba je dostupná ve formátech gRPC a WebApi (REST).  
Součástí řešení je i NuGet **Abstraction**, který umožňuje konzumaci služby formou C# rozhraní.  
Služba je dostupná pouze pro přihlášené uživatele pomocí *Basic Authentication* - stejně jako ostatní CISServices a DomainServices.  
Řešení projektu je koncipované tak, aby si mohl kdokoliv dle potřeby jednoduše přidat vlastní / další číselník (endpoint).

## Prerequisites
----
 - Visual Studio 2022
 - .NET 6

## Run & test project
Spuštění služby:

        dotnet run DomainServices.CodebookService.Api.csproj

Testování endpointu pomocí *grpcurl* (https://github.com/fullstorydev/grpcurl):

        grpcurl -insecure 172.30.35.51:5006 list DomainServices.CodebookService
        grpcurl -insecure -H "Authorization: Basic YTph" 172.30.35.51:5006 DomainServices.CodebookService/MyTestCodebook


## Struktura solution - Visual Studio projects
----  

**DomainService.CodebookService.Contracts**
 - Interface / datový kontrakt pro gRPC službu
 - Request & Response Dto's
 - gRPC kontrakty jsou vytvářeny pro **Code First gRPC** viz. https://docs.microsoft.com/en-us/aspnet/core/grpc/code-first?view=aspnetcore-5.0 a https://protobuf-net.github.io/protobuf-net.Grpc/gettingstarted
 - Request Dto jsou zároveň použité jako rozlišovací objekt pro **MediatR**, aby ten věděl jaký handler zavolat. Musí tedy implementovat *IRequest<>*. Viz. https://github.com/jbogard/MediatR

**DomainService.CodebookService.Endpoints**
- request handlery pro metody definované v *Contracts* projektu
- od programátora se očekává pouze vytvoření Request Handleru, napojení na gRPC a WebApi endpointy proběhne automaticky

**DomainService.CodebookService.Api**
 - hlavní projekt - web application
 - zpracování requestu je napojeno na *MediatR* pipeline, pro každý endpoint se tedy vytváří nový *RequestHandler*

**DomainService.CodebookService.Clients**
 - NuGet balíček zjednodušující konzumaci Codebook služby. Umožňuje volat službu pomocí strongly-typed rozhraní, tj. abstrahuje konzumenta od implentace gRPC nebo Http klienta.
 - balíček interně používá *CIS Service Discovery*, konzumující strana tedy nemusí znát adresu Codebook služby
 - pokud je konzumující aplikace správně nastavena do CIS prostředí, tak si balíček dokáže zjistit technického uživatele dané aplikace a provést autentizaci pod tímto účtem
 - Codebook služba je volána gRPC protokolem
 - data vrácená služou jsou automaticky kešovaná InMemory keší

**DomainService.CodebookService.Tests**
 - unit testy, integrační testy
 - implementace pomocí **xUnit** a **CIS.Testing**

## Návod pro přidání nového endpointu
----

1) **Nastavení datového kontraktu** (projekt Contracts)  
V projektu *Contracts* vytvořit v adresáři *Endpoints* nový podadresář. Tento podadresář pojmenovat podle názvu nového endpointu / metody.  
V nově vytvořeném adresáři přidat gRPC endpoint jako součást partial interface ICodebookService, např.:

        public partial interface ICodebookService
        {
            [OperationContract]
            Task<List<GenericCodebookItem>> MyTestCodebook(Endpoints.MyTestCodebook.MyTestCodebookRequest request, CallContext context = default);
        }

2) **Request Dto pro datový kontrakt** (projekt Contracts)  
Dále je potřeba založit request Dto pro datový kontrakt. Tento objekt musí implementovat interface *IRequest<>* z *MediatR*, jinak by API projekt nevěděl jaký handler pro obsloužení endpointu použít.  
Dto je dobré založit ve stejném adresáři jako kontrakt. Příklad jednoduchého request Dto:

        [DataContract]
        public sealed class MyTestCodebookRequest : IRequest<List<GenericCodebookItem>> { }
        
V příkladech z bodu 1 a 2 by tedy struktura projektu *Contracts* měla po změnách vypadat takto (hvězdičkou označené nově přidané):  
DomainService.CodebookService.Contracts *(projekt)*  
==Endpoints  
====*MyTestCodebook  
======*MyTestCodebook.cs *(kontrakt)*  
======*MyTestCodebookRequest.cs *(request Dto)*  

3) **MediatR handler, který obsahuje byznys logiku endpointu** (projekt Endpoints)  
V projektu *Endpoints* vytvořit nový adresář a pojmenovat podle názvu nového endpointu / metody.  
V adresáři vytvořit nový MediatR Request Handler implementující *IRequestHandler<>* dle kontraktu endpointu. Handler pojmenovat v této konvenci: "*{nazev_endpointu_z_Contracts.ICodebookService}Handler*".  
Příklad signatury Request Handleru:

        public class MyTestCodebookHandler
          : IRequestHandler<Contracts.Endpoints.MyTestCodebook.MyTestCodebookRequest, List<Contracts.GenericCodebookItem>>
        { }

4) **Další pomocné třídy - !volitelně** (projekt Endpoints)  
V případě potřeby přidání pomocných tříd, např. custom repository je možné tyto třídy přidat do nově vytvořeného adresáře.  
U těchto tříd je potřeba nezapomenout na správné odekorování atributy *TransientService*, *SelfService* atd. v případě potřeby zapojení do DI.

5) **Vytvoření unit testů, integračních testů** (projekt Tests)  
V adresáři *Tests* vytvořit třídu s názvem Request Handleru, případně nový podadresář pokud bude testů hodně.  
K testování je použit framework *xUnit* a vlastní implementace *IClassFixture* z projektu *CIS.Testing* (tento projekt obsahuje helper třídy zejména pro zjednodušení integračních testů pro gRPC služby).

## Konzumace nového endpointu
----
Pokud je vše vytvořeno podle návodu, je nový endpoint připraven na nasazení.   
Služba *CodebookService* poslouchá na dvou portech - jeden port pro gRPC (HTTP 2) a jeden port pro WebApi (HTTP 1.1).

**gRPC endpoint:**  
Adresa: */DomainServices.CodebookService/{nazev_metody}*  
Příklad: https://127.0.0.1:5060/DomainServices.CodebookService/MyTestCodebook

**WebApi (REST) endpoint:**  
Adresa: */codebooks/{nazev_metody_konvertovany_z_camelcase_na_dash_delimited}*  
Příklad: https://127.0.0.1:5061/codebooks/my-test-codebook

Zároveň služba podporuje *gRPC Reflection* a standardní CIS *Healthcheck* endpoint.  
Pro zjednodušení může konzumující aplikace nainstalovat *CodebookService.Clients* NuGet a používat pro komunikaci se službou rozhraní v C#.

## Autentizace a autorizace

Služba používá stejnou autentizaci jako všechny CIS services - tj. Basic authentication (pro gRPC i REST endpointy). Aktuálně je pro autentizace použitý hardcoded uživatel a / a.  
Autorizace není použita, jakmile je uživatel ověřen, má dostupné všechny metody služby.

## Další pravidla a doporučení
----
1)  vzhledem k jednoduchosti doporučuji použít pro komunikaci s databází **Dapper** (https://dapper-tutorial.net/). Výchozí *ConnectionProvider* je zaregistrován na databázi *CodebookService*, nicméně každý endpoint si může vytvořit vlastní (viz. příklad).
2) v případě potřeby je možné se napojit na startup build aplikace. Pro to stačí vytvořit třídu implementující interface **ICodebookServiceEndpointStartup** (viz. příklad).
            
        internal sealed class MyTestCodebookStartup : ICodebookServiceEndpointStartup
        {
			public void Register(WebApplicationBuilder builder) { ... }
        }

## Testovací endpoint - příklad
----
Projekt obsahuje vytvořený testovací endpoint, který může sloužit jako ukázka implementace. Název endpointu je "**MyTestCodebook**".  
Implementace obsahuje ukázku uložení do distributed cache, vlastní konfiguraci v *appsettings.json*, vytvoření custom repository. Všechny komponenty tohoto endpointu jsou pro jednoduchost dokumentovány v kódu.

## Known issues & TODOs
----
- projekt *Abstraction* generuje metody bez parametru, pokud má konkrétní endpoint Request Dto obsahující parametry, nebude fungovat správně. V PŘÍPADĚ POTŘEBY NENÍ PROBLÉM DODĚLAT
- stejný problém se týká i generovaných WebApi metod v *Api* projektu