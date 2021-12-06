# DomainServices.OfferService.Api

## Vygenerování EAS wrapperu
        dotnet-svcutil "d:\...\DomainServices\OfferService\EAS_WS_SB_Services.xml" -o c:/EasWrapper.cs -i -n *,DomainServices.OfferService.Api.Eas.EasWrapper

## grpcurl tests
        grpcurl -insecure 172.30.35.51:5003 list
        grpcurl -insecure -d "{\"OfferInstanceId\":1}" -H "Authorization: Basic YTph" 172.30.35.51:5003 DomainServices.OfferService.v1.OfferService/GetBuildingSavingsData
        grpcurl -insecure -d "{\"InputData\":{\"TargetAmount\":500000,\"ProductCode\":61,\"ActionCode\":30,\"IsWithLoan\":false},\"ResourceProcessId\":\"4D115798-0E05-4CF0-8A5A-1A3F871B3726\"}" -H "Authorization: Basic YTph" 127.0.0.1:5020 DomainServices.OfferService.v1.OfferService/SimulateBuildingSavings
