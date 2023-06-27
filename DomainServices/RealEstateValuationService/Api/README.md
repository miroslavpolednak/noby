# DomainServices.ProductService.Api

## grpcurl tests
grpcurl -insecure 172.30.35.51:5009 list
grpcurl -insecure 127.0.0.1:30001 grpc.health.v1.Health/Check
grpcurl -insecure -d "{\"CaseId\":267,\"RealEstateTypeId\":1,\"ValuationStateId\":1,\"IsLoanRealEstate\":true,\"ValuationTypeId\":1}" -H "Authorization: Basic YTph" -H "noby-user-id: 3048" -H "noby-user-ident: KBUID=A09FK3" 127.0.0.1:30030 DomainServices.RealEstateValuationService.v1.RealEstateValuationService/CreateRealEstateValuation
grpcurl -insecure -d "{\"RealEstateValuationId\":1}" -H "Authorization: Basic YTph" -H "noby-user-id: 3048" -H "noby-user-ident: KBUID=A09FK3" 127.0.0.1:30030 DomainServices.RealEstateValuationService.v1.RealEstateValuationService/DeleteRealEstateValuation
grpcurl -insecure -d "{\"CaseId\":267}" -H "Authorization: Basic YTph" -H "noby-user-id: 3048" -H "noby-user-ident: KBUID=A09FK3" 127.0.0.1:30030 DomainServices.RealEstateValuationService.v1.RealEstateValuationService/GetRealEstateValuationList
grpcurl -insecure -d "{\"RealEstateValuationId\":2,\"ValuationStateId\":2}" -H "Authorization: Basic YTph" -H "noby-user-id: 3048" -H "noby-user-ident: KBUID=A09FK3" 127.0.0.1:30030 DomainServices.RealEstateValuationService.v1.RealEstateValuationService/PatchDeveloperOnRealEstateValuation
grpcurl -insecure -d "{\"RealEstateValuationId\":1}" -H "Authorization: Basic YTph" -H "noby-user-id: 3048" -H "noby-user-ident: KBUID=A09FK3" 127.0.0.1:30030 DomainServices.RealEstateValuationService.v1.RealEstateValuationService/GetRealEstateValuationDetail

## run batch
dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\DomainServices\RealEstateValuationService\Api\DomainServices.RealEstateValuationService.Api.csproj"
