## grpcurl tests
        grpcurl -insecure 127.0.0.1:5022 list
        grpcurl -insecure -d "{\"Id\":2,\"Name\":\"John\"}" -H "Authorization: Basic YTph" 127.0.0.1:5022 DomainServices.RiskIntegrationService.v1.TestService/HalloWorld

        grpcurl -insecure -d "{\"ItChannel\":\"NOBY\",\"RiskBusinessCaseIdMp\":\"xxxxx1\",\"ResourceProcessIdMp\":\"2\",\"HumanUser\":{\"Identity\":\"John\",\"IdentityScheme\":\"KBID\"},\"LoanApplicationProduct\":{\"Product\":1,\"Maturity\":1,\"InterestRate\":1,\"AmountRequired\":1,\"Annuity\":1,\"FixationPeriod\":1}}" -H "Authorization: Basic YTph" 127.0.0.1:5022 DomainServices.RiskIntegrationService.v1.CreditWorthinessService/Calculate

        grpcurl -insecure -d "{\"LoanApplicationIdMp\":{\"Id\":\"xx\",\"Name\":\"xxx\"},\"ResourceProcessIdMp\":\"2\",\"ItChannel\":\"John\"}" -H "Authorization: Basic YTph" 127.0.0.1:5022 DomainServices.RiskIntegrationService.v1.RiskBusinessCaseService/CreateCase

## run batch
        dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\InternalServices\ServiceDiscovery\Api\CIS.InternalServices.ServiceDiscovery.Api.csproj"
        dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\DomainServices\CodebookService\Api\DomainServices.CodebookService.Api.csproj"
        dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\DomainServices\RiskIntegrationService\Api\DomainServices.RiskIntegrationService.Api.csproj"