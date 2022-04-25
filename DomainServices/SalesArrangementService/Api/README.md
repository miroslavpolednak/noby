# DomainServices.SalesArrangementService.Api

## grpcurl tests
        grpcurl -insecure 172.30.35.51:5003 list
        grpcurl -insecure -d "{\"CaseId\":99,\"SalesArrangementTypeId\":1,\"OfferId\":8}" -H "Authorization: Basic YTph" 127.0.0.1:5090 DomainServices.SalesArrangementService.v1.SalesArrangementService/CreateSalesArrangement
        grpcurl -insecure -d "{\"SalesArrangementId\":2}" -H "Authorization: Basic YTph" 127.0.0.1:5090 DomainServices.SalesArrangementService.v1.SalesArrangementService/GetSalesArrangement
        grpcurl -insecure -d "{\"OfferId\":9}" -H "Authorization: Basic YTph" 127.0.0.1:5090 DomainServices.SalesArrangementService.v1.SalesArrangementService/GetSalesArrangementByOfferId
        grpcurl -insecure -d "{\"SalesArrangementId\":2}" -H "Authorization: Basic YTph" 127.0.0.1:5090 DomainServices.SalesArrangementService.v1.SalesArrangementService/GetSalesArrangementData
        grpcurl -insecure -d "{\"CaseId\":1}" -H "Authorization: Basic YTph" 127.0.0.1:5090 DomainServices.SalesArrangementService.v1.SalesArrangementService/GetSalesArrangementsByCaseId
        grpcurl -insecure -d "{\"SalesArrangementId\":2,\"State\":2}" -H "Authorization: Basic YTph" 127.0.0.1:5090 DomainServices.SalesArrangementService.v1.SalesArrangementService/UpdateSalesArrangementState
        grpcurl -insecure -d "{\"SalesArrangementId\":2,\"OfferId\":2}" -H "Authorization: Basic YTph" 127.0.0.1:5090 DomainServices.SalesArrangementService.v1.SalesArrangementService/LinkModelationToSalesArrangement
        grpcurl -insecure -d "{\"SalesArrangementId\":2,\"ContractNumber\":\"123456789\"}" -H "Authorization: Basic YTph" 127.0.0.1:5090 DomainServices.SalesArrangementService.v1.SalesArrangementService/UpdateSalesArrangement
        grpcurl -insecure -d "{\"SalesArrangementId\":2}" -H "Authorization: Basic YTph" 127.0.0.1:5090 DomainServices.SalesArrangementService.v1.SalesArrangementService/SendToCmp
        grpcurl -insecure -d "{\"SalesArrangementId\":2,\"Mortgage\":{\"IncomeCurrencyCode\":\"CZK\",\"ResidencyCurrencyCode\":\"CZK\",\"SignatureTypeId\":1,\"LoanRealEstates\":[{\"RealEstateTypeId\":1,\"RealEstatePurchaseTypeId\":1}]}}" -H "Authorization: Basic YTph" 127.0.0.1:5090 DomainServices.SalesArrangementService.v1.SalesArrangementService/UpdateSalesArrangementParameters

        grpcurl -insecure -d "{\"SalesArrangementId\":1,\"CustomerIdentifiers\":[{\"identityId\":1,\"identityScheme\":1}],\"CustomerRoleId\":1,\"FirstNameNaturalPerson\":\"John\",\"Name\":\"Doe\"}" -H "Authorization: Basic YTph" 127.0.0.1:5090 DomainServices.SalesArrangementService.v1.CustomerOnSAService/CreateCustomer
        grpcurl -insecure -d "{\"CustomerOnSAId\":2}" -H "Authorization: Basic YTph" 127.0.0.1:5090 DomainServices.SalesArrangementService.v1.CustomerOnSAService/DeleteCustomer
        grpcurl -insecure -d "{\"CustomerOnSAId\":1}" -H "Authorization: Basic YTph" 127.0.0.1:5090 DomainServices.SalesArrangementService.v1.CustomerOnSAService/GetCustomer
        grpcurl -insecure -d "{\"SalesArrangementId\":1}" -H "Authorization: Basic YTph" 127.0.0.1:5090 DomainServices.SalesArrangementService.v1.CustomerOnSAService/GetCustomerList
        grpcurl -insecure -d "{\"CustomerOnSAId\":1,\"Obligations\":[{\"ObligationTypeId\":1,\"LoanPaymentAmount\":5000,\"CreditCardLimit\":6000}]}" -H "Authorization: Basic YTph" 127.0.0.1:5090 DomainServices.SalesArrangementService.v1.CustomerOnSAService/UpdateObligations
        grpcurl -insecure -d "{\"IncomeId\":1}" -H "Authorization: Basic YTph" 127.0.0.1:5090 DomainServices.SalesArrangementService.v1.CustomerOnSAService/GetIncome
		
		grpcurl -insecure -d "{\"SalesArrangementId\":1,\"HouseholdTypeId\":1,\"CustomerOnSAId1\":1}" -H "Authorization: Basic YTph" 127.0.0.1:5090 DomainServices.SalesArrangementService.v1.HouseholdService/CreateHousehold
        grpcurl -insecure -d "{\"SalesArrangementId\":1,\"HouseholdTypeId\":1,\"CustomerOnSAId1\":1,\"Data\":{\"ChildrenUpToTenYearsCount\":2,\"PropertySettlementId\":1},\"Expenses\":{\"SavingExpenseAmount\":20000,\"OtherExpenseAmount\":5000}}" -H "Authorization: Basic YTph" 127.0.0.1:5090 DomainServices.SalesArrangementService.v1.HouseholdService/CreateHousehold
        grpcurl -insecure -d "{\"HouseholdId\":1}" -H "Authorization: Basic YTph" 127.0.0.1:5090 DomainServices.SalesArrangementService.v1.HouseholdService/GetHousehold
        grpcurl -insecure -d "{\"HouseholdId\":1}" -H "Authorization: Basic YTph" 127.0.0.1:5090 DomainServices.SalesArrangementService.v1.HouseholdService/DeleteHousehold
        grpcurl -insecure -d "{\"HouseholdId\":122,\"CustomerOnSAId1\":222,\"CustomerOnSAId2\":333}" -H "Authorization: Basic YTph" 127.0.0.1:5090 DomainServices.SalesArrangementService.v1.HouseholdService/UpdateHousehold
        grpcurl -insecure -d "{\"HouseholdId\":1,\"CustomerOnSAId1\":1}" -H "Authorization: Basic YTph" 127.0.0.1:5090 DomainServices.SalesArrangementService.v1.HouseholdService/LinkCustomerOnSAToHousehold

        grpcurl -insecure -d "{\"CustomerOnSAId\":1,\"IncomeTypeId\":1}" -H "Authorization: Basic YTph" 127.0.0.1:5090 DomainServices.SalesArrangementService.v1.HouseholdService/CreateIncome
        grpcurl -insecure -d "{\"CustomerOnSAId\":1,\"IncomeTypeId\":1,\"BaseData\":{\"Sum\":20000,\"CurrencyCode\":\"CZK\"}}" -H "Authorization: Basic YTph" 127.0.0.1:5090 DomainServices.SalesArrangementService.v1.HouseholdService/CreateIncome
        grpcurl -insecure -d "{\"CustomerOnSAId\":1,\"IncomeTypeId\":1,\"Employement\":{\"IsAbroadIncome\":true,\"AbroadIncomeTypeId\":2}}" -H "Authorization: Basic YTph" 127.0.0.1:5090 DomainServices.SalesArrangementService.v1.HouseholdService/CreateIncome
        grpcurl -insecure -d "{\"IncomeId\":1}" -H "Authorization: Basic YTph" 127.0.0.1:5090 DomainServices.SalesArrangementService.v1.HouseholdService/DeleteIncome
        grpcurl -insecure -d "{\"IncomeId\":2,\"BaseData\":{\"Sum\":2000},\"Employement\":{\"IsAbroadIncome\":true,\"AbroadIncomeTypeId\":2}}" -H "Authorization: Basic YTph" 127.0.0.1:5090 DomainServices.SalesArrangementService.v1.HouseholdService/UpdateIncome
        grpcurl -insecure -d "{\"CustomerOnSAId\":1}" -H "Authorization: Basic YTph" 127.0.0.1:5090 DomainServices.SalesArrangementService.v1.HouseholdService/GetIncomeList

## run batch
        dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\InternalServices\ServiceDiscovery\Api\CIS.InternalServices.ServiceDiscovery.Api.csproj"
        dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\DomainServices\CodebookService\Api\DomainServices.CodebookService.Api.csproj"
        dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\DomainServices\CaseService\Api\DomainServices.CaseService.Api.csproj"
        dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\DomainServices\OfferService\Api\DomainServices.OfferService.Api.csproj"
        dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\DomainServices\SalesArrangementService\Api\DomainServices.SalesArrangementService.Api.csproj"


