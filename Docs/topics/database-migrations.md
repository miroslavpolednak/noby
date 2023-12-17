# Databázové migrace
Databázové migrace provádíme spouštěním SQL skriptů, které obsahují požadované migrace.
Každá služba / aplikace má vlastní adresář s migračními skripty - ten se vždy jmenuje **DatabaseMigrations** a je na stajné úrovni jako adresář aplikace:
```
[DomainServices.HouseholdService]
  [Api]
  [Contracts]
  [Clients]
  [DatabaseMigrations]
    00001-script.sql
    00002-script.sql
```

Konvence pro pojmenování SQL skriptů je `{číslo-pořadí skriptu:00000}-{JIRA task (pokud existuje)}-{popis}.sql`, např.:
```
00002-HFICH-1587-pridani_noveho_sloupce.sql
```
Číslo pro pořadí skriptů je manuálně udržované a není třeba aby se jednalo o souvislou řadu, ani nevadí pokud se budou čísla duplikovat.
Jedná se pouze o hint pro migrační tool v jakém pořadí skripty případně pouštět.

Jakmile se SQL skript dostane do *master* nebo *release* branch v GITu, bude s prvním spuštěním CI/CD proveden nad databází dané služby.
Migrace se pouští automaticky v rámci DevOps release pipeline pomocí vlastní konzolové aplikace **DatabaseMigrations**.

## Migrace s použitím C# kódu
V případě komplexních migracích, kdy je jednodušší použít C# kód místo SQL, je možné použít .NET migrace podporované v *DbUp* - viz. https://dbup.readthedocs.io/en/latest/usage/ část "**Code-based scripts**".

.NET migrace je standardní C# třída, která implementuje interface `IScript`.
Tato třída se musí nacházet a v *Api* projektu dané služby v adresáři *Database/CodeMigrations*.

Struktura adresářů s code migrations skripty:
```
[DomainServices.OfferService.Api]
  [Database]
    [CodeMigrations]
      MyMigration1.cs
      MyMigration2.cs
```

Protože migrační konzolová aplikace spouští skripty postupně v pořadí podle jejich názvu (nezávisle na tom, zda se jedná o SQL nebo .NET migraci), je nutné správně pojmenovat C# migrační třídu.
K tomu slouží atribut `DbUpScriptName`, který umožňuje nastavit jméno migračního skriptu pro *DbUp* bez ohledu na skutečné jméno třídy / namespace.

> Pro podporu .NET based migrací je potřeba referencovat projekt **DatabaseMigrationsSupport**

> Aby migrační konzolová aplikace spustila .NET migrace, je nutné při jejím startu použít parametr "**a**", který nastavuje cestu k DLL s Api projektem dané služby

### Příklad migračního skriptu
```csharp
[DatabaseMigrationsSupport.DbUpScriptName("00002_my_script")]
public class Script0005ComplexUpdate : IScript
{
    public string ProvideScript(Func<IDbCommand> commandFactory)
    {
        var cmd = commandFactory();
        cmd.CommandText = "Select * from SomeTable";
        var scriptBuilder = new StringBuilder();

        using (var reader = cmd.ExecuteReader())
        {
            while (reader.Read())
            {
                scriptBuilder.AppendLine(string.Format("insert into AnotherTable values ({0})", reader.GetString(0)));
            }
        }

        return scriptBuilder.ToString();
    }
}
```

## Konzolová aplikace DatabaseMigrations
Jedná se o samostatný projekt v rámci solution NOBY. Aplikace používá **DbUp** (https://dbup.readthedocs.io/en/latest/) pro provádění migrací.
**DbUp** zajistí, aby se skripty spouštěli pouze pokud je to požadováno - buď proto, že se jedná o nový skript, nebo také v případě rekurentního skriptu.

Spuštěním aplikace se zobrazí help s možnými parametry.

Příklad spuštění migrací:
```
DatabaseMigrations.exe -c "Data Source=localhost;Initial Catalog=MpssIntegration;Integrated Security=True;Encrypt=True;TrustServerCertificate=Yes;" -f "d:\Visual Studio Projects\MPSS-FOMS\DomainServices\HouseholdService\DatabaseMigrations"
```
