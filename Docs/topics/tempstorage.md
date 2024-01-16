# Práce s dočasnými soubory
Pro ukládání dočasných (temp) souborů používáme službu `ITempStorage`, která je součástí namespace `SharedComponents.Storage` (projekt `SharedComponents.Storage`).
Tato služba zapisuje soubory na temp uložiště (filesystem, S3, Azure...) a zároveň jejich metadata do databáze. 
Temp soubory jsou identifikovány GUIDem, podle kterého se dají zpětně dohledat (včetně metadat jako název souboru, MIME typ atd.).

K metadatům ukládaného souboru se dají připojit další informace:
- ke které entitě (např. CustomerOnSA) byl daný soubor nahráván (vlastnosti `ObjectId`, `ObjectType`)
- session identifikátor (vlastnost `SessionId`)

> Podle `SessionId` se následně dají získat/smazat všechny soubory, které byly nahrány v dané session. To samé platí pro soubory uložené s informací o entitě.

## Použité exceptions
### TempStorageException
**90032**: jedná se o vyjímku, která je vyvolána při kontrole povolené koncovky.  
**90038**: jedná se o vyjímku, která je vyvolána při kontrole názvu souboru.

## Přidání temp storage do aplikace
Registrace temp storage během startupu aplikace (v `program.cs`):
```csharp
...
builder.AddCisStorageServices()
...
```

## Konfigurace temp storage
Konfigurace se provádí standardně v *appsettings.json* v sekci **CisStorage:TempStorage**.  
Obrazem konfigurace je třída `SharedComponents.Storage.Configuration.TempStorageConfiguration`.
Popis jednotlivých vlastností konfigurace je v komentářích této třídy.
```json
{
  "CisStorage": {
    "TempStorage": {
      "UseAllowedFileExtensions": false,
      "StoragePath": "c:/tempfiles"
    }
  }
}
```

## Příklady použití

### Základní použití ITempStorage
```csharp
private readonly SharedComponents.Storage.ITempStorage _tempFileManager;

IFormFile file = ...; // Souborová data odeslaná klientským browserem

// Uložení nového temp souboru
var response = await _tempFileManager.Save(file, cancellationToken);
var id = response.TempStorageItemId; // TempStorageItemId obsahuje jedinečný GUID daného souboru

// Získání obsahu temp souboru (byte[])
var content = await _tempFileManager.GetContent(id, cancellationToken);

// Získání metadat o temp souboru
var metadata = await _tempFileManager.GetMetadata(id, cancellationToken);

// Smazání temp souboru
await _tempFileManager.Delete(id, cancellationToken);
```

### Uložení souboru včetně informací o entitě, ke které je soubor ukládán
```csharp
long caseId = 1;
string entityType = "CASE";

await _tempFileManager.Save(file, caseId, entityType, cancellationToken);

// Získat všechny temp soubory uložené k dané entitě
var files = await _tempFileManager.GetByObjectType(entityType, caseId, cancellationToken);
```

## Čistění dočasného úložiště
??? měl by existovat job na serveru, který adresář bude čistit?
??? nebo to má být background job některé service?