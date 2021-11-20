# DomainServices.ProductService.Api

## grpcurl tests
        grpcurl -insecure 172.30.35.51:5003 list
        grpcurl -insecure -d "{\"PartyId\":1,\"FirstNameNaturalPerson\":\"John\",\"Name\":\"Doe\",\"ProductInstanceType\":1}" -H "Authorization: Basic YTph" 127.0.0.1:5080 DomainServices.CaseService.CaseService/CreateCase
        grpcurl -insecure -d "{\"PartyId\":1}" -H "Authorization: Basic YTph" 127.0.0.1:5080 DomainServices.CaseService.CaseService/GetCaseList
        grpcurl -insecure -d "{\"CaseId\":1}" -H "Authorization: Basic YTph" 127.0.0.1:5080 DomainServices.CaseService.CaseService/GetCaseDetail
        grpcurl -insecure -d "{\"CaseId\":1,\"PartyId\":2}" -H "Authorization: Basic YTph" 127.0.0.1:5080 DomainServices.CaseService.CaseService/LinkOwnerToCase
        grpcurl -insecure -d "{\"CaseId\":1,\"ContractNumber\":\"10000001\"}" -H "Authorization: Basic YTph" 127.0.0.1:5080 DomainServices.CaseService.CaseService/UpdateCaseData
        grpcurl -insecure -d "{\"CaseId\":1,\"State\":2}" -H "Authorization: Basic YTph" 127.0.0.1:5080 DomainServices.CaseService.CaseService/UpdateCaseState

## run batch
        dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\CIS\InternalServices\ServiceDiscovery\Api\CIS.InternalServices.ServiceDiscovery.Api.csproj"
        dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\DomainServices\CodebookService\Api\DomainServices.CodebookService.Api.csproj"
        dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\DomainServices\CaseService\Api\DomainServices.CaseService.Api.csproj"


