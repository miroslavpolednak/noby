## grpcurl tests
        grpcurl -insecure 127.0.0.1:5022 list
        grpcurl -insecure -d "{\"Id\":2,\"Name\":\"John\"}" -H "Authorization: Basic YTph" 127.0.0.1:5022 DomainServices.RiskIntegrationService.v1.TestService/HalloWorld

## grpcurl tests CREDIT WORTHINESS
        grpcurl -insecure -d "{\"RiskBusinessCaseIdMp\":\"1\",\"ResourceProcessIdMp\":\"2\",\"HumanUser\":{\"Identity\":\"111\",\"IdentityScheme\":\"KBID\"},\"LoanApplicationProduct\":{\"Product\":20001,\"Maturity\":1,\"InterestRate\":\"1.5\",\"AmountRequired\":1,\"Annuity\":1,\"FixationPeriod\":1},\"Households\":[{\"ChildrenUnderAnd10\":1,\"ChildrenOver10\":0,\"ExpensesSummary\":{\"Rent\":\"4000\",\"Saving\":\"2500\"},\"Clients\":[{\"IdMp\":\"111\",\"MaritalStatusMp\":1,\"LoanApplicationIncome\":[{\"CategoryMp\":1,\"Amount\":\"15000\"}]}]}]}" -H "Authorization: Basic YTph" -H "mp-user-id: 267" 127.0.0.1:5022 DomainServices.RiskIntegrationService.v1.CreditWorthinessService/Calculate

## run batch
        dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\InternalServices\ServiceDiscovery\Api\CIS.InternalServices.ServiceDiscovery.Api.csproj"
        dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\DomainServices\CodebookService\Api\DomainServices.CodebookService.Api.csproj"
        dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\DomainServices\UserService\Api\DomainServices.UserService.Api.csproj"
        dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\DomainServices\RiskIntegrationService\Api\DomainServices.RiskIntegrationService.Api.csproj"