# DomainServices.ProductService.Api

## grpcurl tests
grpcurl -insecure 172.30.35.51:30007 list
grpcurl -insecure -d "{\"CaseId\":305}" -H "Authorization: Basic YTph" 127.0.0.1:5011 DomainServices.ProductService.v1.ProductService/GetProductList
grpcurl -insecure -d "{\"ProductId\":2954604}" -H "Authorization: Basic YTph" -H "noby-user-id: 3048" -H "noby-user-ident: KBUID=A09FK3" 127.0.0.1:30007 DomainServices.ProductService.v1.ProductService/GetMortgage
grpcurl -insecure -d "{\"CaseId\":301, \"Mortgage\":{\"PartnerId\":1, \"ContractNumber\": \"KB301\", \"LoanAmount\":{\"units\":6500000}, \"LoanInterestRate\":{\"units\":2}, \"FixedRatePeriod\":4, \"ProductTypeId\":20001 }}" -H "Authorization: Basic YTph" 127.0.0.1:30007 DomainServices.ProductService.v1.ProductService/CreateMortgage
grpcurl -insecure -d "{\"ProductId\":301, \"Mortgage\":{\"PartnerId\":1, \"ContractNumber\": \"KB301_A\", \"LoanAmount\":{\"units\":6250000}, \"LoanInterestRate\":{\"units\":2}, \"FixedRatePeriod\":4, \"ProductTypeId\":20001 }}" -H "Authorization: Basic YTph" 127.0.0.1:5011 DomainServices.ProductService.v1.ProductService/UpdateMorgage
grpcurl -insecure -d "{\"ProductId\":300, \"Relationship\":{\"PartnerId\":1, \"ContractRelationshipTypeId\":2}}" -H "Authorization: Basic YTph" 127.0.0.1:5011 DomainServices.ProductService.v1.ProductService/CreateContractRelationship
grpcurl -insecure -d "{\"ProductId\":300, \"PartnerId\":1}" -H "Authorization: Basic YTph" 127.0.0.1:5011 DomainServices.ProductService.v1.ProductService/DeleteContractRelationship
grpcurl -insecure -d "{\"ProductId\":3046041}" -H "Authorization: Basic YTph" 127.0.0.1:30007 DomainServices.ProductService.v1.ProductService/GetProductObligationList

grpcurl -insecure -d "{\"ProductId\":2193680}" -H "Authorization: Basic YTph" 127.0.0.1:30007 DomainServices.ProductService.v1.ProductService/GetCustomersOnProduct
grpcurl -insecure -d "{\"ContractNumber\":{\"ContractNumber\":\"HF00000001353\"}}" -H "Authorization: Basic YTph" 127.0.0.1:30007 DomainServices.ProductService.v1.ProductService/GetCaseId
grpcurl -insecure -d "{\"PaymentAccount\":{\"Prefix\":\"35\",\"AccountNumber\":\"2271460227\"}}" -H "Authorization: Basic YTph" 127.0.0.1:30007 DomainServices.ProductService.v1.ProductService/GetCaseId

grpcurl -insecure -d "{\"CaseId\":3045664}" -H "Authorization: Basic WFhfTk9CWV9STVRfVVNSX1RFU1Q6cHBtbGVzbnJUV1lTRFlHRFIhOTg1Mzg1MzU2MzQ1NDQ=" 172.30.35.51:30007 DomainServices.ProductService.v1.ProductService/GetCovenantList

## run batch
dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\CIS\InternalServices\ServiceDiscovery\Api\CIS.InternalServices.ServiceDiscovery.Api.csproj"
dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\DomainServices\CodebookService\Api\DomainServices.CodebookService.Api.csproj"
dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\DomainServices\ProductService\Api\DomainServices.ProductService.Api.csproj"
