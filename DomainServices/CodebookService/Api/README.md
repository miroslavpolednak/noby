## grpcurl tests
grpcurl -insecure 127.0.0.1:30003 list
grpcurl -insecure 127.0.0.1:30003 grpc.health.v1.Health/Check
grpcurl -insecure -H "Authorization: Basic YTph" 127.0.0.1:30003 DomainServices.CodebookService.v1.CodebookService/AcademicDegreesAfter
grpcurl -insecure -d "{\"DeveloperId\":3014640}" -H "Authorization: Basic YTph" 127.0.0.1:30003 DomainServices.CodebookService.v1.CodebookService/GetDeveloper

## run batch
dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\DomainServices\CodebookService\Api\DomainServices.CodebookService.Api.csproj"