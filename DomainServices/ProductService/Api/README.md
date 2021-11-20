# DomainServices.ProductService.Api

## grpcurl tests
        grpcurl -insecure 172.30.35.51:5003 list
        grpcurl -insecure -d "{\"CaseId\":1,\"ProductInstanceType\":1}" -H "Authorization: Basic YTph" 127.0.0.1:5070 DomainServices.ProductService.ProductService/CreateProductInstance
        grpcurl -insecure -d "{\"ProductInstanceId\":1}" -H "Authorization: Basic YTph" 127.0.0.1:5070 DomainServices.ProductService.ProductService/GetHousingSavingsInstanceBasicDetail
        grpcurl -insecure -d "{\"ProductInstanceId\":1}" -H "Authorization: Basic YTph" 127.0.0.1:5070 DomainServices.ProductService.ProductService/GetHousingSavingsInstance
        grpcurl -insecure -d "{\"CaseId\":1}" -H "Authorization: Basic YTph" 127.0.0.1:5070 DomainServices.ProductService.ProductService/GetProductInstanceList

## run batch
        dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\CIS\InternalServices\ServiceDiscovery\Api\CIS.InternalServices.ServiceDiscovery.Api.csproj"
        dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\DomainServices\CodebookService\Api\DomainServices.CodebookService.Api.csproj"
        dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\DomainServices\ProductService\Api\DomainServices.ProductService.Api.csproj"


