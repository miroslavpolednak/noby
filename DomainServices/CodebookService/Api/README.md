﻿## grpcurl tests
grpcurl -insecure 127.0.0.1:30003 list
grpcurl -insecure 127.0.0.1:30003 grpc.health.v1.Health/Check
grpcurl -insecure -H "noby-user-id: 3048" -H "noby-user-ident: KBUID=A09FK3" -H "Authorization: Basic YTph" 127.0.0.1:30003 DomainServices.CodebookService.v1.CodebookService/LoanKinds
grpcurl -insecure -H "Authorization: Basic YTph" 127.0.0.1:30003 DomainServices.CodebookService.v1.CodebookService/WorkflowProcessType
grpcurl -insecure -H "Authorization: Basic YTph" 127.0.0.1:30003 DomainServices.CodebookService.v1.CodebookService/LoanPurposes
grpcurl -insecure -d "{\"DeveloperId\":3014640}" -H "Authorization: Basic YTph" 127.0.0.1:30003 DomainServices.CodebookService.v1.CodebookService/GetDeveloper
grpcurl -insecure -d "{\"PerformerLogin\":\"sss\"}" -H "Authorization: Basic YTph" 127.0.0.1:30003 DomainServices.CodebookService.v1.CodebookService/GetOperator
grpcurl -insecure -d "{\"PerformerLogin\":\"sss\"}" -H "Authorization: Basic WFhfTk9CWV9STVRfVVNSX1RFU1Q6cHBtbGVzbnJUV1lTRFlHRFIhOTg1Mzg1MzU2MzQ1NDQ=" 172.30.35.51:30003 DomainServices.CodebookService.v1.CodebookService/GetOperator
grpcurl -insecure -H "Authorization: Basic WFhfTk9CWV9STVRfVVNSX1RFU1Q6cHBtbGVzbnJUV1lTRFlHRFIhOTg1Mzg1MzU2MzQ1NDQ=" 172.30.35.51:30003 DomainServices.CodebookService.v1.CodebookService/WorkflowTaskStates
grpcurl -insecure -H "Authorization: Basic WFhfTk9CWV9STVRfVVNSX1RFU1Q6cHBtbGVzbnJUV1lTRFlHRFIhOTg1Mzg1MzU2MzQ1NDQ=" 172.30.35.51:30003 DomainServices.CodebookService.v1.CodebookService/RealEstateValuationBuildingMaterialStructures
grpcurl -insecure -d "{\"RealEstateStateId\":1,\"RealEstateSubtypeId\":1,\"RealEstateTypeId\":1}" -H "Authorization: Basic WFhfTk9CWV9STVRfVVNSX1RFU1Q6cHBtbGVzbnJUV1lTRFlHRFIhOTg1Mzg1MzU2MzQ1NDQ=" 172.30.35.51:30003 DomainServices.CodebookService.v1.CodebookService/GetACVAndBagmanRealEstateType

## run batch
dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\DomainServices\CodebookService\Api\DomainServices.CodebookService.Api.csproj"