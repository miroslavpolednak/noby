# Použití DataAggregatoru

## Data pro dokumenty

Metoda [GetDocumentData](https://wiki.kb.cz/display/HT/getDocumentData) zajistí vygenerování dat pro PDF ve formě key-value.
Na vstupu je třeba zadat požadovaný dokomunet pomocí kombinace typu dokumentu, verze a varianty.
Jako vstupní parametry se zadávají pouze ID, které jsou momentálně známé a zbytek se případně dotáhnou dynamicky. Všechny dotažené ID se vracejí v odpovědi doplněné v **InputParameters**.

| **Název** | **Popis** |
|--|--|
| DocumentTypeId | Hodnota z číselníku [DocumentType](https://wiki.kb.cz/display/HT/DocumentType) |
| DocumentTemplateVersionId | Volitelná hodnota z číselníku [DocumentTemplateType](https://wiki.kb.cz/display/HT/DocumentTemplateVersion). V případě nevyplnění, se automaticky dotáhne nejnovější verze (platí i když je vyplněná varianta) |
| DocumentTemplateVariantId | Volitelná hodnota z číselníku [DocumentTemplateVariant](https://wiki.kb.cz/display/HT/DocumentTemplateVariant). Pokud není vyplněno a šablona má varianty, tak se automaticky doplní dle zadané (či automaticky dotažené) verze dokumentu. |

### Příklad volání

```csharp
var inputParameters = new InputParameters { SalesArrangementId = 20002, UserId = 3048 };

var dataRequest = new GetDocumentDataRequest
{
    DocumentTypeId = (int)DocumentTemplateType.ZADOSTHU,
    DocumentTemplateVersionId = 1,
    DocumentTemplateVariantId = 4,
    InputParameters = inputParameters
};

var data = await dataAggregatorService.GetDocumentData(dataRequest);
```

## Datová věta

Metoda [GetEasForm](https://wiki.kb.cz/display/HT/getDocumentData) vygeneruje požadované datové věty a vrátí je ve formě JSON.
Na vstupu se musí specifikovat o jaký typ žádosti se jedná (produktová, servisní) a pro jaké dokumenty ([DocumentType](https://wiki.kb.cz/display/HT/DocumentType)) se má datová věta vygenerovat.
U produktové žádosti se mimo jiné na vstupu specifikuje i HouseholdId pro konkrétní dokument. 

Jak a jaké vstupní parametry zadávat se dá nalézt na Confluence u endpointu GetEasForm. Případně širší kontext a další podrobnosti lze nalézt v v endpointu [SendToCmp](https://wiki.kb.cz/display/HT/sendToCmp) na SalesArrangement service.

### Příklad volání

```csharp
var request = new GetEasFormRequest
{
    SalesArrangementId = 1248,
    EasFormRequestType = EasFormRequestType.Product,
    DynamicFormValues =
    {
        new DynamicFormValues
        {
            DocumentTypeId = 4,
            HouseholdId = 123
        }
    }
};

var response = await dataAggregator.GetEasForm(request);
```

# DataAggregator API - Struktura

Služba se interně skládá ze 3 částí. 

1. **Konfigurace** - v této části se nachází databázový model a metody k načtení konfigurace, jak se mají data vygenerovat.
2. **DataServices** - tato část je sdílená pro všechny typy generování. Načítá a agreguje data z potřebných služeb, které byly nakonfigurovány pro daný případ v databázi.
3. **Generátory dat** - vlastní logika pro generátor dat. Momentálně knihovna obsahuje dva a to na generování dat pro dokumenty a sestavení json věty.

## Konfigurace

Konfigurace se nachází v databázi DataAggregatorService. Tabulky s konfigurací jsou buď sdílené (například DataServices, DataField), nebo specifické pro konkrétní konfiguraci dokumentů či datových vět.
Detailnější popis tabulek se nachází níže.

Konfiguraci řídí `IConfigurationManager`, který může být buď cachováný nebo ne. Cachování se zapíná v `appsettings.json` následnou konfigurací:

```json
"DataAggregatorConfiguration": {
    "UseCacheForConfiguration": true,
    "CacheExpirationSeconds": 3600
}
```

### Sdílené tabulky

| **Název** | **Popis** |
|--|--|
| DataService | Číselník jednotlivých zdrojů, ze kterých je možné genericky načíst data. Kód poté využívá tento číselník a každá zdrojová služba znamená jeden endpoint do doménové služby. |
| DataField | Obecný záznam, který specifikuje cestu k atributu objektu, který byl načten přes DataService. Pro cestu k atributu se využívá tečková notace, případně je možné použít "[]" pro označení, že se jedná o kolekci. <br /> <br /> V tabulce lze též definovat výchozí StringFormat, který se momentálně využije pouze v dokumentech. |
| InputParameter | Číselník, který obsahuje možné typy ID v objektu `InputParameters`, který sdržuje všechny potřebné vstupní parametry pro jednotlivé služby definové v DataService. Tento číselník se využívá pro dynamické načítání vstupních parametrů = načtení ID z výsledku vráceného jednou z DataService. |


### Dokumenty

| **Název** | **Popis** |
|--|--|
| Document | Číselník pro typy dokumentů, především pro zachování relační integrity. |
| DocumentDataField | Záznamy definují mapování z DataService do acrofieldů v dokumentu. Lze zde nastavit jak StringFormat, tak i výchozí text, pokud by načtená hodnota byla null. |
| DocumentDataFieldVariant | Definice, které záznamy z DokumentDataField se mají použít pro kterou variantu (pokud se nejedná o variantní šablonu, tak se tato tabulka nepoužívá) |
| DocumentSpecialDataField | Speciální mapování atributů, které nejsou normálně obsaženy v DataService či obsahují specifickou logiku. Každý dokument v kódu může mít svůj konkrétní objekt, kde se daná logika zachytí a v této tabulce se dopíše mapování. |
| DocumentSpecialDataFieldVariant | Obdoba DocumentDataFieldVariant, pouze pro speciální atributy. |
| DocumentTable | Obsahuje definici generické tabulky (momentálně pouze splátkový kalendář). Nastavuje se zde název acrofieldů, který slouží jako placeholder pro zjištění velikosti a pozice tabulky. Také se zde uvádí závěrečný paragraf, který se může vytisknout na konec tabulky. |
| DocumentTableColumn | Záznamy určují kolik sloupců má tabulka obsahovat. Definuje se zde název sloupce, StringFormat, který se aplikuje na všechny záznamy v tomto sloupci, a také velikost sloupce v procentech. |
| DocumentDynamicInputParameter | Definuje dynamické parametry, které se mají získat z výsledků již zavolané služby. Například na vstup se uvede SalesArrangementId a z načteného SalesArrangementu se načte CaseId a OfferId. |
| DynamicStringFormat | Záznamy nastavují dynamický StringFormat pro jednotlivá pole z tabulky DocumentDataField. Zvolí se takový StringFormat, který splňuje první (dle Order) všechny podmínky. |
| DynamicStringFormatCondition | Podmínky pro určení dynamického StringFormatu. |

### Datová věta

|  **Název** | **Popis** |
|--|--|
| EasRequestType | Číselník obsahující typ žádostí - produktová nebo servisní |
| EasFormType | Číslník obsahující název jednotlivých formulářů. Též se zde nastavuje verze formuláře a jeho platnost. |
| EasFormDataField | Obdoba DocumentDataField. Specifikuje mapování z výsledků služby do JSONu. Pro uvedení cesty v JSONu se využívá tečková notace, případně "[]" pro označení kolekce. |
| EasFormSpecialDataField | Mapuje specifické atributy, které neobsahují generické služby. V kódu pro každou žádost existují vlastní objekty, kde je možné napsat konkrétní vlastnosti a ty poté zde namapovat. |
| EasFormDynamicInputParameter | Stejně jako u dokumentů, definují se zde vstupní parametry, které se mají dotáhnout z výsledků již zavolaných služeb. |

### SQL skripty pro dokumenty
  
Mapping obecných atributů

```` sql
SELECT dd.DocumentId, dd.DocumentVersion, dd.AcroFieldName, ds.DataServiceName, dd.DataFieldId, df.FieldPath, ISNULL(dd.StringFormat, df.DefaultStringFormat) as StringFormat
FROM DocumentDataField dd
INNER JOIN DataField df ON df.DataFieldId = dd.DataFieldId
INNER JOIN DataService ds ON ds.DataServiceId = df.DataServiceId
WHERE dd.DocumentId = 1 AND DocumentVersion = '001A'
````

Mapping speciálních atributů

```` sql
SELECT dd.DocumentId, dd.AcroFieldName, ds.DataServiceName, dd.FieldPath, dd.StringFormat
FROM DocumentSpecialDataField dd
INNER JOIN DataService ds ON ds.DataServiceId = dd.DataServiceId
WHERE dd.DocumentId = 1
````

Mapping dynamických StringFormatů

```` sql
SELECT dd.DocumentId, dd.DocumentVersion, dd.AcroFieldName, dd.StringFormat as FieldStringFormat, dsf.Format as DynamicStringFormat, c.EqualToValue as WhenEqualToValue, ds.DataServiceName TargetService, df.FieldPath as TargetFieldPath
FROM DynamicStringFormat dsf
INNER JOIN DynamicStringFormatCondition c ON c.DynamicStringFormatId = dsf.DynamicStringFormatId
INNER JOIN DocumentDataField dd ON dd.DocumentDataFieldId = dsf.DocumentDataFieldId
INNER JOIN DataField df ON df.DataFieldId = c.DataFieldId
INNER JOIN DataService ds ON ds.DataServiceId = df.DataServiceId
ORDER BY dsf.DocumentDataFieldId, dsf.Priority
````

### SQL skripty pro datovou větu

Mapping JSON věty

```` sql
SELECT * FROM
(
  SELECT rt.EasRequestTypeId, rt.EasRequestTypeName, ft.EasFormTypeName, f.JsonPropertyName, ds.DataServiceName, df.FieldPath 
  FROM EasFormDataField f
  INNER JOIN EasRequestType rt ON rt.EasRequestTypeId = f.EasRequestTypeId
  INNER JOIN EasFormType ft ON ft.EasFormTypeId = f.EasFormTypeId
  INNER JOIN DataField df ON df.DataFieldId = f.DataFieldId
  INNER JOIN DataService ds ON ds.DataServiceId = df.DataServiceId

  UNION

  SELECT rt.EasRequestTypeId, rt.EasRequestTypeName, ft.EasFormTypeName, f.JsonPropertyName, ds.DataServiceName, f.FieldPath 
  FROM EasFormSpecialDataField f
  INNER JOIN EasRequestType rt ON rt.EasRequestTypeId = f.EasRequestTypeId
  INNER JOIN EasFormType ft ON ft.EasFormTypeId = f.EasFormTypeId
  INNER JOIN DataService ds ON ds.DataServiceId = f.DataServiceId
) as b
WHERE b.EasRequestTypeId = 2
ORDER BY b.EasFormTypeName, b.JsonPropertyName
````

## DataServices

DataServices se starají o agregaci dat z ostatních služeb. Nachází se zde obecná logika na načtení dat ze služeb dle konfigurace, či dotažení dynamických vstupních parametrů.

Mapování záznamů z DataService tabulky do skutečných tříd zajišťuje třída `ServiceMap`.
Je možné zde zaregistrovat "základní" DataService, tedy standardně hlavní endpoint dané služby (např. GetSalesArrangement u SalesArrangement Service).
Také se zde zaregistrovat "extensions", která může obsahovat další endpointy dané služby (např. Offer Service již má hlavní metodu, ale ještě chceme mít možnost dotáhnout PaymentSchedule z dalšího endpointu).

Mapování DataServices na konkrétní třídy probíhá přes interface `IServiceWrapper`, kde předpis metody slouží pro hlavní endpoint dané služby. Poté se do této služby mohou dopsat další rozšiřující metody, které se následně namapujou na DataService.

## Generátor dat pro dokumenty

Logika generování dat pro dokumenty se nachází v Services/Documents. Třída `DocumentMapper` združuje hlavní metody pro samotné mapování dat do key-value kolekce.

Složka TemplateData obsahuje třídy pro jednotlivé šablony, které rozšiřují hlavní agregační třídu. Tato třída obsahuje specifický vlastnosti a jejich logiku, jak se mají zobrazit. Na tyto atributy se odkazuje právě tabulka DocumentSpecialDataField.
Třídy by měly obsahovat pouze atributy, které mají složitější logiku, kterou nelze nastavit dosavadní konfigurací. Měla by zde být snaha o minimalizaci tabulky DocumentSpecialDataField z důvodu udržitelnosti.

Velká míra složitosti nastavá právě díky podmíněnému formátování v tabulkách DynamicStringFormat a DynamicStringFormatCondition.
Tyto podmínky lze nastavit pouze na klasické atributy z tabulky DocumentDataField. Logika zde ovládá, kdy se má použít jaký text (přes StringFormat) a nebo naopak, kdy se má skrýt (prázdný string).
Logika funguje na principu nastavení daného textu, jeho priority a poté kolekce podmínek. Následně se postupuje dle priorit a vyhodnocují se jednotlivé podmínky. 
Pokud všechny definové podmínky byly pro dynamický text splněny, tak je použit a proces vyhodnocení končí. Pokud žádný dynamický text nesplní podmínku, tak se použije klasický StringFormat či DefaultStringFormat z tabulky DataField.

## Generátor datových vět

Proces generování se nachází v Services/EasForms. Zde se třída `EasFormFactory` stará o vytvoření správného objektu podle požadavku. 
Momentálně existují dva hlavní objekty, které zastřešují formuláře - `EasProductForm` a `ServiceProductForm`.

Generování datových vět dost závisí na číselníku [DocumentType](https://wiki.kb.cz/display/HT/DocumentType). Každá datová věta k sobě má dokument a právě zadaných ID dokumentů se generují datové věty.
Na vstupu je tedy mandatorní zadat správně DocumentTypeId, jinak se žádná datová věta nevygeneruje.

Pro správné generování datových vět je také potřeba mít již vytvořený objekct DocumentOnSa. Z tohoto objektu se dotahují ID potřebné v SB.
Pokud objekt není vytvořený, tak hodnoty budou null a datová věta nebude platná. To ovšem neplatí v případě generování datové věty pro případ validace, kde hodnoty z DocumentOnSa nejsou povinné či je potlačujeme.

Třída EasProductForm slouží pro produktovou žádost a počítá s více datovými větami. V této třídě tedy dochází k iteraci kolekce "DynamicFormValues", která přijde na vstupu.
V iteraci dojde k nastavení dat na správný Household a DocumentOnSa.

Třída ServiceProductForm počítá pouze s jednou datovou větou. HouseholdId zde není potřeba pouze se vygeneruje datová věta, dle konfigurace bez větších zásahů či nastavování specifických dat.

Specifikum vygenerované JSONu, který podstatě představuje datovou větu, je jeho formát. V tomto případě se nejedná úplně o standardní JSON formát, ale všechny hodnoty jsou zde zadané jako text.
To proces generování trochu komplikuje, jelikož se nedá spoléhat na standard a někdy s číselnými typy či datem může být problém.
JSON se zde dynamicky skládá pomocí Dictionary, která představuje kolekci název atributu - hodnota. Pro vnořené objekty se používá jako hodnota další dictionary.
O logiku generování JSON se starají objekty ve složce Services/EasForms/Json. Tyto objekty se na sebe naskládají a vznikne podstatě stromová struktura.
Nejspodnější uzly stromu poté obsahují objekt `EasFormJsonValue`, což je samotná hodnota a v tomto objektu se též nachází specifické formátování, které je vyžadováno při převádění klasických typů na text.