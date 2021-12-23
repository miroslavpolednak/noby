# DomainServices.ProductService.Api

## grpcurl tests
        grpcurl -insecure 172.30.35.51:5003 list
        grpcurl -insecure -d "{\"UserId\":99,\"FirstNameNaturalPerson\":\"John\",\"Name\":\"Doe\",\"ProductInstanceType\":1}" -H "Authorization: Basic YTph" 127.0.0.1:5080 DomainServices.CaseService.v1.CaseService/CreateCase
        grpcurl -insecure -d "{\"UserId\":99}" -H "Authorization: Basic YTph" 127.0.0.1:5080 DomainServices.CaseService.v1.CaseService/SearchCases
        grpcurl -insecure -d "{\"CaseId\":49}" -H "Authorization: Basic YTph" 127.0.0.1:5080 DomainServices.CaseService.v1.CaseService/GetCaseDetail
        grpcurl -insecure -d "{\"CaseId\":49,\"UserId\":98}" -H "Authorization: Basic YTph" 127.0.0.1:5080 DomainServices.CaseService.v1.CaseService/LinkOwnerToCase
        grpcurl -insecure -d "{\"CaseId\":49,\"ContractNumber\":\"1000000001\"}" -H "Authorization: Basic YTph" 127.0.0.1:5080 DomainServices.CaseService.v1.CaseService/UpdateCaseData
        grpcurl -insecure -d "{\"CaseId\":49,\"State\":2}" -H "Authorization: Basic YTph" 127.0.0.1:5080 DomainServices.CaseService.v1.CaseService/UpdateCaseState
        grpcurl -insecure -d "{\"CaseId\":49,\"FirstNameNaturalPerson\":\"Peter\",\"Name\":\"Mortal\"}" -H "Authorization: Basic YTph" 127.0.0.1:5080 DomainServices.CaseService.v1.CaseService/UpdateCaseCustomer

## run batch
        dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\InternalServices\ServiceDiscovery\Api\CIS.InternalServices.ServiceDiscovery.Api.csproj"
        dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\DomainServices\CodebookService\Api\DomainServices.CodebookService.Api.csproj"
        dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\DomainServices\CaseService\Api\DomainServices.CaseService.Api.csproj"


