﻿## grpcurl tests
grpcurl -insecure 127.0.0.1:30003 list
grpcurl -insecure 127.0.0.1:30003 grpc.health.v1.Health/Check
grpcurl -insecure -H "noby-user-id: 3048" -H "noby-user-ident: KBUID=A09FK3" -H "Authorization: Basic YTph" 127.0.0.1:30003 DomainServices.CodebookService.v1.CodebookService/LoanKinds
grpcurl -insecure -H "Authorization: Basic YTph" 127.0.0.1:30003 DomainServices.CodebookService.v1.CodebookService/FixedRatePeriods
grpcurl -insecure -H "Authorization: Basic YTph" 127.0.0.1:30003 DomainServices.CodebookService.v1.CodebookService/SignatureTypeDetails
grpcurl -insecure -d "{\"DeveloperId\":3014640}" -H "Authorization: Basic YTph" 127.0.0.1:30003 DomainServices.CodebookService.v1.CodebookService/GetDeveloper
grpcurl -insecure -d "{\"PerformerLogin\":\"sss\"}" -H "Authorization: Basic YTph" 127.0.0.1:30003 DomainServices.CodebookService.v1.CodebookService/GetOperator
grpcurl -insecure -d "{\"PerformerLogin\":\"sss\"}" -H "Authorization: Basic WFhfTk9CWV9STVRfVVNSX1RFU1Q6cHBtbGVzbnJUV1lTRFlHRFIhOTg1Mzg1MzU2MzQ1NDQ=" 172.30.35.51:30003 DomainServices.CodebookService.v1.CodebookService/GetOperator
grpcurl -insecure -H "Authorization: Basic WFhfTk9CWV9STVRfVVNSX1RFU1Q6cHBtbGVzbnJUV1lTRFlHRFIhOTg1Mzg1MzU2MzQ1NDQ=" 172.30.35.51:30003 DomainServices.CodebookService.v1.CodebookService/FixedRatePeriods
grpcurl -insecure -H "Authorization: Basic WFhfTk9CWV9STVRfVVNSX1RFU1Q6cHBtbGVzbnJUV1lTRFlHRFIhOTg1Mzg1MzU2MzQ1NDQ=" 172.30.35.51:30003 DomainServices.CodebookService.v1.CodebookService/RealEstateValuationBuildingMaterialStructures
grpcurl -insecure -H "Authorization: Basic WFhfTk9CWV9STVRfVVNSX1RFU1Q6cHBtbGVzbnJUV1lTRFlHRFIhOTg1Mzg1MzU2MzQ1NDQ=" 172.30.35.51:30003 DomainServices.CodebookService.v1.CodebookService/ProfessionCategories
grpcurl -insecure -d "{\"RealEstateStateId\":1,\"RealEstateSubtypeId\":1,\"RealEstateTypeId\":1}" -H "Authorization: Basic WFhfTk9CWV9STVRfVVNSX1RFU1Q6cHBtbGVzbnJUV1lTRFlHRFIhOTg1Mzg1MzU2MzQ1NDQ=" 172.30.35.51:30003 DomainServices.CodebookService.v1.CodebookService/GetACVAndBagmanRealEstateType
grpcurl -insecure -d "{\"DateFrom\":{\"year\":2023,\"month\":12,\"day\":18},\"DateTo\":{\"year\":2024,\"month\":2,\"day\":18}}" -H "Authorization: Basic YTph" 127.0.0.1:30003 DomainServices.CodebookService.v1.CodebookService/GetBankingDays
grpcurl -insecure -d "{\"DateFrom\":{\"year\":2022,\"month\":12,\"day\":18},\"DateTo\":{\"year\":2024,\"month\":2,\"day\":18}}" -H "Authorization: Basic WFhfTk9CWV9STVRfVVNSX1RFU1Q6cHBtbGVzbnJUV1lTRFlHRFIhOTg1Mzg1MzU2MzQ1NDQ=" 172.30.35.51:30003 DomainServices.CodebookService.v1.CodebookService/GetBankingDays

grpcurl -insecure -H "Authorization: Basic YTph" 127.0.0.1:30003 DomainServices.CodebookService.MaintananceService/DownloadRdmCodebooks

## run batch
dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\DomainServices\CodebookService\Api\DomainServices.CodebookService.Api.csproj"