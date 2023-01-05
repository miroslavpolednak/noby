# Příklady použití

### Vygenerování dat pro dokumenty

```csharp
var input = new InputParameters { OfferId = 1160, UserId = 3048 };

var dataAggregator = serviceProvider.GetRequiredService<IDataAggregator>();
var data = await dataAggregator.GetDocumentData(DocumentTemplateType.KALKULHU, "001A", input);
```

### Vygenerování json věty

```csharp
var dataAggregator = serviceProvider.GetRequiredService<IDataAggregator>();
var easForm = await dataAggregator.GetEasForm<IProductFormData>(500);

//Returns collection of forms (e.g. F3601 and F3602)
var forms = easForm.BuildForms(Enumerable.Empty<DynamicFormValues>());
```

# Struktura projektu

Projekt je v momentální podobě jako knihovna a je interní struktura je rozložena do 3 částí.

1. **Konfigurace** - v této části se nachází databázový model a metody k načtení konfigurace, jak se mají data vygenerovat. (pro dokumenty a json větu).
2. **DataServices** - tato část je sdílená pro všechny typy generování. Načítá a agreguje data z potřebných služeb, které byly nakonfigurovány pro daný případ v databázi.
3. **Generátory dat** - vlastní logika pro generátor dat. Momentálně knihovna obsahuje dva a to na generování dat pro dokumenty a sestavení json věty.

# Konfigurace

Konfigurace pro DataAggregator se nachází v databázi DataAggregatorService. Tabulky databáze se rozdělují na sdílené a na konkrétní případ (json věta nebo dokumenty).

## Sdílené tabulky

| **Název** | **Popis** |
|--|--|
| DataService | Číselník jednotlivých zdrojů, ze kterých je možné genericky načíst data. Kód poté využívá tento číselník a každá zdrojová služba znamená jeden endpoint do doménové služby. |
| DataField | Obecný záznam, který specifikuje cestu k atributu objektu, který byl načten přes DataService. Pro cestu k atributu se využívá tečková notace, případně je možné použít "[]" pro označení, že se jedná o kolekci. <br /> <br /> V tabulce lze též definovat výchozí StringFormat, který se momentálně využije pouze v dokumentech. |
| InputParameter | Číselník, který obsahuje vlastnosti objektu `InputParameters`, který sdržuje všechny potřebné vstupní parametry pro jednotlivé služby definové v DataService. Tento číselník se využívá pro dynamické načítání vstupních parametrů = načtení ID z výsledku vráceného jednou z DataService. |

## Konfigurace dokumentů

### Konfigurační tabulky

| **Název** | **Popis** |
|--|--|
| Document | Číselník pro typy dokumentů, především pro zachování integrity v DB. |
| DocumentDataField | Záznamy definují mapování z DataService do acrofieldů v dokumentu. Lze zde nastavit jak StringFormat, tak i výchozí text, pokud by načtená hodnota byla null. |
| DocumentSpecialDataField | Speciální mapování atributů, které nejsou normálně obsaženy v DataService či obsahují specifickou logiku. Každý dokument v kódu může mít svůj konkrétní objekt, kde se daná logika zachytí a v této tabulce se dopíše mapování. |
| DocumentTable | Obsahuje definici generické tabulky (momentálně pouze splátkový kalendář). Nastavuje se zde název acrofieldů, který slouží jako placeholder pro zjištění velikosti a pozice tabulky. Také se zde uvádí závěrečný paragraf, který se může vytisknout na konec tabulky. |
| DocumentTableColumn | Záznamy určují kolik sloupců má tabulka obsahovat. Definuje se zde název sloupce, StringFormat, který se aplikuje na všechny záznamy v tomto sloupci, a také velikost sloupce v procentech. |
| DocumentDynamicInputParameter | Definuje dynamické parametry, které se mají získat z výsledků již zavolané služby. Například na vstup se uvede SalesArrangementId a z načteného SalesArrangementu se načte CaseId a OfferId. |
| DynamicStringFormat | Záznamy nastavují dynamický StringFormat pro jednotlivá pole z tabulky DocumentDataField. Zvolí se takový StringFormat, který splňuje první (dle Order) všechny podmínky. |
| DynamicStringFormatCondition | Podmínky pro určení dynamického StringFormatu. |

### SQL skripty
  
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

## Konfigurace JSON věty

### Konfigurační tabulky

|  **Název** | **Popis** |
|--|--|
| EasRequestType | Číselník obsahující typ žádostí - produktová nebo servisní |
| EasFormType | Číslník obsahující název jednotlivých formulářů. Též se zde nastavuje verze formuláře a jeho platnost. |
| EasFormDataField | Obdoba DocumentDataField. Specifikuje mapování z výsledků služby do JSONu. Pro uvedení cesty v JSONu se využívá tečková notace, případně "[]" pro označení kolekce. |
| EasFormSpecialDataField | Mapuje specifické atributy, které neobsahují generické služby. V kódu pro každou žádost existují vlastní objekty, kde je možné napsat konkrétní vlastnosti a ty poté zde namapovat. |
| EasFormDynamicInputParameter | Stejně jako u dokumentů, definují se zde vstupní parametry, které se mají dotáhnout z výsledků již zavolaných služeb. |

### SQL skripty

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