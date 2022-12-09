# DomainServices.OfferService.Api

## Vygenerování EAS wrapperu
        dotnet-svcutil "d:\...\DomainServices\OfferService\EAS_WS_SB_Services.xml" -o c:/EasWrapper.cs -i -n *,DomainServices.OfferService.Api.Eas.EasWrapper

## grpcurl tests
        grpcurl -insecure 172.30.35.51:31006 list DomainServices.OfferService.v1.OfferService
       
        grpcurl -insecure -d "{\"OfferId\":6}" -H "Authorization: Basic YTph" 172.30.35.51:31006 DomainServices.OfferService.v1.OfferService/GetOffer
        grpcurl -insecure -d "{\"OfferId\":6}" -H "Authorization: Basic YTph" 172.30.35.51:31006 DomainServices.OfferService.v1.OfferService/GetMortgageOffer
        grpcurl -insecure -d "{\"OfferId\":6}" -H "Authorization: Basic YTph" 172.30.35.51:31006 DomainServices.OfferService.v1.OfferService/GetMortgageOfferDetail
        grpcurl -insecure -d "{\"OfferId\":6}" -H "Authorization: Basic YTph" 172.30.35.51:31006 DomainServices.OfferService.v1.OfferService/GetMortgageOfferFPSchedule
        grpcurl -insecure -d "{\"ResourceProcessId\":\"4D115798-0E05-4CF0-8A5A-1A3F871B3727\", \"BasicParameters\": {}, \"SimulationInputs\": {\"RealEstateInsurance\":{\"Sum\":null,\"Frequency\":12},\"ExpectedDateOfDrawing\":{\"year\":2022,\"month\":12,\"day\":15},\"ProductTypeId\":20001, \"LoanKindId\":2000, \"LoanAmount\":{\"units\":3150020},\"LoanDuration\":36, \"GuaranteeDateFrom\": {\"year\":2022,\"month\":12,\"day\":2 }, \"InterestRateDiscount\": {\"units\":1}, \"FixedRatePeriod\": 24, \"CollateralAmount\": {\"units\":6500000}, \"LoanPurposes\":[{\"LoanPurposeId\":201,\"Sum\":{\"units\":1000000}}, {\"LoanPurposeId\":202,\"Sum\":{\"units\":2000000}}], \"MarketingActions\":{\"Domicile\": true} }}" -H "Authorization: Basic YTph" 127.0.0.1:5020 DomainServices.OfferService.v1.OfferService/SimulateMortgage

        grpcurl -insecure -d "{\"OfferId\":6}" -H "Authorization: Basic YTph" 127.0.0.1:5020 DomainServices.OfferService.v1.OfferService/GetOffer
        grpcurl -insecure -d "{\"OfferId\":6}" -H "Authorization: Basic YTph" 127.0.0.1:5020 DomainServices.OfferService.v1.OfferService/GetMortgageOffer
        grpcurl -insecure -d "{\"OfferId\":6}" -H "Authorization: Basic YTph" 127.0.0.1:5020 DomainServices.OfferService.v1.OfferService/GetMortgageOfferDetail
        grpcurl -insecure -d "{\"OfferId\":6}" -H "Authorization: Basic YTph" 127.0.0.1:5020 DomainServices.OfferService.v1.OfferService/GetMortgageOfferFPSchedule
        grpcurl -insecure -d "{\"ResourceProcessId\":\"4D115798-0E05-4CF0-8A5A-1A3F871B3727\", \"BasicParameters\": {}, \"SimulationInputs\": {\"ProductTypeId\":20001, \"LoanKindId\":2000, \"LoanAmount\":{\"units\":3150020},\"LoanDuration\":36, \"GuaranteeDateFrom\": {\"year\":2022,\"month\":5,\"day\":15 }, \"InterestRateDiscount\": {\"units\":1}, \"FixedRatePeriod\": 24, \"CollateralAmount\": {\"units\":6500000}, \"DrawingDuration\": 0, \"LoanPurposes\":[{\"LoanPurposeId\":201,\"Sum\":{\"units\":1000000}}, {\"LoanPurposeId\":202,\"Sum\":{\"units\":2000000}}], \"MarketingActions\":{\"Domicile\": true} }}" -H "Authorization: Basic YTph" 127.0.0.1:5020 DomainServices.OfferService.v1.OfferService/SimulateMortgage

## run batch
        dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\InternalServices\ServiceDiscovery\Api\CIS.InternalServices.ServiceDiscovery.Api.csproj"
        dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\DomainServices\CodebookService\Api\DomainServices.CodebookService.Api.csproj"
        dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\DomainServices\OfferService\Api\DomainServices.OfferService.Api.csproj"
