## grpcurl tests

### GetAllGrpcServices
```
grpcurl -insecure 127.0.0.1:30019 list DomainServices.DocumentOnSAService.v1.DocumentOnSAService
```
### Generate FormId
```
grpcurl -insecure -d "{\"HouseholdId\":1}" -H "Authorization: Basic WFhfTk9CWV9STVRfVVNSX1RFU1Q6cHBtbGVzbnJUV1lTRFlHRFIhOTg1Mzg1MzU2MzQ1NDQ=" 127.0.0.1:30019 DomainServices.DocumentOnSAService.v1.DocumentOnSAService/GenerateFormId
```
### StartSigning
```
grpcurl -insecure -d "{\"SalesArrangementId\":6,\"DocumentTypeId\":4,\"SignatureMethodCode\":\"PHYSICAL\"}" -H "Authorization: Basic WFhfTk9CWV9STVRfVVNSX1RFU1Q6cHBtbGVzbnJUV1lTRFlHRFIhOTg1Mzg1MzU2MzQ1NDQ=" -H "noby-user-id: 1" -H "noby-user-ident: 990614w" 127.0.0.1:30019 DomainServices.DocumentOnSAService.v1.DocumentOnSAService/StartSigning
```
### StopSigning: if you want call this method, it is necessary call start signing first to get DocumentOnSaId 
```
grpcurl -insecure -d "{\"DocumentOnSAId\":3}" -H "Authorization: Basic WFhfTk9CWV9STVRfVVNSX1RFU1Q6cHBtbGVzbnJUV1lTRFlHRFIhOTg1Mzg1MzU2MzQ1NDQ=" 127.0.0.1:30019 DomainServices.DocumentOnSAService.v1.DocumentOnSAService/StopSigning
```
### GetDocumentsToSignList
```
grpcurl -insecure -d "{\"SalesArrangementId\":11}" -H "Authorization: Basic WFhfTk9CWV9STVRfVVNSX1RFU1Q6cHBtbGVzbnJUV1lTRFlHRFIhOTg1Mzg1MzU2MzQ1NDQ=" 127.0.0.1:30019 DomainServices.DocumentOnSAService.v1.DocumentOnSAService/GetDocumentsToSignList
```
### GetDocumentOnSAData
```
grpcurl -insecure -d "{\"DocumentOnSAId\":1}" -H "Authorization: Basic WFhfTk9CWV9STVRfVVNSX1RFU1Q6cHBtbGVzbnJUV1lTRFlHRFIhOTg1Mzg1MzU2MzQ1NDQ=" 127.0.0.1:30019 DomainServices.DocumentOnSAService.v1.DocumentOnSAService/GetDocumentOnSAData
```
### SignDocumentManually
```
grpcurl -insecure -d "{\"DocumentOnSAId\":1}" -H "Authorization: Basic WFhfTk9CWV9STVRfVVNSX1RFU1Q6cHBtbGVzbnJUV1lTRFlHRFIhOTg1Mzg1MzU2MzQ1NDQ=" -H "noby-user-id: 1" -H "noby-user-ident: 990614w" 127.0.0.1:30019 DomainServices.DocumentOnSAService.v1.DocumentOnSAService/SignDocumentManually
```
### UpdateDocumentOnSa
```
grpcurl -insecure -d "{\"DocumentOnSAId\":1, \"IsArchived\":true}" -H "Authorization: Basic WFhfTk9CWV9STVRfVVNSX1RFU1Q6cHBtbGVzbnJUV1lTRFlHRFIhOTg1Mzg1MzU2MzQ1NDQ=" -H "noby-user-id: 1" -H "noby-user-ident: 990614w" 127.0.0.1:30019 DomainServices.DocumentOnSAService.v1.DocumentOnSAService/UpdateDocumentOnSA
```
### GetDocumentsOnSAList
```
grpcurl -insecure -d "{\"SalesArrangementId\":8}" -H "Authorization: Basic WFhfTk9CWV9STVRfVVNSX1RFU1Q6cHBtbGVzbnJUV1lTRFlHRFIhOTg1Mzg1MzU2MzQ1NDQ=" 127.0.0.1:30019 DomainServices.DocumentOnSAService.v1.DocumentOnSAService/GetDocumentsOnSAList
```
### CreateDocumentOnSA
```
grpcurl -insecure -d "{\"SalesArrangementId\":20008,\"DocumentTypeId\":5,\"FormId\":\"N00000000000699\",\"EArchivId\":\"KBHXXD00000000000000000000021\",\"IsFinal\":true}" -H "Authorization: Basic WFhfTk9CWV9STVRfVVNSX1RFU1Q6cHBtbGVzbnJUV1lTRFlHRFIhOTg1Mzg1MzU2MzQ1NDQ=" -H "noby-user-id: 1" -H "noby-user-ident: 990614w" 127.0.0.1:30019 DomainServices.DocumentOnSAService.v1.DocumentOnSAService/CreateDocumentOnSA
```
### LinkEArchivIdToDocumentOnSA
```
grpcurl -insecure -d "{\"DocumentOnSAId\":1,\"EArchivId\":\"KBHXXD00000000000000000000007\"}" -H "Authorization: Basic WFhfTk9CWV9STVRfVVNSX1RFU1Q6cHBtbGVzbnJUV1lTRFlHRFIhOTg1Mzg1MzU2MzQ1NDQ=" 127.0.0.1:30019 DomainServices.DocumentOnSAService.v1.DocumentOnSAService/LinkEArchivIdToDocumentOnSA
```
### GetElectronicDocumentFromQueue
```
grpcurl -insecure -d "{\"DocumentOnSAId\":1}" -H "Authorization: Basic WFhfTk9CWV9STVRfVVNSX1RFU1Q6cHBtbGVzbnJUV1lTRFlHRFIhOTg1Mzg1MzU2MzQ1NDQ=" 127.0.0.1:30019 DomainServices.DocumentOnSAService.v1.DocumentOnSAService/GetElectronicDocumentFromQueue
```
### GetElectronicDocumentPreview
```
grpcurl -insecure -d "{\"DocumentOnSAId\":1}" -H "Authorization: Basic WFhfTk9CWV9STVRfVVNSX1RFU1Q6cHBtbGVzbnJUV1lTRFlHRFIhOTg1Mzg1MzU2MzQ1NDQ=" 127.0.0.1:30019 DomainServices.DocumentOnSAService.v1.DocumentOnSAService/GetElectronicDocumentPreview
```

### Migrations
1) Open Package manager console
2) Set project with dbcontext as startpup project (DomainServices.DocumentArchiveService.Api)
3) In package manager console set Default project to the project with dbcontext (DomainServices.DocumentArchiveService.Api)
### Add migration
EntityFrameworkCore\Add-Migration YourMigrationName -OutputDir "Database/Migrations"
### Update database to latest migration
EntityFrameworkCore\Update-Database
### or to specific migration
Update-Database YourMigrationName
## if you want create sql from migration see
https://learn.microsoft.com/en-us/ef/core/managing-schemas/migrations/applying?tabs=vs