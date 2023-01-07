# Contracts projekt v gRPC službách
Projekt slouží jako samostatná definice a) gRPC služby, b) kontraktů / messages gRPC služby.
Obsahuje *.proto soubory kontraktů a C# partial třídy pro zjednodušení napojení *MediatR* pipeline na gRPC request.

Pro každou službu a verzi musí existovat právě jeden soubor s gRPC definicí služby. 
Konvence pro pojmenování souboru je: `{název služby}.{verze služby}.proto`
Např. tedy *CaseService.v1.proto*.  
Tento soubor vždy obsahuje pouze definici služby, např.:
```
service HouseholdService {
    rpc CreateHousehold (CreateHouseholdRequest) returns (CreateHouseholdResponse);
}
```

Dále jsou v projektu soubory s definicemi kontraktů jednotlivých endpointů. 
Konvence pro pojmenování těchto souborů je následovná:
- pokud se jedná o definici request a response objektu endpointu, jmenuje se soubor podle názvu daného endpointu a obsahuje oba kontrakty (request i response).  
Např. *CreateHousehold.proto*
- pokud se jedná o definici samostatného objektu, je soubor pojmenován podle daného objektu.  
Např. *Household.proto*
- *proto* soubory s kontrakty služby jsou uložené v podadresáři `Messages`.

## Podpora pro použití proto kontraktu v MediatR pipeline
Abychom nemuseli pro každý MediatR handler / gRPC endpoint vytvářet umělou `IRequest` třídu, můžeme využít toho, že **ProtoC** generuje všechny *proto* kontrakty jako `partial` class.
To nám umožňuje přidat do definice kontraktu další interface a použít ho jako request pro *MediatR*.
Partial class se vytváří pro každou request message v *proto* kontraktu a má následující tvar:

```
public partial class CreateHouseholdRequest
    : MediatR.IRequest<CreateHouseholdResponse>
```
V případě, že má request podporovat *FluentValidation*, přidáme ještě interface `IValidatableRequest`:
```
public partial class CreateHouseholdRequest
    : MediatR.IRequest<CreateHouseholdResponse>, CIS.Core.Validation.IValidatableRequest
```

## Adresářová struktura
Pokud projekt obsahuje pouze jednu službu, jsou všechny .proto soubory v rootu projektu.
Pokud obsahuje více služeb, je každá služba (definice a kontrakty) ve vlastním adresáři.

Každá služba má vlastní adresář **Partials**, který obsahuje partial třídu s implementací *MediatR*.

## Nastavení *.csproj
Pro správnou funkčnost ProtoC toolu pro generování C# tříd z proto souborů je nutné mít správně nastavený .NET projekt.
Každý .proto soubor v projektu musí mít nastavený způsob kompilace a odkaz na společně sdílené soubory.

Ukázka elementu v .csproj pro soubor s definicí služby:
```
<Protobuf Include="CustomerOnSAService.v1.proto" GrpcServices="Both" ProtoRoot="/" AdditionalImportDirs="../../../CIS/Infrastructure.gRPC.CisTypes/Protos" />
```

Ukázka elementu v .csproj pro soubor s definicí kontraktu:
```
<Protobuf Include="CustomerOnSA.proto" GrpcServices="None" ProtoRoot="/" AdditionalImportDirs="../../../CIS/Infrastructure.gRPC.CisTypes/Protos" />
```

## Hlavičky .proto souborů
Každému .proto souboru je nutné nastavit správně hlavičku. Nastavujeme následovné:
- Verzi protobuf  
Vždy `syntax = "proto3";`
- Název balíku  
Název odvozujeme z názvu služby s prefixem DomainServices. Např. `package DomainServices.HouseholdService;`
- Namespace vygenerované C# třídy
Název odvozujeme z názvu služby. Např. `option csharp_namespace = "DomainServices.HouseholdService.Contracts";`

Hlavička každého .proto souboru tedy vypadá takto:
```
syntax = "proto3";
package DomainServices.HouseholdService;
option csharp_namespace = "DomainServices.HouseholdService.Contracts";
```

## NOBY gRPC custom types
V projektu `CIS.Infrastructure.gRPC.CisTypes` jsou definované vlastní proto objekty používané obecně ve všech službách.
Většina těchto typů má zároveň definované implicitní konverze na nativní .NET typy, takže není nutné je přetypovávat.  
Jedná se zejména o (ale nikoliv pouze):
- GrpcDate - C# `DateOnly`
- NullableGrpcDate - C# `DateOnly?`
- GrpcDateTime - C# `DateTime`
- NullableGrpcDateTime - C# `DateTime?`
- GrpcDecimal - C# `decimal`
- NullableGrpcDecimal - C# `decimal?`
- UserIdentity - informace o klientovi (schema a ID)

Tyto typy je možné v kontraktech použít následovně:
- nejdříve se typ musí naimportovat:
```
import "NullableGrpcDate.proto";
```
- poté je možné ho použít v message:
```
message SalesArrangement {
    cis.types.NullableGrpcDate OfferGuaranteeDateFrom = 12;
}
```