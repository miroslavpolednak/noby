# DomainServices.ProductService.Api

## grpcurl tests
        grpcurl -insecure 172.30.35.51:5003 list
        grpcurl -insecure -d "{\"CaseId\":1,\"ProductInstanceTypeId\":1}" -H "Authorization: Basic YTph" 127.0.0.1:5070 DomainServices.ProductService.ProductService/CreateProductInstance
        grpcurl -insecure -d "{\"ProductInstanceId\":1}" -H "Authorization: Basic YTph" 127.0.0.1:5070 DomainServices.ProductService.ProductService/GetHousingSavingsInstanceBasicDetail
        grpcurl -insecure -d "{\"ProductInstanceId\":1}" -H "Authorization: Basic YTph" 127.0.0.1:5070 DomainServices.ProductService.ProductService/GetHousingSavingsInstance
        grpcurl -insecure -d "{\"CaseId\":1}" -H "Authorization: Basic YTph" 127.0.0.1:5070 DomainServices.ProductService.ProductService/GetProductInstanceList

        grpcurl -insecure -d "{\"CaseId\":1}" -H "Authorization: Basic YTph" 127.0.0.1:5011 DomainServices.ProductService.v1.ProductService/GetProductList
        grpcurl -insecure -d "{\"ProductId\":1}" -H "Authorization: Basic YTph" 127.0.0.1:5011 DomainServices.ProductService.v1.ProductService/GetMortgage
        grpcurl -insecure -d "{\"CaseId\":1, \"Mortgage\":{\"ProductTypeId\":5}}" -H "Authorization: Basic YTph" 127.0.0.1:5011 DomainServices.ProductService.v1.ProductService/CreateMortgage
        grpcurl -insecure -d "{\"ProductId\":1, \"Mortgage\":{\"ProductTypeId\":5}}" -H "Authorization: Basic YTph" 127.0.0.1:5011 DomainServices.ProductService.v1.ProductService/UpdateMorgage
        grpcurl -insecure -d "{\"ProductId\":1, \"Relationship\":{\"PartnerId\":1, \"ContractRelationshipTypeId\":2}}" -H "Authorization: Basic YTph" 127.0.0.1:5011 DomainServices.ProductService.v1.ProductService/CreateContractRelationship
        grpcurl -insecure -d "{\"ProductId\":1, \"ProductTypeId\":1}" -H "Authorization: Basic YTph" 127.0.0.1:5011 DomainServices.ProductService.v1.ProductService/DeleteContractRelationship
        

         grpcurl -insecure -d "{\"ProductId\":272}" -H "Authorization: Basic YTph" 127.0.0.1:5011 DomainServices.ProductService.v1.ProductService/GetMortgage

         grpcurl -insecure -d "{\"CaseId\":300, \"Mortgage\":{\"PartnerId\":1, \"ContractNumber\": \"KB300\", \"LoanAmount\":{\"units\":5000000}, \"LoanInterestRate\":{\"units\":3}, \"FixedRatePeriod\":5, \"ProductTypeId\":20001 }}" -H "Authorization: Basic YTph" 127.0.0.1:5011 DomainServices.ProductService.v1.ProductService/CreateMortgage


                 "partnerId": 1,
  "loanContractNumber": "KB269",
  "monthlyInstallment": 10000,
  "loanAmount": 5000000,
  "interestRate": 3.0,
 
  "loanType": "KBMortgage",

  
	
	int32 PartnerId = 1;
  string ContractNumber = 2;
	cis.types.GrpcDecimal LoanAmount = 3;
	cis.types.GrpcDecimal LoanInterestRate = 4;
	google.protobuf.Int32Value FixedRatePeriod = 5;
	int32 ProductTypeId = 6;
	cis.types.GrpcDecimal LoanPaymentAmount = 7;
	google.protobuf.Int32Value LoanActionCode = 8;
	cis.types.GrpcDecimal CurrentAmount = 9;
	cis.types.NullableGrpcDate DrawingMaxOn = 10;
	cis.types.NullableGrpcDate VUP = 11;
	google.protobuf.BoolValue Statement = 12;



## run batch
        dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\CIS\InternalServices\ServiceDiscovery\Api\CIS.InternalServices.ServiceDiscovery.Api.csproj"
        dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\DomainServices\CodebookService\Api\DomainServices.CodebookService.Api.csproj"
        dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\DomainServices\ProductService\Api\DomainServices.ProductService.Api.csproj"
