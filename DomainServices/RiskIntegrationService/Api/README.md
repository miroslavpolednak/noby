## grpcurl tests
        grpcurl -insecure 127.0.0.1:5022 list
        grpcurl -insecure -d "{\"Id\":2,\"Name\":\"John\"}" -H "Authorization: Basic YTph" 127.0.0.1:5022 DomainServices.RiskIntegrationService.v1.TestService/HalloWorld
        grpcurl -insecure -d "{\"ResourceProcessIdMp\":\"xxxx\",\"ItChannel\":\"MP\",\"RiskBusinessCaseIdMp\":\"1\",\"HumanUser\":{\"Identity\":\"990111a\",\"IdentityScheme\":\"MPAD\"},\"LoanApplicationProduct\":{\"Product\":1},\"Households\":[]}" -H "Authorization: Basic YTph" 127.0.0.1:5030 DomainServices.RiskIntegrationService.v1.RipService/CreditWorthiness

## run batch
        dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\DomainServices\RiskIntegrationService\Api\DomainServices.RiskIntegrationService.Api.csproj"