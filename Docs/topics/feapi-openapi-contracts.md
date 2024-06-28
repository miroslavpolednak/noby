# Generování kontraktů FE API z OpenApi specifikace

## Základní nastavení
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

# DEV dokumentace
Kontrakty a partial třídy pro FE API jsou generovány z OpenApi specifikace a jsou umístěny v projektu **NOBY.ApiContracts** v souboru **Contracts.cs**.
Kontrakty jsou vygenerovány pomocí nástroje *NSwag*, nastavení pro NSwag je uloženo v souboru **settings.nswag** v projektu *NOBY.ApiContracts*.

Projekt **NOBY.ApiContracts** dále obsahuje rozšíření (partial classes) pro třídy vygenerované *NSwagem*.
Zejména se jedná o Mediatr requesty u kterých je potřeba implementovat rozhraní `IRequest`, případně metodu `InfuseId`.
Partial třídy requestů jsou umístěny v adresáři **PartialRequests**.