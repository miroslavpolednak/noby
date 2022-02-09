# DomainServices.OfferService.Api

## Vygenerování EAS wrapperu
        dotnet-svcutil "d:\...\DomainServices\OfferService\EAS_WS_SB_Services.xml" -o c:/EasWrapper.cs -i -n *,DomainServices.OfferService.Api.Eas.EasWrapper

## grpcurl tests
        grpcurl -insecure 172.30.35.51:5003 list DomainServices.OfferService.v1.OfferService
       
        grpcurl -insecure -d "{\"OfferInstanceId\":15}" -H "Authorization: Basic YTph" 127.0.0.1:5020 DomainServices.OfferService.v1.OfferService/GetOfferInstance
        grpcurl -insecure -d "{\"OfferInstanceId\":1}" -H "Authorization: Basic YTph" 127.0.0.1:5020 DomainServices.OfferService.v1.OfferService/GetMortgageData
        grpcurl -insecure -d "{\"InputData\":{\"ProductInstanceTypeId\":5},\"ResourceProcessId\":\"4D115798-0E05-4CF0-8A5A-1A3F871B3727\"}" -H "Authorization: Basic YTph" 127.0.0.1:5020 DomainServices.OfferService.v1.OfferService/SimulateMortgage
        grpcurl -insecure -d "{\"InputData\":{\"ProductInstanceTypeId\":5,\"LoanKindId\":1,\"LoanAmount\":{\"units\":5000000},\"LoanDuration\":120,\"LoanPaymentAmount\":{\"units\":15000},\"FixedLengthPeriodId\":2,\"EmployeeBonusLoanCode\":5,\"CollateralAmount\":{\"units\":2000000},\"LoanToValue\":{\"units\":3000000},\"PaymentDayOfTheMonth\":20 },\"ResourceProcessId\":\"4D115798-0E05-4CF0-8A5A-1A3F871B3727\"}" -H "Authorization: Basic YTph" 127.0.0.1:5020 DomainServices.OfferService.v1.OfferService/SimulateMortgage
        grpcurl -insecure -d "{\"InputData\":{\"ProductInstanceTypeId\":6,\"LoanPurpose\":[{\"ProductLoanPurposeId\":1,\"Sum\":{\"units\":1000000}}, {\"ProductLoanPurposeId\":2,\"Sum\":{\"units\":2000000}}]},\"ResourceProcessId\":\"4D115798-0E05-4CF0-8A5A-1A3F871B3727\"}" -H "Authorization: Basic YTph" 127.0.0.1:5020 DomainServices.OfferService.v1.OfferService/SimulateMortgage


## run batch
        dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\InternalServices\ServiceDiscovery\Api\CIS.InternalServices.ServiceDiscovery.Api.csproj"
        dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\DomainServices\CodebookService\Api\DomainServices.CodebookService.Api.csproj"
        dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\DomainServices\OfferService\Api\DomainServices.OfferService.Api.csproj"
