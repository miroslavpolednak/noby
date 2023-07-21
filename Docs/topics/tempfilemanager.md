# Práce s dočasnými soubory na FE API
Pro ukládání dočasných (temp) souborů je možné na FE API používáme `NOBY.Infrastructure.Services.TempFileManager.ITempFileManagerService`.
Tato služba zapisuje soubory na temp uložiště a zároveň jejich metadata do databáze. 
Temp soubory jsou identifikovány GUIDem, podle kterého se dají zpětně dohledat (včetně metadat jako název souboru, MIME typ atd.).

```csharp
private readonly ITempFileManagerService _tempFileManager;

IFormFile file = ...;

// Uložení nového temp souboru
var response = await _tempFileManager.Save(file, cancellationToken);
var id = response.TempFileId; // TempFileId obsahuje jedinečný GUID daného souboru

// Získání obsahu temp souboru (byte[])
var content = await _tempFileManager.GetContent(id, cancellationToken);

// Získání metadat o temp souboru
var metadata = await _tempFileManager.GetMetadata(id, cancellationToken);

// Smazání temp souboru
await _tempFileManager.Delete(id, cancellationToken);
```

K metadatům ukládaného temp souboru se dají připojit další informace ke kterému objektu byl daný soubor nahráván, případně session identifikátor.
K tomu slouží složitější přetížení metody Save.

```csharp
Task<TempFile> Save(
    IFormFile file,
    long? objectId = null,
    string? objectType = null,
    Guid? sessionId = null,
    CancellationToken cancellationToken = default);
```

