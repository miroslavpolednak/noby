## grpcurl tests
        grpcurl -insecure -d "{\"Id\":2}" -H "Authorization: Basic YTph" 127.0.0.1:5082 DomainServices.RiskIntegrationService.v1.RiskIntegrationService/MyTest

## run batch
        dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\DomainServices\RiskIntegrationService\Api\DomainServices.RiskIntegrationService.Api.csproj"