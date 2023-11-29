# Ukládání datových struktur
Existují případy, kdy nechceme pracovat s daty v databázi v klasické relační struktuře, ale potřebujeme uložit "hmotu" dat ve formátu JSON.
V takovém případě ukládáme JSON data do samostatné tabulky v databázi do nvarchar pole a pracujeme s ním pomocí SQL JSON funkcí.
Aplikace může obsahovat více JSON schémat (C# modelů), tj. více tabulek v databázi.

Pro tabulky s JSON daty existuje samostatné schéma **DDS**.
Každé JSON schéma (C# model) má vlastní tabulku, která se jmenuje stejně jako její C# model.
Např. C# třída `Income`, která je schématem pro JSON data s příjmy bude mít svůj protějšek v databázi pojmenovaný **DDS.Income**.

Abychom docílili koherentní práce s JSON daty, všechny DS/API používají pro práci s JSON daty stejnou service `IDocumentDataStorage`.
Tato service zajišťuje serializaci/deserializaci JSON objektů a jejich uložení/načtení z databáze. 
`IDocumentDataStorage` se nachází v projektu `SharedComponents`.

## EntityId vs ID záznamu
Každý záznam v databázi je uložen pod vlastním ID [int32] (toto ID se vrací při založení záznamu metodou `Add()`). 
Zároveň je možné záznamy ukládat také s ID entity (`EntityId`) ke které se daná data vztahují - např. pokud se jedná o data o příjmech klienta, může být toto ID = *CustomerOnSAId*.
ID entity je kvůli univerzálnosti vnitřně ukládáno jako string, nicméně ve většině případů budeme používat int.

> `EntityId` je nepovinný parametr, je tedy možné ukládat data i bez vazby na konkrétní entitu.

## Přidání nového JSON schéma do aplikace
1) založení C# modelu - jedná se o C# třídu, která odpovídá požadovanému JSON schématu. 
Tato třída musí implementovat interface `SharedComponents.DocumentDataStorage.IDocumentData`. 
Tento interface vyžaduji implementaci pouze jedné vlastnoti: `Version` - viz. níže.
2) kontrola, zda v dané databázi již existuje schéma **DDS**. Pokud ne, je třeba založit.
3) založení tabulky pro JSON data. Název tabulky odpovídá názvu modelu z bodu 1. 
SQL create skript pro tabulku je uveden [níže](#sql-skript-pro-vytvoření-tabulky-json-dat).

## Verzování JSON schématů
Pro přehlednost je možné verzovat změny ve schématech. 
Implementace `IDocumentData` vynucuje implementaci vlastnosti `Version`, která by měla vracet číslo verze schématu.
Číslo verze by se s každou změnou modelu mělo inkrementovat.

> Verzování nicméně není povinné a nemá žádný dopad na funkčnost service `IDocumentDataStorage` - jedná se pouze o informativní údaj.

## Adresářová struktura - umístění modelů
C# modely umisťujeme do adresáře s database features. Zde mají vlastní podadresář **DocumentDataEntities**.

```
[Api]
  [Database]
    [DocumentDataEntities]
	  Income.cs			// JSON schema
	  Household.cs			// JSON schema
	  ...
	[Entities]
	  ...
	MyDbContext.cs
```

## Příklady použití:

```csharp
private readonly IDocumentDataStorage _documentDataStorage;

...

record MyModel(int Amount, bool Confirmed) 
	: SharedComponents.DocumentDataStorage.IDocumentData: {}

...

// ID entity - např. HouseholdId, CaseId atd.
var entityId = 1;
// objekt instance dat
var document = new MyModel(100, true);

// založení nové instance dat
var id = await _documentDataStorage.Add(entityId, document, cancellationToken);

// update existující instance dat dle ID instance
await _documentDataStorage.Update(id, document);

// update existující instance dat dle ID entity
await UpdateByEntityId.Update(entityId, document);

// získat z databáze instanci dat dle ID instance
var loadedDocument = await _documentDataStorage.FirstOrDefault<MyModel>(id, cancellationToken)

// získat seznam instancí dat dle ID entity
var listOfDocuments = await _documentDataStorage.GetList<MyModel>(entityId, cancellationToken);

// smazání instance dat dle ID instance
// deletedRows je počet smazaných záznamů - tj. 0 znamená, že se smazání nepovedlo, protože ID neexistuje
var deletedRows = await _documentDataStorage.Delete<MyModel>(id);

// smazání instance dat dle ID entity
var deletedRows = await _documentDataStorage.DeleteByEntityId<MyModel>(entityId);
```

## SQL skript pro vytvoření tabulky JSON dat
*{{table_name}}* musí být nahrazeno názvem tabulky.

> Tabulka může být a nemusí historizovaná - záleží na zvážení konkrétního use case.

```sql
CREATE TABLE [DDS].[{{table_name}}](
	[DocumentDataStorageId] [int] IDENTITY(1,1) NOT NULL,
	[DocumentDataEntityId] [varchar](50) NULL,
	[DocumentDataVersion] [int] NOT NULL,
	[Data] [nvarchar](max) NULL,
	[CreatedUserId] [int] NOT NULL,
	[CreatedTime] [datetime] NOT NULL,
	[ModifiedUserId] [int] NULL,
	[ValidFrom] [datetime2](7) GENERATED ALWAYS AS ROW START NOT NULL,
	[ValidTo] [datetime2](7) GENERATED ALWAYS AS ROW END NOT NULL,
 CONSTRAINT [PK_{{table_name}}] PRIMARY KEY CLUSTERED 
(
	[DocumentDataStorageId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
	PERIOD FOR SYSTEM_TIME ([ValidFrom], [ValidTo])
) ON [PRIMARY]
WITH
(
SYSTEM_VERSIONING = ON (HISTORY_TABLE = [DDS].[{{table_name}}History])
)
```