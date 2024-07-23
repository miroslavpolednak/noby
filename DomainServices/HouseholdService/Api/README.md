# DomainServices.HouseholdService.Api

## grpcurl tests
grpcurl -insecure 172.30.35.51:5003 list

grpcurl -insecure -d "{\"SalesArrangementId\":1,\"CustomerRoleId\":2,\"Customer\":{\"CustomerIdentifiers\":[{\"identityId\":950984840,\"identityScheme\":2}],\"FirstNameNaturalPerson\":\"John\",\"Name\":\"Doe\"}}" -H "noby-user-id: 65466" -H "noby-user-ident: KBUID=A09V61" -H "Authorization: Basic YTph" 127.0.0.1:30018 DomainServices.HouseholdService.v1.CustomerOnSAService/CreateCustomer
grpcurl -insecure -d "{\"CustomerOnSAId\":3,\"Customer\":{\"CustomerIdentifiers\":[{\"identityId\":300519418,\"identityScheme\":2}],\"FirstNameNaturalPerson\":\"John\",\"Name\":\"Doe\"}}" -H "Authorization: Basic YTph" 172.30.35.51:30009 DomainServices.HouseholdService.v1.CustomerOnSAService/UpdateCustomer
grpcurl -insecure -d "{\"CustomerOnSAId\":28}" -H "Authorization: Basic YTph" -H "noby-user-id: 3048" -H "noby-user-ident: KBUID=A09FK3" 127.0.0.1:31009 DomainServices.HouseholdService.v1.CustomerOnSAService/DeleteCustomer
grpcurl -insecure -d "{\"CustomerOnSAId\":1}" -H "Authorization: Basic YTph" 127.0.0.1:30018 DomainServices.HouseholdService.v1.CustomerOnSAService/GetCustomer
grpcurl -insecure -d "{\"SalesArrangementId\":1}" -H "Authorization: Basic YTph" 127.0.0.1:30018 DomainServices.HouseholdService.v1.CustomerOnSAService/GetCustomerList
grpcurl -insecure -d "{\"CustomerOnSAId\":1,\"Obligations\":[{\"ObligationTypeId\":1,\"LoanPaymentAmount\":5000,\"CreditCardLimit\":6000}]}" -H "Authorization: Basic YTph" 127.0.0.1:30018 DomainServices.HouseholdService.v1.CustomerOnSAService/UpdateObligations
grpcurl -insecure -d "{\"IncomeId\":1}" -H "Authorization: Basic YTph" 127.0.0.1:30018 DomainServices.HouseholdService.v1.CustomerOnSAService/GetIncome
grpcurl -insecure -d "{\"CustomerOnSAId\":3,\"CustomerChangeData\":[{\"Key\":\"xxx\",\"Value\":\"honza\"}]}" -H "Authorization: Basic YTph" 127.0.0.1:30018 DomainServices.HouseholdService.v1.CustomerOnSAService/UpdateCustomerDetail

grpcurl -insecure -d "{\"SalesArrangementId\":20000}" -H "noby-user-id: 65466" -H "noby-user-ident: KBUID=A09V61" -H "Authorization: Basic YTph" 127.0.0.1:30018 DomainServices.HouseholdService.v1.CustomerOnSAService/GetCustomerChangeMetadata

grpcurl -insecure -d "{\"SalesArrangementId\":1,\"HouseholdTypeId\":1,\"CustomerOnSAId1\":1}" -H "Authorization: Basic YTph" 127.0.0.1:30018 DomainServices.HouseholdService.v1.HouseholdService/CreateHousehold
grpcurl -insecure -d "{\"SalesArrangementId\":1,\"HouseholdTypeId\":1,\"CustomerOnSAId1\":1,\"Data\":{\"ChildrenUpToTenYearsCount\":2,\"PropertySettlementId\":1},\"Expenses\":{\"SavingExpenseAmount\":20000,\"OtherExpenseAmount\":5000}}" -H "Authorization: Basic YTph" 127.0.0.1:30018 DomainServices.HouseholdService.v1.HouseholdService/CreateHousehold
grpcurl -insecure -d "{\"HouseholdId\":1}" -H "Authorization: Basic YTph" 127.0.0.1:30018 DomainServices.HouseholdService.v1.HouseholdService/GetHousehold
grpcurl -insecure -d "{\"HouseholdId\":1}" -H "Authorization: Basic YTph" 127.0.0.1:30018 DomainServices.HouseholdService.v1.HouseholdService/DeleteHousehold
grpcurl -insecure -d "{\"HouseholdId\":122,\"CustomerOnSAId1\":222,\"CustomerOnSAId2\":333}" -H "Authorization: Basic YTph" 127.0.0.1:30018 DomainServices.HouseholdService.v1.HouseholdService/UpdateHousehold
grpcurl -insecure -d "{\"HouseholdId\":1,\"CustomerOnSAId1\":1}" -H "Authorization: Basic YTph" 127.0.0.1:30018 DomainServices.HouseholdService.v1.HouseholdService/LinkCustomerOnSAToHousehold

grpcurl -insecure -d "{\"CustomerOnSAId\":1}" -H "noby-user-id: 3048" -H "noby-user-ident: KBUID=A09FK3" -H "Authorization: Basic WFhfTk9CWV9STVRfVVNSX1RFU1Q6cHBtbGVzbnJUV1lTRFlHRFIhOTg1Mzg1MzU2MzQ1NDQ=" 172.30.35.51:30018 DomainServices.HouseholdService.v1.HouseholdService/GetHouseholdIdByCustomerOnSAId

