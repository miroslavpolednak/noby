# Databáze - čtení, zápis dat
Pro práci s relačními databázemi (MS SQL server, Oracle) je možné použít Entity Framework nebo Dapper.
Pro CRUD operace používáme výhradně Entity Framework, který automaticky doplňuje informace o CRUD operaci (viz. níže).
Dapper je možné použít pro čtení dat v případě, kdy je výhodnější napsat vlastní SQL query než se dotazovat LINQem v EF.

Striktně dodržujeme architekturu **databáze x aplikace** - tj. pokud aplikace nebo služba potřebuje databázi pro perzistenci vlastních dat, vždy má vlastní databázi.
Tato databáze je určena pouze pro danou službu a žádná jiná aplikace nebo služba z ní nemůže číst nebo zapisovat.
Pokud jiná aplikace potřebuje provádět operace nad databází této služby, vždy se tak děje přes gRPC API vystavené danou službou.

V případě potřeby auditu změn na databázových entitách používáme na MS SQL serveru feature [Temporal Tables](https://learn.microsoft.com/en-us/sql/relational-databases/tables/temporal-tables?view=sql-server-ver16).

## Dapper
*Dapper* používáme výhradně pro READ operace. Jeho výhodou je lepší práce s raw SQL dotazy.
Pro použití *Dapper* je nutné z DI získat instanci interface `CIS.Core.Data.IConnectionProvider`.
Interface `IConnectionProvider` má i generickou variantu. 
Pokud aplikace využívá více databází (více connection stringů), tak generickou variantou interface odlišujeme jednotlivé databáze.
Jako generický parametr používáme umělý marker interface, který nemá v rámci žádnou jinou funkci.

*Dapper* connection se registruje při startupu aplikace extension metodou `AddDapper(string connectionString)` (namespace `CIS.Infrastructure.StartupExtensions`).
```
// jeden connection string / instance Dapper connection v aplikaci
builder.Services.AddDapper(builder.Configuration.GetConnectionString("default")!);

// více různých connection stringů
builder.Services.AddDapper<IXxdDatabase>(builder.Configuration.GetConnectionString("xxd")!);
builder.Services.AddDapper<INobyDatabase>(builder.Configuration.GetConnectionString("noby")!);
```

Následně je možné z DI získat instanci connection objektu:
```
class T {
	// jeden connection string / instance Dapper connection v aplikaci
	public ctr(IConnectionProvider conn) { }

	// více různých connection stringů
	public ctr(IConnectionProvider<IXxdDatabase> conn1, IConnectionProvider<NobyDatabase> conn2) { }
}
```

V projektu `CIS.Infrastructure` jsou v namespace `CIS.Infrastructure.Data` extension metody pro materializaci objektů nebo listů z SQL dotazu.
- ExecuteDapperRawSqlToList()
- ExecuteDapperRawSqlFirstOrDefault()
- ExecuteDapperQuery()
```
// načtení celého recordsetu 
await _connectionProvider.ExecuteDapperRawSqlToList<MyItem>("SELECT * FROM Table", cancellationToken);

// načtení jednoho objektu
long x = await _connectionProvider.ExecuteDapperRawSqlFirstOrDefault<long>("SELECT TOP 1 Id FROM Table", cancellation);
```

## Entity Framework
*Entity Framework* **DbContext** se registruje při startupu aplikace extension metodou `AddEntityFramework<TDbContext>()` v namespace `CIS.Infrastructure.StartupExtensions`.
```
builder.AddEntityFramework<CaseServiceDbContext>();
```

### DbContext
Každý EF *DbContext* musí dědit z bázové třídy `CIS.Infrastructure.Data.BaseDbContext<>` z projektu `CIS.Infrastructure`.
Příklad implementace *DbContextu*:
```
internal sealed class CaseServiceDbContext
    : BaseDbContext<CaseServiceDbContext>
{
    public CaseServiceDbContext(BaseDbContextAggregate<CaseServiceDbContext> aggregate)
        : base(aggregate) { }
	...
}
```

Použitím bázové třídy `BaseDbContext` získává *DbContext* schopnost automaticky trackovat kdo a kdy provedl změny na entitách.

### Automatické trackování změn
Entity, u kterých to dává smysl, mohou ukládat informace o svém vytvoření a/nebo updatu.
Tyto informace jsou uložené formou databázových polí:
- `CreatedUserName` (nvarchar): jméno a příjmení uživatele, který entitu vytvořil
- `CreatedUserId` (int): v33id uživtele, který entitu vytvořil
- `CreatedTime` (datetime): čas vytvoření entity
- `ModifiedUserName` (nvarchar): jméno a příjmení uživatele, který entitu naposledy updatoval
- `ModifiedUserId` (int): v33id, který entitu naposledy updatoval
Entita může obsahovat pouze pole `Created...` (tj. pouze informace o vytvoření) nebo i pole `Modified...` (tj. informace o updatu).
Entita nemůže obsahovat pouze pole `Modified...`.

Plnění těchto polí zajišťuje přetížená metoda `DbContext.SaveChanes()`.
Aby tato metoda věděla jak s entitami pracovat, musí tyto dědit z marker interface v namespace `CIS.Core.Data`.
- `ICreated`: pole CreatedUserName, CreatedUserId a CreatedTime
- `IModifiedUser`: pole ModifiedUserName ModifiedUserId

Pro jednodušší implementaci obsahuje namespace `CIS.Core.Data` i bázové třídy pro jednotlivé interface: `BaseCreated`, `BaseModifiedUser`, `BaseCreatedWithModifiedUserId`.

Speciálním polem je také `IsActual` (bit). Toto pole obsahuje příznak smazání entity v případě soft-delete.
V případě použití soft-delete musí entity implementovat interface `IIsActual`, případně bázovou třídu `BaseIsActual`.

