# DomainServices.DocumentService.Api

## grpcurl tests
        grpcurl -insecure 172.30.35.51:5022 list DomainServices.DocumentService.v1.DocumentService
        grpcurl -insecure 127.0.0.1:5022 list DomainServices.DocumentService.v1.DocumentService
        
        grpcurl -insecure -d "{\"DocumentId\":\"1\",\"Mandant\":\"Mp\"}" -H "Authorization: Basic YTph" 127.0.0.1:5022 DomainServices.DocumentService.v1.DocumentService/GetDocument
        grpcurl -insecure -d "{\"CaseId\":10}" -H "Authorization: Basic YTph" 127.0.0.1:5022 DomainServices.DocumentService.v1.DocumentService/GetDocumentsListByCaseId
        grpcurl -insecure -d "{\"ContractNumber\":\"1\",\"Mandant\":\"Mp\"}" -H "Authorization: Basic YTph" 127.0.0.1:5022 DomainServices.DocumentService.v1.DocumentService/GetDocumentsListByContractNumber
        grpcurl -insecure -d "{\"CustomerId\":\"1\"}" -H "Authorization: Basic YTph" 127.0.0.1:5022 DomainServices.DocumentService.v1.DocumentService/GetDocumentsListByCustomerId
        grpcurl -insecure -d "{\"RelationId\":\"1\",\"Mandant\":\"Mp\"}" -H "Authorization: Basic YTph" 127.0.0.1:5022 DomainServices.DocumentService.v1.DocumentService/GetDocumentsListByRelationId
        grpcurl -insecure -d "{\"DocumentId\":\"3033308\",\"Mandant\":\"Mp\"}" -H "Authorization: Basic YTph" 127.0.0.1:5022 DomainServices.DocumentService.v1.DocumentService/GetDocumentStatus

   
## DocumentId (to test GetDocumentStatus method) 
        3033308
        35722
        3033307
        3033306
        3033305
        3033304
        3033303
        3033302
        3033301
        3033300
        3033299
        3033298
        3033297
        3033296
        3033295
        3033294
        3033293
        3033292
