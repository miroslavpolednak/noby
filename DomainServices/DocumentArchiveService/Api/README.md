## grpcurl tests
        grpcurl -insecure 127.0.0.1:5015 list
        grpcurl -insecure -d "{\"EnvironmentName\":1}" -H "Authorization: Basic YTph" 127.0.0.1:5015 DomainServices.DocumentArchiveService.Contracts.V1/GenerateDocumentId

## run batch
        dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\DomainServices\DocumentArchiveService\Api\DomainServices.DocumentArchiveService.Api.csproj"