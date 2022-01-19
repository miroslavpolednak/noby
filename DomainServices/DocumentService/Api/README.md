# DomainServices.DocumentService.Api

## grpcurl tests
        grpcurl -insecure 172.30.35.51:5022 list DomainServices.DocumentService.v1.DocumentService
        grpcurl -insecure 127.0.0.1:5022 list DomainServices.DocumentService.v1.DocumentService
        
        grpcurl -insecure -d "{\"DocumentId\":\"1\",\"Mandant\":\"MP\"}" -H "Authorization: Basic YTph" 127.0.0.1:5022 DomainServices.DocumentService.v1.DocumentService/GetDocument
        grpcurl -insecure -d "{\"CaseId\":10}" -H "Authorization: Basic YTph" 127.0.0.1:5022 DomainServices.DocumentService.v1.DocumentService/GetDocumentsListByCaseId
        