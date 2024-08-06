# Generování kontraktů FE API z OpenApi specifikace

## Práce se společnou OAS
Sdílená dokumentace je v souboru **FEAPI.json**, který je v repository `OpenAPI`, branch `master`. 
Tato verze OAS by měla zrcadlit stav v `master` branch NOBY DEV.

Flow zadávání změna v OAS:
1) ITA definuje změny ve vlastní feature branch
2) feature branch projde review ze strany BE a FE DEV. 
3) 
    a) následně je buď feature branch rovnou mergována do `OpenAPI/master`
    b) nebo na něm bude probíhat vývoj ze strany DEV. Jakmile je vývoj ukončen, DEV merguje feature branch do `OpenAPI/master`.

Ideálně bude v rámci DevOps pipelines existovat proces, který během buildu aplikace zkompiluje aktuální OAS z OpenAPI repa a nedovolí nasadit aplikaci, pokud se kontrakty mezi aplikací a OAS budou lišit.

## Základní nastavení OAS
- každé schéma musí obsahovat `"additionalProperties": false`, jinak se do C# bude generovat slovník ostatních vlastností.
- důsledně používáme `"nullable": true` v případě, že se jedná o nullable property nebo schéma.

```json
"CasesSharedCaseOwnerModel": {
	"type": "object",
	"properties": {
		"cpm": {
			"type": "string",
			"nullable": true
		}
	},
	"additionalProperties": false
},
```

## Jmenné konvence
Název objektu (OpenApi schématu) je složen z namespace (části aplikace v kontextu které je objekt používán), názvu endpointu a suffixu.  
Např. *CustomerIncomeCreateIncomeRequest* je složen takto:

```
[CustomerIncome][CreateIncome][Request]  
 namespace       endpoint      suffix
```

### Standardní OpenApi objekty (schémata)
- Request (vstup do endpointu) - suffixován "**Request**", např. "CustomerIncomeCreateIncomeRequest"
- Response (výstup endpointu) - suffixován "**Response**", např. "CustomerIncomeCreateIncomeResponse"
- Enum (výčet hodnot) - prefixován "**Enum**" a vždy v množném čísle, např. "*EnumRealEstateValuationTypes*".
Pokud se jedná o enum pro konkrétní namespace, obsahuje v názvu i tento namespace.  
Tj. *[Enum][RealEstateValuation][Types]*

### Objekty společné pro celou aplikaci
Název objektu prefixujeme "**SharedTypes**":  
Tj. *[SharedTypes][SigningSignatureState]*
```json
"SharedTypesSigningSignatureState": { ...  }
```

### Objekty společné pro namespace (část aplikace)
Název objektu sdílí prefix namespace a přidává klíčové slovo "Shared":  
Tj. *[CustomerIncome][Shared][DataEmployement]*
```json
"CustomerIncomeSharedDataEmployement": { ...  }
```

## Datové typy

**DateTime** - datum s časem (2017-05-16 15:25:35.867)
```json
"createdOn": {
	"type": "string",
	"format": "date-time"
}
```

**Date** - pouze datum (2017-05-16)
```json
"createdOn": {
	"type": "string",
	"format": "date"
}
```

**Decimal** - číslo s desetinnou čárkou, nepoužíváme nikdy double
```json
"flatArea": {
	"type": "number",
	"format": "decimal"
}
```

**Enum** - výčtový typ
Enum definujeme jako OpenApi schéma a následně jej referencujeme pomocí *$ref*.  
Enum má vždy typ integer / int32 a obsahuje props **x-enum-varnames** a **x-enumNames**, které definují názvy položek enumu v C#.
```json
"EnumWorkflowTaskStateFilters": {
	"enum": [0, 1, 2],
	"type": "integer",
	"format": "int32",
	"x-enum-varnames": ["Unknown", "Active", "Finished"],
	"x-enum-descriptions": ["Unknown", "Active", "Finished"],
	"x-enumNames": ["Unknown", "Active", "Finished"]
}

Následně je možné enum používat v ostatních schématech:
"stateFilter": {
	"$ref": "#/components/schemas/EnumWorkflowTaskStateFilters"
}
```

## Další témata

