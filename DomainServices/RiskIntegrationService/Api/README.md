﻿## grpcurl tests
        grpcurl -insecure 127.0.0.1:5022 list
        grpcurl -insecure -d "{\"Id\":2,\"Name\":\"John\"}" -H "Authorization: Basic YTph" 127.0.0.1:5022 DomainServices.RiskIntegrationService.v1.TestService/HalloWorld

## grpcurl tests CREDIT WORTHINESS
        grpcurl -insecure -d "{\"RiskBusinessCaseId\":\"1\",\"ResourceProcessId\":\"2\",\"UserIdentity\":{\"IdentityId\":\"111\",\"IdentityScheme\":\"KBID\"},\"Product\":{\"ProductTypeId\":20001,\"LoanDuration\":160,\"LoanInterestRate\":\"1.5\",\"LoanAmount\":1000000,\"LoanPaymentAmount\":1000,\"FixedRatePeriod\":30},\"Households\":[{\"ChildrenUpToTenYearsCount\":1,\"ChildrenOverTenYearsCount\":0,\"ExpensesSummary\":{\"Rent\":\"4000\",\"Saving\":\"2500\"},\"Customers\":[{\"InternalCustomerId\":\"111\",\"MaritalStateId\":1,\"Incomes\":[{\"IncomeTypeId\":1,\"Amount\":\"15000\"}]}]}]}" -H "Authorization: Basic YTph" -H "noby-user-id: 267" 127.0.0.1:5022 DomainServices.RiskIntegrationService.CreditWorthinessService.V2/Calculate

        grpcurl -insecure -d "{\"SalesArrangementId\":\"1989330125\",\"ResourceProcessId\":\"54103095-27D1-4CB6-8DC4-51A0C189CC81\"}" -H "Authorization: Basic YTph" 172.30.35.51:5003 DomainServices.RiskIntegrationService.RiskBusinessCaseService.V2/CreateCase

## run batch
        dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\InternalServices\ServiceDiscovery\Api\CIS.InternalServices.ServiceDiscovery.Api.csproj"
        dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\DomainServices\CodebookService\Api\DomainServices.CodebookService.Api.csproj"
        dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\DomainServices\UserService\Api\DomainServices.UserService.Api.csproj"
        dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\DomainServices\RiskIntegrationService\Api\DomainServices.RiskIntegrationService.Api.csproj"