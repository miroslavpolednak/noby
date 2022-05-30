# DomainServices.ProductService.Api

## grpcurl tests
        grpcurl -insecure 172.30.35.51:5009 list
        grpcurl -insecure -d "{\"CaseOwnerUserId\":267,\"Customer\":{\"FirstNameNaturalPerson\":\"John\",\"Name\":\"Doe\"},\"Data\":{\"TargetAmount\":{\"units\":300000},\"ProductTypeId\":20001}}" -H "Authorization: Basic YTph" 127.0.0.1:5080 DomainServices.CaseService.v1.CaseService/CreateCase
        grpcurl -insecure -d "{\"CaseOwnerUserId\":267}" -H "Authorization: Basic YTph" 127.0.0.1:5080 DomainServices.CaseService.v1.CaseService/SearchCases
        grpcurl -insecure -d "{\"CaseOwnerUserId\":267,\"Pagination\":{\"recordOffset\":1,\"pageSize\":2}}" -H "Authorization: Basic YTph" 127.0.0.1:5080 DomainServices.CaseService.v1.CaseService/SearchCases
        grpcurl -insecure -d "{\"CaseId\":49}" -H "Authorization: Basic YTph" 127.0.0.1:5080 DomainServices.CaseService.v1.CaseService/GetCaseDetail
        grpcurl -insecure -d "{\"CaseOwnerUserId\":1}" -H "Authorization: Basic YTph" 127.0.0.1:5080 DomainServices.CaseService.v1.CaseService/GetCaseCounts
        grpcurl -insecure -d "{\"CaseId\":49,\"CaseOwnerUserId\":9557}" -H "Authorization: Basic YTph" 127.0.0.1:5080 DomainServices.CaseService.v1.CaseService/LinkOwnerToCase
        grpcurl -insecure -d "{\"CaseId\":59,\"Data\":{\"ProductTypeId\":1,\"ContractNumber\":\"HF1000000001\",\"TargetAmount\":{\"units\":200000,\"nanos\":0}}}" -H "Authorization: Basic YTph" 127.0.0.1:5080 DomainServices.CaseService.v1.CaseService/UpdateCaseData
        grpcurl -insecure -d "{\"CaseId\":49,\"State\":2}" -H "Authorization: Basic YTph" 127.0.0.1:5080 DomainServices.CaseService.v1.CaseService/UpdateCaseState
        grpcurl -insecure -d "{\"CaseId\":49,\"Customer\":{\"FirstNameNaturalPerson\":\"Peter\",\"Name\":\"Mortal\"}}" -H "Authorization: Basic YTph" 127.0.0.1:5080 DomainServices.CaseService.v1.CaseService/UpdateCaseCustomer
        grpcurl -insecure -d "{\"CaseId\":1}" -H "Authorization: Basic YTph" 127.0.0.1:5080 DomainServices.CaseService.v1.CaseService/GetTaskList

## run batch
        dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\InternalServices\ServiceDiscovery\Api\CIS.InternalServices.ServiceDiscovery.Api.csproj"
        dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\DomainServices\CodebookService\Api\DomainServices.CodebookService.Api.csproj"
        dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\DomainServices\UserService\Api\DomainServices.UserService.Api.csproj"
        dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\DomainServices\CaseService\Api\DomainServices.CaseService.Api.csproj"
