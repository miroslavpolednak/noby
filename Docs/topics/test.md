# Integrační a unit testy

Pro integrační a unit testy existuje celá řada knihoven a frameworů, nicméně v rámci NOBY používáme jen ty, které jsou v projektu **CIS.Testing**, snažíme se tedy maximálně využívat infrastrukturu z tohoto projektu a nepřidáváme si nějaké specifické frameworky do našich test projektů. Nicméně pokud někomu nebude stačit to, co je v **CIS.Testing**, můžeme se dohodnout na adopci dalšího frameworku/knihovny

## Vytvoření testovacího projektu

1) Ve Visual studiu si vybereme testovací projekt pro **Xunit**, testovací projekt umístíme do adresářové struktury s následujícím namingem

- SomeDomainService
- - Tests
- - - DomainServices.SomeDomainService.Tests

2) Přidáme referenci na **CIS.Testing** a na projekt, který chceme testovat
3) V rámci testovacího projektu držíme odděleně unit a integratrion testy a veškerou pomocnou test infrastrukturu (baseClass atd.) držíme v rámci složky Helpers 

- DomainServices.SomeDomainService.Tests
- - IntegrationTests
- - - Helpers
- - UnitTests
- - - Helpers

## Unit testy 
 Unit testy píšeme pomocí knihoven v **CIS.Testing** (NSubstitute, xunit, FluentAssertions atd.) Jelikož na psaní unit testů není nic speciálního a všude je dostatek návodů, jak unit testy pomocí výše popsaných knihoven napsat, nebude se tato dokumentace psaní unit testů nějak blíže věnovat.  

## Integrační testy 
Noby integrační testy jsou postaveny nad WebApplicationFactory, což je abstrakce z dílny MS pro psaní integračních testů (primárně REST, ale naše implementace je upravená pro GRPC a specifické NOBY potřeby). Je třeba si uvědomit, že při integračních testech proběhne veškerá registrace do DI a jsou použity stejné middleware jako při bežném grpc requestu. Pokud tedy chcete do DI zaregistrovat nějaký mock (přes NSubstitute) je třeba nejprve odebrat z DI specifickou třídu, která implementuje nějaký interface a zaregistrovat si svůj mock. Vše bude detailně popsáno a vysvětleno dále v tomto návodu.   

### Příprava infrakstruktury pro integrační testy 

1 )  **WebApplicationFactory** potřebuje k svému fungování bootstrapovat naši aplikaci (typicky API projekt) za využití Program.cs. Jelikož od .NET6 se v rámci konfigurace používá top-level statements, musíme Program.cs zviditelnit pro WebApplicationFactory, na konec Program.cs přidáme následující:

```csharp
#pragma warning disable CA1050 // Declare types in namespaces
public partial class Program
#pragma warning restore CA1050 // Declare types in namespaces
{
    // Expose the Program class for use with WebApplicationFactory<T>
}
```
2 ) **Vytvoříme si IntegrationTestBase.cs** ve složce Helpers pro konfiguraci integračních testů a obecnou infrastrukturu (od této base třídy musí dědit všechny naše integrační testy). 

