# SQLite SetUp

## SQLite Install

    - stáhnout si SQLite [sqlite-tools-win32-x86-3400000.zip], viz. links
    - obsah toho ZIPu si uložit [např. do 'C:\sqlite']
    - rošířit proměnnou prostředí 'Path' o cestu k té složce ['C:\sqlite']
    - vyzkoušet v CML příkaz 'sqlite3'


## SQLite DB

    - databáze je obsahem složky [viz. NobyTest.db] (vytvoření nové: '$sqlite3 DatabaseName.db', viz. links)
    - v kódu (soubor 'run_script.py') upravit proměnné 'path_to_db' a 'path_to_sql'
    - pak už stačí kód spustit (vymaže db a vytvoří tabulky pro simulaci Offer)

    - poznámka: v db skriptu [OfferService.sql] je u každého sloupce tabulky v komentáři přesný datový typ, viz. ProtoTypes 

## ProtoTypes:
    - bool
    - cis.types.GrpcDate
    - cis.types.GrpcDecimal
    - cis.types.ModificationStamp
    - cis.types.NullableGrpcDate
    - cis.types.NullableGrpcDecimal
    - google.protobuf.BoolValue
    - google.protobuf.Int32Value
    - google.protobuf.StringValue
    - int32
    - string
    
## Links

    - download SQLite: https://www.sqlite.org/download.html
    - SQLite tutorial: https://www.tutorialspoint.com/sqlite/sqlite_create_database.htm