### Dědičnost
OpenApi podporuje dědičnost pomocí tagu "**allOf**". 
C# podporuje pouze jeden base model, tj. pole *allOf* musí obsahovat vždy pouze jedno schéma.
```json
"CustomerIncomeBaseIncome": {
	"type": "object",
	"properties": {
	  ...
	},
	"additionalProperties": false
},

Následně objekt, který dědí z base modelu:
"CustomerIncomeCreateIncomeRequest": {
	"type": "object",
	"allOf": [{ "$ref": "#/components/schemas/CustomerIncomeBaseIncome" }],
	"properties": {
	  ...
	},
	"additionalProperties": false
}
```

### OneOf -> více možných typů objektů v jedné vlastnosti
OneOf funkčnost v OpenApi nepoužíváme, protože do C# generuje špatně kód. 
Abychom dokázali tuto funkčnost nahradit, používáme podobný přístup jako Protobuf.
OneOf je tedy objekt, který má tolik vlastností, kolika typů může výsledek nabývat.

> Objekt zastupující OneOf v OAS má vždy suffix **OneOf**.

K vytvořenému objektu, který zastupuje všechny OneOf typy přidáváme vlastnost `discriminator`, která obsahuje název objektu, který je v dané instanci naplněn.

Ukázka OpenApi specifikace:
```json
// implementace objektu, který obsahuje OneOf vlastnost -> "amendments" může nabývat různých typů
"SharedTypesWorkflowTaskDetail": {
	...
	"properties": {
		"amendments": {
			"$ref": "#/components/schemas/SharedTypesWorkflowTaskDetailAmendmentsOneOf",
			"description": "OneOf",
			"nullable": true
		}
	}
	...
}

// implementace OneOf objektu
"SharedTypesWorkflowTaskDetailAmendmentsOneOf": {
	"type": "object",
	"properties": {
		"discriminator": {
			"type": "string",
			"nullable": false
		},
		"consultationData": {
			"$ref": "#/components/schemas/SharedTypesWorkflowAmendmentsConsultationData",
			"nullable": true
		},
		"priceException": {
			"$ref": "#/components/schemas/SharedTypesWorkflowAmendmentsPriceException",
			"nullable": true
		}
	}
},
```

Ukázka výsledného JSONu:
```json
{
	"amendments": {
		"discriminator": "consultationData",
		"consultationData": { a: 1, b: 1 },
		"priceException": null
	}
}
```

# DEV dokumentace
Kontrakty a partial třídy pro FE API jsou generovány z OpenApi specifikace a jsou umístěny v projektu **NOBY.ApiContracts** v souboru **Contracts.cs**.
Kontrakty jsou vygenerovány pomocí nástroje *NSwag*, nastavení pro NSwag je uloženo v souboru **settings.nswag** v projektu *NOBY.ApiContracts*.

## Adresář PartialRequests
Projekt **NOBY.ApiContracts** dále obsahuje rozšíření (partial classes) pro třídy vygenerované *NSwagem*.
Zejména se jedná o Mediatr requesty u kterých je potřeba implementovat rozhraní `IRequest`, případně metodu `InfuseId`.
Partial třídy requestů jsou umístěny v adresáři **PartialRequests**.

Ukázka partial class pro request v adresáři **PartialRequest**:
```csharp
namespace NOBY.ApiContracts;

public partial class CustomerIncomeCreateIncomeRequest : IRequest<int>
{
    [JsonIgnore]
    public int? CustomerOnSAId { get; private set; }

    public CustomerIncomeCreateIncomeRequest InfuseId(int customerOnSAId)
    {
        this.CustomerOnSAId = customerOnSAId;
        return this;
    }
}
```

## Adresář Partials
V adresáři Partials jsou umístěné partial classes, které obsahují další logiku jinou než implementaci IRequest.
Zejména se jedná o implicitní / explicitní operátory atd.

## Enumy sdílené v řešení mimo FE API
Pokud je v kontraktu FE API použit enum, který je sdílený i v rámci doménových / interních služeb, chováme se k němu jinak než ke standardním enumům v OAS.

V OAS je enum definován jako každý jiný enum, nicméně navíc vytváříme jeho obraz i v projektu *SharedTypes* kde je definován jako standardní C# enum. 
Následně pak v nastavení NSwagu tento enum vyjmeme z generovaných typů v nastavení "Exluded Type Names".
Enumy v tomto režimu vždy prefixujeme "**Enum**", tj. místo `CaseStates` je název `EnumCaseStates`.

> Příkladem takového enumu je EnumCaseStates v `SharedTypes.Enums.EnumCaseStates`.
