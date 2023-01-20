## Generate FormId
grpcurl -insecure -d "{\"HouseholdId\":1}" -H "Authorization: Basic YTph" 127.0.0.1:30019 DomainServices.DocumentOnSAService.v1.DocumentOnSAService/GenerateFormId

## Migrations
1) Open Package manager console
2) Set project with dbcontext as startpup project (DomainServices.DocumentArchiveService.Api)
3) In package manager console set Default project to the project with dbcontext (DomainServices.DocumentArchiveService.Api)
## Add migration
Add-Migration <YourMigrationName> -OutputDir "Database/Migrations"
## Update database to latest migration
Update-Database
## or to specific migration
Update-Database  <YourMigrationName>
## if you want create sql from migration see
https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/applying?tabs=vs