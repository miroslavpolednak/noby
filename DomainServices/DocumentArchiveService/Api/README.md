## grpcurl tests

## GenerateDocumentId
grpcurl -insecure -d "{\"EnvironmentName\":1}" -H "Authorization: Basic YTph" 127.0.0.1:30017 DomainServices.DocumentArchiveService.v1.DocumentArchiveService/GenerateDocumentId
## GetDocument
grpcurl -insecure -d "{\"documentId\":\"KBHCWS000000000000000013832943\"}" -H "Authorization: Basic YTph" 127.0.0.1:30017 DomainServices.DocumentArchiveService.v1.DocumentArchiveService/GetDocument
## GetGetDocumentList
grpcurl -insecure -d "{\"CaseId\":12345}" -H "Authorization: Basic YTph" 127.0.0.1:30017 DomainServices.DocumentArchiveService.v1.DocumentArchiveService/GetGetDocumentList
## UploadDocument: If you want use this call, you have to generate document id (GenerateDocumentId) first and past it as DocumentId parameter!!! 
grpcurl -insecure -d "{\"BinaryData\":\"VGhpcyBpcyBhIHRlc3Q=\",\"Metadata\":{\"CaseId\":131,\"DocumentId\":\"KBHXXD00000000000000000000009\",\"Filename\":\"test.txt\",\"AuthorUserLogin\":\"a\",\"EaCodeMainId\":1,\"CreatedOn\":{\"year\":2022,\"month\":9,\"day\":25}}}" -H "Authorization: Basic YTph" 127.0.0.1:30017 DomainServices.DocumentArchiveService.v1.DocumentArchiveService/UploadDocument


## run batch
dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\DomainServices\DocumentArchiveService\Api\DomainServices.DocumentArchiveService.Api.csproj"

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