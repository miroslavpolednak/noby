## grpcurl tests

### GenerateDocumentId
```
grpcurl -insecure -d "{\"EnvironmentName\":1}" -H "Authorization: Basic WFhfTk9CWV9STVRfVVNSX1RFU1Q6cHBtbGVzbnJUV1lTRFlHRFIhOTg1Mzg1MzU2MzQ1NDQ=" 127.0.0.1:30005 DomainServices.DocumentArchiveService.v1.DocumentArchiveService/GenerateDocumentId
```
### GetDocument
```
grpcurl -insecure -d "{\"documentId\":\"KBHCWS000000000000000013832943\"}" -H "Authorization: Basic WFhfTk9CWV9STVRfVVNSX1RFU1Q6cHBtbGVzbnJUV1lTRFlHRFIhOTg1Mzg1MzU2MzQ1NDQ=" 127.0.0.1:30005 DomainServices.DocumentArchiveService.v1.DocumentArchiveService/GetDocument
```
### GetGetDocumentList
```
grpcurl -insecure -d "{\"CaseId\":12345}" -H "noby-user-id: 3048" -H "noby-user-ident: KBUID=A09FK3" -H "Authorization: Basic WFhfTk9CWV9STVRfVVNSX1RFU1Q6cHBtbGVzbnJUV1lTRFlHRFIhOTg1Mzg1MzU2MzQ1NDQ=" 127.0.0.1:30005 DomainServices.DocumentArchiveService.v1.DocumentArchiveService/GetGetDocumentList
```
### UploadDocument 
**If you want use this call, you have to generate document id (GenerateDocumentId) first and past it as DocumentId parameter!!!** 
```
grpcurl -insecure -d "{\"BinaryData\":\"VGhpcyBpcyBhIHRlc3Q=\",\"Metadata\":{\"CaseId\":131,\"DocumentId\":\"KBHXXD00000000000000000000009\",\"Filename\":\"test.txt\",\"AuthorUserLogin\":\"a\",\"EaCodeMainId\":1,\"CreatedOn\":{\"year\":2022,\"month\":9,\"day\":25}}}" -H "Authorization: Basic WFhfTk9CWV9STVRfVVNSX1RFU1Q6cHBtbGVzbnJUV1lTRFlHRFIhOTg1Mzg1MzU2MzQ1NDQ=" 127.0.0.1:30005 DomainServices.DocumentArchiveService.v1.DocumentArchiveService/UploadDocument
```
### GetDocumentsInQueue
```
grpcurl -insecure -d "{\"EArchivIds\":[\"KBHXXD00000000000000000000007\",\"KBHXXD00000000000000000000009\"]}" -H "Authorization: Basic WFhfTk9CWV9STVRfVVNSX1RFU1Q6cHBtbGVzbnJUV1lTRFlHRFIhOTg1Mzg1MzU2MzQ1NDQ=" 127.0.0.1:30005 DomainServices.DocumentArchiveService.v1.DocumentArchiveService/GetDocumentsInQueue
```
### Run batch
```
dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\DomainServices\DocumentArchiveService\Api\DomainServices.DocumentArchiveService.Api.csproj"
```
### Migrations
1) Open Package manager console
2) Set project with dbcontext as startpup project (DomainServices.DocumentArchiveService.Api)
3) In package manager console set Default project to the project with dbcontext (DomainServices.DocumentArchiveService.Api)
### Add migration
```
EntityFrameworkCore\Add-Migration <YourMigrationName> -OutputDir "Database/Migrations"
```
### Update database to latest migration
```
EntityFrameworkCore\Update-Database
```
### or to specific migration
```
EntityFrameworkCore\Update-Database  <YourMigrationName>
```
### if you want create sql from migration see
https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/applying?tabs=vs