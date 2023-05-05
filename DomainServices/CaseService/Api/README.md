# DomainServices.ProductService.Api

## grpcurl tests
grpcurl -insecure 172.30.35.51:5009 list
grpcurl -insecure 127.0.0.1:30001 grpc.health.v1.Health/Check
grpcurl -insecure -d "{\"CaseOwnerUserId\":3048,\"OfferContacts\":{\"EmailForOffer\":\"aaa@aaaa.cz\"},\"Customer\":{\"DateOfBirthNaturalPerson\":{\"year\":2022,\"month\":12,\"day\":18},\"FirstNameNaturalPerson\":\"John\",\"Name\":\"Doe\"},\"Data\":{\"TargetAmount\":{\"units\":3666666},\"ProductTypeId\":20001}}" -H "noby-user-id: 3048" -H "noby-user-ident: KBUID=A09FK3" -H "Authorization: Basic YTph" 127.0.0.1:30001 DomainServices.CaseService.v1.CaseService/CreateCase
grpcurl -insecure -d "{\"CaseOwnerUserId\":267}" -H "Authorization: Basic YTph" 127.0.0.1:5080 DomainServices.CaseService.v1.CaseService/SearchCases
grpcurl -insecure -d "{\"CaseOwnerUserId\":267,\"Pagination\":{\"recordOffset\":1,\"pageSize\":2}}" -H "Authorization: Basic YTph" 127.0.0.1:5080 DomainServices.CaseService.v1.CaseService/SearchCases
grpcurl -insecure -d "{\"CaseId\":3014569}" -H "noby-user-id: 1" -H "noby-user-ident: 990614w" -H "Authorization: Basic YTph" 127.0.0.1:30001 DomainServices.CaseService.v1.CaseService/GetCaseDetail
grpcurl -insecure -d "{\"CaseOwnerUserId\":1}" -H "Authorization: Basic YTph" 127.0.0.1:5080 DomainServices.CaseService.v1.CaseService/GetCaseCounts
grpcurl -insecure -d "{\"CaseId\":49,\"CaseOwnerUserId\":9557}" -H "Authorization: Basic YTph" 127.0.0.1:5080 DomainServices.CaseService.v1.CaseService/LinkOwnerToCase
grpcurl -insecure -d "{\"CaseId\":59,\"Data\":{\"ProductTypeId\":1,\"ContractNumber\":\"HF1000000001\",\"TargetAmount\":{\"units\":200000,\"nanos\":0}}}" -H "Authorization: Basic YTph" 127.0.0.1:5080 DomainServices.CaseService.v1.CaseService/UpdateCaseData
grpcurl -insecure -d "{\"CaseId\":49,\"State\":2}" -H "Authorization: Basic YTph" 127.0.0.1:5080 DomainServices.CaseService.v1.CaseService/UpdateCaseState
grpcurl -insecure -d "{\"CaseId\":49,\"Customer\":{\"FirstNameNaturalPerson\":\"Peter\",\"Name\":\"Mortal\"}}" -H "Authorization: Basic YTph" 127.0.0.1:5080 DomainServices.CaseService.v1.CaseService/UpdateCaseCustomer
grpcurl -insecure -d "{\"CaseId\":1}" -H "Authorization: Basic YTph" 127.0.0.1:5080 DomainServices.CaseService.v1.CaseService/GetTaskList
grpcurl -insecure -d "{\"CaseId\":1}" -H "Authorization: Basic YTph" 127.0.0.1:30001 DomainServices.CaseService.v1.CaseService/GetTaskListByContract
grpcurl -insecure -d "{\"CaseId\":1,\"State\":2}" -H "Authorization: Basic YTph" -H "Correlation-Context: MpPartyId=3048" -H "traceparent: 00-ddc1760e36a462c9c03b2583b1c9a098-ea157dc423037e71-01" 127.0.0.1:5080 DomainServices.CaseService.v1.CaseService/UpdateCaseState
grpcurl -insecure -d "{\"CaseId\":3014591}" -H "noby-user-id: 3048" -H "noby-user-ident: KBUID=A09FK3" -H "Authorization: Basic YTph" 127.0.0.1:30001 DomainServices.CaseService.v1.CaseService/DeleteCase
grpcurl -insecure -d "{\"CaseId\":3014640}" -H "noby-user-id: 3048" -H "noby-user-ident: KBUID=A09FK3" -H "Authorization: Basic WFhfTk9CWV9STVRfVVNSX1RFU1Q6cHBtbGVzbnJUV1lTRFlHRFIhOTg1Mzg1MzU2MzQ1NDQ=" 172.30.35.51:30001 DomainServices.CaseService.v1.CaseService/NotifyStarbuild
grpcurl -insecure -d "{\"CaseId\":3014640}" -H "noby-user-id: 3048" -H "noby-user-ident: KBUID=A09FK3" -H "Authorization: Basic YTph" 127.0.0.1:30001 DomainServices.CaseService.v1.CaseService/NotifyStarbuild

grpcurl -insecure -d "{\"TaskTypeId\":3,\"TaskSubtypeId\":1,\"ProcessId\":6610481,\"TaskRequest\":\"nejaky text\",\"CaseId\":3014654}" -H "Authorization: Basic YTph" 127.0.0.1:30001 DomainServices.CaseService.v1.CaseService/CreateTask
grpcurl -insecure -d "{\"TaskIdSB\":3}" -H "Authorization: Basic YTph" 127.0.0.1:30001 DomainServices.CaseService.v1.CaseService/CancelTask

## run batch
dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\InternalServices\ServiceDiscovery\Api\CIS.InternalServices.ServiceDiscovery.Api.csproj"
dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\DomainServices\CodebookService\Api\DomainServices.CodebookService.Api.csproj"
dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\DomainServices\UserService\Api\DomainServices.UserService.Api.csproj"
dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\DomainServices\CaseService\Api\DomainServices.CaseService.Api.csproj"