grpcurl -insecure -d "{\"CustomerOnSAId\":1,\"IncomeTypeId\":1}" -H "Authorization: Basic YTph" 127.0.0.1:30018 DomainServices.HouseholdService.v1.CustomerOnSAService/CreateIncome
grpcurl -insecure -d "{\"CustomerOnSAId\":1,\"IncomeTypeId\":1,\"BaseData\":{\"Sum\":{\"units\":200000,\"nanos\":0},\"CurrencyCode\":\"CZK\"}}" -H "Authorization: Basic YTph" -H "noby-user-id: 65466" -H "noby-user-ident: KBUID=A09V61" 127.0.0.1:30018 DomainServices.HouseholdService.v1.CustomerOnSAService/CreateIncome
grpcurl -insecure -d "{\"IncomeId\":1}" -H "Authorization: Basic YTph" 127.0.0.1:30018 DomainServices.HouseholdService.v1.CustomerOnSAService/DeleteIncome
grpcurl -insecure -d "{\"IncomeId\":2,\"BaseData\":{\"Sum\":2000},\"Employement\":{\"IsForeignIncome\":true,\"ForeignIncomeTypeId\":2}}" -H "Authorization: Basic YTph" 127.0.0.1:30018 DomainServices.HouseholdService.v1.CustomerOnSAService/UpdateIncome
grpcurl -insecure -d "{\"CustomerOnSAId\":3}" -H "Authorization: Basic YTph" 127.0.0.1:30018 DomainServices.HouseholdService.v1.CustomerOnSAService/GetIncomeList
grpcurl -insecure -d "{\"CustomerOnSAId\":3,\"ObligationTypeId\":1,\"InstallmentAmount\":{\"units\":200000,\"nanos\":0},\"Creditor\":{\"Name\":\"Franta\"},\"Correction\":{\"CorrectionTypeId\":1,\"InstallmentAmountCorrection\":{\"units\":200000,\"nanos\":0}}}" -H "Authorization: Basic YTph" -H "noby-user-id: 3048" -H "noby-user-ident: KBUID=A09FK3" 127.0.0.1:30018 DomainServices.HouseholdService.v1.CustomerOnSAService/CreateObligation
grpcurl -insecure -d "{\"ObligationId\":1}" -H "Authorization: Basic YTph" -H "noby-user-id: 3048" -H "noby-user-ident: KBUID=A09FK3" 127.0.0.1:30018 DomainServices.HouseholdService.v1.CustomerOnSAService/GetObligation
grpcurl -insecure -d "{\"ObligationId\":1}" -H "Authorization: Basic YTph" -H "noby-user-id: 3048" -H "noby-user-ident: KBUID=A09FK3" 127.0.0.1:30018 DomainServices.HouseholdService.v1.CustomerOnSAService/DeleteObligation
grpcurl -insecure -d "{\"ObligationId\":2,\"ObligationTypeId\":2}" -H "Authorization: Basic YTph" 127.0.0.1:30018 DomainServices.HouseholdService.v1.CustomerOnSAService/UpdateObligation
grpcurl -insecure -d "{\"CustomerOnSAId\":1}" -H "Authorization: Basic YTph" -H "noby-user-id: 3048" -H "noby-user-ident: KBUID=A09FK3" 127.0.0.1:30018 DomainServices.HouseholdService.v1.CustomerOnSAService/GetObligationList

grpcurl -insecure -d "{\"IncomeId\":1}" -H "Authorization: Basic YTph" -H "noby-user-id: 3048" -H "noby-user-ident: KBUID=A09FK3" 127.0.0.1:30018 DomainServices.HouseholdService.v1.CustomerOnSAService/GetIncome
grpcurl -insecure -d "{\"CustomerOnSAId\":3}" -H "Authorization: Basic YTph" -H "noby-user-id: 3048" -H "noby-user-ident: KBUID=A09FK3" 127.0.0.1:30018 DomainServices.HouseholdService.v1.CustomerOnSAService/GetIncomeList
grpcurl -insecure -d "{\"IncomeId\":1}" -H "Authorization: Basic YTph" -H "noby-user-id: 3048" -H "noby-user-ident: KBUID=A09FK3" 127.0.0.1:30018 DomainServices.HouseholdService.v1.CustomerOnSAService/DeleteIncome
grpcurl -insecure -d "{\"CustomerOnSAId\":1,\"IncomeTypeId\":1,\"BaseData\":{\"Sum\":{\"units\":200000,\"nanos\":0},\"CurrencyCode\":\"CZK\"},\"Employement\":{\"HasProofOfIncome\":true,\"ForeignIncomeTypeId\":2,\"HasWageDeduction\":true,\"Employer\":{\"Name\":\"aaaa\"}}}" -H "noby-user-id: 3048" -H "noby-user-ident: KBUID=A09FK3" -H "Authorization: Basic YTph" 127.0.0.1:30018 DomainServices.HouseholdService.v1.CustomerOnSAService/CreateIncome

## run batch
dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\InternalServices\ServiceDiscovery\Api\CIS.InternalServices.ServiceDiscovery.Api.csproj"
dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\DomainServices\CodebookService\Api\DomainServices.CodebookService.Api.csproj"
dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\DomainServices\CaseService\Api\DomainServices.CaseService.Api.csproj"
dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\DomainServices\OfferService\Api\DomainServices.OfferService.Api.csproj"
dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\DomainServices\HouseholdService\Api\DomainServices.HouseholdService.Api.csproj"


