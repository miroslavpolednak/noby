# DomainServices.ProductService.Api

## grpcurl tests
grpcurl -insecure 172.30.35.51:5009 list
grpcurl -insecure 127.0.0.1:30001 grpc.health.v1.Health/Check
grpcurl -insecure -d "{\"CaseOwnerUserId\":267}" -H "Authorization: Basic YTph" 127.0.0.1:5080 DomainServices.CaseService.v1.CaseService/SearchCases

## run batch
dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\DomainServices\RealEstateValuationService\Api\DomainServices.RealEstateValuationService.Api.csproj"
