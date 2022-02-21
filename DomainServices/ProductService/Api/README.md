# DomainServices.ProductService.Api

## grpcurl tests
        grpcurl -insecure 172.30.35.51:5011 list
        grpcurl -insecure -d "{\"CaseId\":305}" -H "Authorization: Basic YTph" 127.0.0.1:5011 DomainServices.ProductService.v1.ProductService/GetProductList
        grpcurl -insecure -d "{\"ProductId\":301}" -H "Authorization: Basic YTph" 127.0.0.1:5011 DomainServices.ProductService.v1.ProductService/GetMortgage
        grpcurl -insecure -d "{\"CaseId\":301, \"Mortgage\":{\"PartnerId\":1, \"ContractNumber\": \"KB301\", \"LoanAmount\":{\"units\":6500000}, \"LoanInterestRate\":{\"units\":2}, \"FixedRatePeriod\":4, \"ProductTypeId\":20001 }}" -H "Authorization: Basic YTph" 127.0.0.1:5011 DomainServices.ProductService.v1.ProductService/CreateMortgage
        grpcurl -insecure -d "{\"ProductId\":301, \"Mortgage\":{\"PartnerId\":1, \"ContractNumber\": \"KB301_A\", \"LoanAmount\":{\"units\":6250000}, \"LoanInterestRate\":{\"units\":2}, \"FixedRatePeriod\":4, \"ProductTypeId\":20001 }}" -H "Authorization: Basic YTph" 127.0.0.1:5011 DomainServices.ProductService.v1.ProductService/UpdateMorgage
        grpcurl -insecure -d "{\"ProductId\":300, \"Relationship\":{\"PartnerId\":1, \"ContractRelationshipTypeId\":2}}" -H "Authorization: Basic YTph" 127.0.0.1:5011 DomainServices.ProductService.v1.ProductService/CreateContractRelationship
        grpcurl -insecure -d "{\"ProductId\":300, \"PartnerId\":1}" -H "Authorization: Basic YTph" 127.0.0.1:5011 DomainServices.ProductService.v1.ProductService/DeleteContractRelationship

## run batch
        dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\CIS\InternalServices\ServiceDiscovery\Api\CIS.InternalServices.ServiceDiscovery.Api.csproj"
        dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\DomainServices\CodebookService\Api\DomainServices.CodebookService.Api.csproj"
        dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\DomainServices\ProductService\Api\DomainServices.ProductService.Api.csproj"
