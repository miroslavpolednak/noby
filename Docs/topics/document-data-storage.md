# Ukládání datových struktur
Pro ukládání obecných datových struktur (instance dat) ve formátu JSON používáme .

## Příklady použítí:

```csharp
private readonly IDocumentDataStorage _documentDataStorage;

// ID entity - např. HouseholdId, CaseId atd.
var entityId = 1;
// objekt instance dat
var document = new MyModel { Amount = 100, Confirmed = true };

// založení nové instance dat
var id = await _documentDataStorage.Add(entityId, document, cancellationToken);

// update existující instance dat dle ID instance
await _documentDataStorage.Update(id, document);

// update existující instance dat dle ID entity
await UpdateByEntityId.Update(entityId, document);

// získat z databáze instanci dat dle ID instance
var documentEntity = await _documentDataStorage.FirstOrDefault<MyModel>(id, cancellationToken)

// získat seznam instancí dat dle ID entity
var list = await _documentDataStorage.GetList<MyModel>(entityId, cancellationToken);

// smazání instance dat dle ID instance
// deletedRows je počet smazaných záznamů - tj. 0 znamená, že se smazání nepovedlo, protože ID neexistuje
var deletedRows = await _documentDataStorage.Delete<MyModel>(id);

// smazání instance dat dle ID entity
var deletedRows = await _documentDataStorage.DeleteByEntityId<MyModel>(entityId);
```