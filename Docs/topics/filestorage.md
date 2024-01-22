# Práce se standardním úložištěm souborů (filesystem, S3, Azure)
Pro práci se soubory vždy používáme projekt `SharedComponents.Storage`, nikdy nepřistupujeme např. na filesystem přímo přes `System.IO`.  
`SharedComponents.Storage` umožňuje definovat libovolné množství různých Storage klientů, které zajišťují přístup na dané úložiště.  
Aktuálně podporujeme **FileSystem, Azure Blob, Amazon S3**.

Pro práci se souborem na konkrétním úložišti je nedříve potřeba vytvořit Storage klienta typu `IStorageClient<TStorage>`.
`TStorage` je v tomto případě marker interface, který identifikuje/pojmenovává daného klienta.  
Instance klienta je dostupná v DI kontajneru.

```csharp
// Marker interface daného úložiště
interface IStorage1 { }

class MyHandler {
    private readonly IStorageClient<IStorage1> _storageClient;

    public MyHandler(IStorageClient<IStorage1> storageClient) {
        _storageClient = storageClient;
    }

    public async Task Handle() {
        // Použití storage klienta
        await _storageClient.SaveFile(...);
    }
}
```

## Obecná konfigurace Storage komponenty
Storage komponenta je konfigurována standardně v *appsettings.json* ve vlastním elementu **CisStorage**.

```json
{
    "CisStorage": {
        "StorageClients": { // definice jednoho nebo více Storage klientů
            ...
        }
    }
}
```

## Konfigurace storage klienta
Každý Storage klient má vlastní konfigurační objekt v **CisStorage:StorageClients**.
Obrazem konfigurace klienta je třída `SharedComponents.Storage.Configuration.StorageClientConfiguration`.
Klíč pod kterým je konfigurace uložena je názvem marker interface.

```json
"StorageClients": {
    "IStorage1": {
        "StorageType": 1,
        "FileSystem": {
            "BasePath": "c:/temp3"
        }
    },
    ...
}
```

Každý typ Storage klienta má vlastní konfigurační podobjekt, který je poplatný pouze pro daný typ:
- FileSystem
- AzureBlob
- AmazonS3

