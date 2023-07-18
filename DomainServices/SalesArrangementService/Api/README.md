# DomainServices.SalesArrangementService.Api

## grpcurl tests
grpcurl -insecure 172.30.35.51:5003 list
grpcurl -insecure -d "{\"CaseId\":3045664,\"SalesArrangementTypeId\":6,\"OfferId\":null,\"ContractNumber\":\"\",\"Mortgage\":null,\"Drawing\":{\"Applicant\":null,\"Agent\":null,\"RepaymentAccount\":{\"IsAccountNumberMissing\":false,\"Prefix\":\"\",\"Number\":\"3510720263\",\"BankCode\":\"0100\"},\"PayoutList\":[],\"DrawingDate\":null,\"IsImmediateDrawing\":false}}" -H "Authorization: Basic YTph" -H "noby-user-id: 3048" -H "noby-user-ident: KBUID=A09FK3" 127.0.0.1:30009 DomainServices.SalesArrangementService.v1.SalesArrangementService/CreateSalesArrangement
grpcurl -insecure -d "{\"SalesArrangementId\":20070}" -H "Authorization: Basic YTph" 127.0.0.1:30009 DomainServices.SalesArrangementService.v1.SalesArrangementService/GetSalesArrangement
grpcurl -insecure -d "{\"SalesArrangementId\":1}" -H "Authorization: Basic WFhfTk9CWV9STVRfVVNSX1RFU1Q6cHBtbGVzbnJUV1lTRFlHRFIhOTg1Mzg1MzU2MzQ1NDQ=" 127.0.0.1:5090 DomainServices.SalesArrangementService.v1.SalesArrangementService/DeleteSalesArrangement
grpcurl -insecure -d "{\"OfferId\":9}" -H "Authorization: Basic YTph" 127.0.0.1:5090 DomainServices.SalesArrangementService.v1.SalesArrangementService/GetSalesArrangementByOfferId
grpcurl -insecure -d "{\"SalesArrangementId\":2}" -H "Authorization: Basic YTph" 127.0.0.1:5090 DomainServices.SalesArrangementService.v1.SalesArrangementService/GetSalesArrangementData
grpcurl -insecure -d "{\"CaseId\":1}" -H "Authorization: Basic YTph" 127.0.0.1:5090 DomainServices.SalesArrangementService.v1.SalesArrangementService/GetSalesArrangementsByCaseId
grpcurl -insecure -d "{\"SalesArrangementId\":2,\"State\":2}" -H "Authorization: Basic YTph" 127.0.0.1:5090 DomainServices.SalesArrangementService.v1.SalesArrangementService/UpdateSalesArrangementState
grpcurl -insecure -d "{\"SalesArrangementId\":20070,\"OfferId\":79}" -H "Authorization: Basic YTph" -H "noby-user-id: 3048" -H "noby-user-ident: KBUID=A09FK3" 127.0.0.1:30009 DomainServices.SalesArrangementService.v1.SalesArrangementService/LinkModelationToSalesArrangement
grpcurl -insecure -d "{\"SalesArrangementId\":2,\"ContractNumber\":\"123456789\"}" -H "Authorization: Basic YTph" 127.0.0.1:5090 DomainServices.SalesArrangementService.v1.SalesArrangementService/UpdateSalesArrangement
grpcurl -insecure -d "{\"SalesArrangementId\":938}" -H "Authorization: Basic YTph" 127.0.0.1:30009 DomainServices.SalesArrangementService.v1.SalesArrangementService/SendToCmp
grpcurl -insecure -d "{\"SalesArrangementId\":1,\"Mortgage\":{\"IncomeCurrencyCode\":\"CZK\",\"ResidencyCurrencyCode\":\"CZK\",\"LoanRealEstates\":[{\"RealEstateTypeId\":1,\"RealEstatePurchaseTypeId\":1}]}}" -H "Authorization: Basic YTph" 127.0.0.1:5090 DomainServices.SalesArrangementService.v1.SalesArrangementService/UpdateSalesArrangementParameters
grpcurl -insecure -d "{\"SalesArrangementId\":2,\"LoanApplicationAssessmentId\":222,\"RiskSegment\":\"xxxxx\"}" -H "Authorization: Basic YTph" 127.0.0.1:5090 DomainServices.SalesArrangementService.v1.SalesArrangementService/UpdateLoanAssessmentParameters

grpcurl -insecure -d "{\"SalesArrangementId\":26}" -H "Authorization: Basic YTph" 127.0.0.1:5010 DomainServices.SalesArrangementService.v1.SalesArrangementService/SendToCmp
grpcurl -insecure -d "{\"SalesArrangementId\":26}" -H "Authorization: Basic YTph" 127.0.0.1:5090 DomainServices.SalesArrangementService.v1.SalesArrangementService/ValidateSalesArrangement
grpcurl -insecure -d "{\"SalesArrangementId\":705}" -H "Authorization: Basic WFhfTk9CWV9STVRfVVNSX1RFU1Q6cHBtbGVzbnJUV1lTRFlHRFIhOTg1Mzg1MzU2MzQ1NDQ=" 172.30.35.51:30009 DomainServices.SalesArrangementService.v1.SalesArrangementService/ValidateSalesArrangement

grpcurl -insecure -d "{\"SalesArrangementId\":23990}" -H "noby-user-id: 3048" -H "noby-user-ident: KBUID=A09FK3" -H "Authorization: Basic WFhfTk9CWV9STVRfVVNSX1RFU1Q6cHBtbGVzbnJUV1lTRFlHRFIhOTg1Mzg1MzU2MzQ1NDQ=" 172.30.35.51:30009 DomainServices.SalesArrangementService.v1.SalesArrangementService/GetFlowSwitches
grpcurl -insecure -d "{\"SalesArrangementId\":20000,\"FlowSwitches\":[{\"FlowSwitchId\":1,\"Value\":true},{\"FlowSwitchId\":2,\"Value\":false}]}" -H "noby-user-id: 3048" -H "noby-user-ident: KBUID=A09FK3" -H "Authorization: Basic WFhfTk9CWV9STVRfVVNSX1RFU1Q6cHBtbGVzbnJUV1lTRFlHRFIhOTg1Mzg1MzU2MzQ1NDQ=" 172.30.35.51:30009 DomainServices.SalesArrangementService.v1.SalesArrangementService/SetFlowSwitches
grpcurl -insecure -d "{\"CaseId\":3047708}" -H "Authorization: Basic WFhfTk9CWV9STVRfVVNSX1RFU1Q6cHBtbGVzbnJUV1lTRFlHRFIhOTg1Mzg1MzU2MzQ1NDQ=" 172.30.35.51:30009 DomainServices.SalesArrangementService.v1.SalesArrangementService/GetProductSalesArrangementId

## run batch
dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\InternalServices\ServiceDiscovery\Api\CIS.InternalServices.ServiceDiscovery.Api.csproj"
dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\DomainServices\CodebookService\Api\DomainServices.CodebookService.Api.csproj"
dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\DomainServices\CaseService\Api\DomainServices.CaseService.Api.csproj"
dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\DomainServices\OfferService\Api\DomainServices.OfferService.Api.csproj"
dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\DomainServices\SalesArrangementService\Api\DomainServices.SalesArrangementService.Api.csproj"