```csharp
internal abstract class IntegrationTestBase : IClassFixture<WebApplicationFactoryFixture<Program>>
{
    public IntegrationTestBase(WebApplicationFactoryFixture<Program> fixture)
    {
        Fixture = fixture;

        ConfigureWebHost();
    }
    public WebApplicationFactoryFixture<Program> Fixture { get; }

    private void ConfigureWebHost()
    {
        Fixture.ConfigureCisTestOptions(options =>
        {
            options.DbMockAdapter = new SqliteInMemoryMockAdapter();
        })
        .ConfigureServices(services =>
        {
            // This mock is necessary for mock of service discovery
            services.RemoveAll<IUserServiceClient>().AddSingleton<IUserServiceClient, MockUserService>();
        });
    }

}
```
3 ) **Přidáme si appsettings.Testing.json**  do testovacího projektu, minimální konfigure může vypadat například takto:
```json
{
  "CisEnvironmentConfiguration": {
    "DefaultApplicationKey": "DS:MyDomainService",
    "EnvironmentName": "TEST",
    "ServiceDiscoveryUrl": "https://notExtsxxxx.cz",
    "DisableServiceDiscovery": true
  }
}
```
 naming musí být dodržen, popřípadě lze změnit pomocí CisWebApplicationFactoryOptions.AppSettingsName za pomoci ConfigureCisTestOptions. Do testovací konfigurace si můžete dávat libovolnou konfiguraci nebo overridovat existující (při overridu vzhledem k tomu, že konfigurace registrujeme jako singleton, musíte nejprve odstranit z DI kontejneru stávající konfiguraci a nahradit ji tou z appsettings.Testing.json). 

4 ) **Mock Service discovery**
Pokud chceme testovat api, které využívá service discovery, musíme v appsettings.Testing.json nastavit "DisableServiceDiscovery": true a namockovat IUserServiceClient viz. bod 2

5 ) **Vytvoření mocku za využiní NSubstitute**
Většína Api služeb má externí zavislosti na jiné služby, nebo nějakou funkcionalitu, kterou potřebujeme namockovat. Mock vytvoříme následujícím spůsobem:    

```csharp
public abstract class IntegrationTestBase : IClassFixture<WebApplicationFactoryFixture<Program>>
{
    //Mocks
    internal IDataAggregatorServiceClient DataAggregatorServiceClient { get; }
    
    public IntegrationTestBase(WebApplicationFactoryFixture<Program> fixture)
    {
        Fixture = fixture;

        DataAggregatorServiceClient = Substitute.For<IDataAggregatorServiceClient>();
    }
    
    public WebApplicationFactoryFixture<Program> Fixture { get; }

    private void ConfigureWebHost()
    {
        Fixture
        .ConfigureServices(services =>
        {
            // This mock is necessary for mock of service discovery
            services.RemoveAll<IUserServiceClient>().AddSingleton<IUserServiceClient, MockUserService>();
            // NSubstitute mocks
            services.RemoveAll<IDataAggregatorServiceClient>().AddTransient(r => DataAggregatorServiceClient);
        });
    }
}
```
a pak už jen někde v testu použijeme mock za pomoci NSubstitute (nejlépe je dělat mock v konstruktoru)

```csharp
public class SomeTest : IntegrationTestBase
{
    public SomeTest(WebApplicationFactoryFixture<Program> fixture) : base(fixture)
    {
        var resp = new GetDocumentDataResponse
        {
            DocumentTemplateVariantId = 1,
            DocumentTemplateVersionId = 4,
        };

        resp.DocumentData.Add(new DocumentFieldData { FieldName = "TestName", Text = "TestText" });
        DataAggregatorServiceClient.GetDocumentData(Arg.Any<GetDocumentDataRequest>()).ReturnsForAnyArgs(resp);
    }

}
```

### Konfigurace integračních testů
Defaultně je vše nastaveno tak, aby se nemuselo nic specificky konfigurovat, nicméně pokud vám z nějakého důvodu nevyhovuje defaultní konfigurace, která se nachází v:

```csharp
namespace CIS.Testing;
public class CisWebApplicationFactoryOptions
{
    // Každá konfigurační property je důkladně popsána
    ...
}
```
Tak se dají integrační testy specificky nakonfigurovat pomocí options paternu viz. bod 2, kde je ukázáno, jak namísto defautního EfCoreInmemory adaptéru použijeme sqlite adaptér pro mockování databáze, kde sqlite databáze je blíže chování sql serveru (funguje zde auto increment primárních klíčů, view atd.).

Pokud by z tohoto návodu nebylo něco jasné, tak je dobré se inspirovat již hotovými integračními testy (DocumentArchiveService, ServiceDiscovery, DocumentOnSaService). 