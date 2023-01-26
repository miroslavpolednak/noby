# Databázové migrace
Databázové migrace provádíme spouštěním SQL skriptů, které obsahují požadované migrace.
Každá služba / aplikace má vlastní adresář s migračními skripty - ten se vždy jmenuje **DatabaseMigrations** a je na stajné úrovni jako adresář aplikace:
```
[DomainServices.HouseholdService]
  [Api]
  [Contracts]
  [Clients]
  [DatabaseMigrations]
```

Konvence pro pojmenování SQL skriptů je `{číslo-pořadí skriptu:000}-{JIRA task (pokud existuje)}-{popis}.sql`, např.:
```
002-HFICH-1587-pridani_noveho_sloupce.sql
```
Číslo pro pořadí skriptů je manuálně udržované a není třeba aby se jednalo o souvislou řadu, ani nevadí pokud se budou čísla duplikovat.
Jedná se pouze o hint pro migrační tool v jakém pořadí skripty případně pouštět.

Jakmile se SQL skript dostane do *master* nebo *release* branch v GITu, bude s prvním spuštěním CI/CD proveden nad databází dané služby.
Migrace se pouští automaticky v rámci DevOps release pipeline pomocí vlastní konzolové aplikace **DatabaseMigrations**.

## Konzolová aplikace DatabaseMigrations
Jedná se o samostatný projekt v rámci solution NOBY. Aplikace používá DbUp (https://dbup.readthedocs.io/en/latest/) pro provádění migrací.
DbUp zajistí, aby se skripty spouštěli pouze pokud je to požadováno - buď proto, že se jedná o nový skript, nebo také v případě rekurentního skriptu.

Příklad spuštění migrací:
```
DatabaseMigrations.exe -c "Data Source=localhost;Initial Catalog=MpssIntegration;Integrated Security=True;Encrypt=True;TrustServerCertificate=Yes;" -f "d:\Visual Studio Projects\MPSS-FOMS\DomainServices\HouseholdService\DatabaseMigrations"
```
