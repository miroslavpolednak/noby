# DomainServices.ProductService.Api

## grpcurl tests
grpcurl -insecure 127.0.0.1:30015 list
grpcurl -insecure 127.0.0.1:30015 grpc.health.v1.Health/Check
grpcurl -insecure -d "{\"PhoneNumber\":\"+420608967971\",\"Type\":\"INSIGN_PROCESS\",\"Text\":\"testovaci text\",\"CustomId\":\"\"}" -H "noby-user-id: 3048" -H "noby-user-ident: KBUID=A09FK3" -H "Authorization: Basic YTph" 127.0.0.1:30015 CIS.InternalServices.NotificationService.v2.NotificationService/SendSms
grpcurl -insecure -d "{\"PhoneNumber\":\"+420608967971\",\"ProcessingPriority\":0,\"Type\":\"INSIGN_PROCESS\",\"Text\":\"testovaci text\"}" -H "noby-user-id: 3048" -H "noby-user-ident: KBUID=A09FK3" -H "Authorization: Basic YTph" 127.0.0.1:30015 CIS.InternalServices.NotificationService.v2.NotificationService/SendSms

grpcurl -insecure -d "{\"From\":{\"Value\":\"info@kb.cz\"},\"To\":[{\"Value\":\"filip@mpss.cz\"}],\"Subject\":\"testovaci email\",\"CaseId\":1111,\"Content\":{\"Format\":\"text/html\",\"Language\":\"cs\",\"Text\":\"\"}}" -H "noby-user-id: 3048" -H "noby-user-ident: KBUID=A09FK3" -H "Authorization: Basic YTph" 127.0.0.1:30015 CIS.InternalServices.NotificationService.v2.NotificationService/SendEmail

## run batch
dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\InternalServices\NotificationService\Api\CIS.InternalServices.NotificationService.Api.csproj"
