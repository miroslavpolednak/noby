﻿# DomainServices.ProductService.Api

## grpcurl tests
grpcurl -insecure 127.0.0.1:30015 list
grpcurl -insecure 127.0.0.1:30015 grpc.health.v1.Health/Check

V2
// send sms
grpcurl -insecure -d "{\"PhoneNumber\":\"+420608967971\",\"Type\":\"INSIGN_PROCESS\",\"Text\":\"testovaci text\",\"CustomId\":\"XXXX\"}" -H "noby-user-id: 3048" -H "noby-user-ident: KBUID=A09FK3" -H "Authorization: Basic YTph" 127.0.0.1:30015 CIS.InternalServices.NotificationService.v2.NotificationService/SendSms
grpcurl -insecure -d "{\"PhoneNumber\":\"+420608967971\",\"ProcessingPriority\":0,\"Type\":\"INSIGN_PROCESS\",\"Text\":\"testovaci text\"}" -H "noby-user-id: 3048" -H "noby-user-ident: KBUID=A09FK3" -H "Authorization: Basic YTph" 127.0.0.1:30015 CIS.InternalServices.NotificationService.v2.NotificationService/SendSms

// send email
grpcurl -insecure -d "{\"From\":{\"Value\":\"info@kb.cz\"},\"To\":[{\"Value\":\"filip@mpss.cz\"}],\"Subject\":\"testovaci email\",\"CaseId\":1111,\"Content\":{\"Format\":0,\"Language\":0,\"Text\":\"testovaci email\"}}" -H "noby-user-id: 3048" -H "noby-user-ident: KBUID=A09FK3" -H "Authorization: Basic YTph" 127.0.0.1:30015 CIS.InternalServices.NotificationService.v2.NotificationService/SendEmail

// get result
grpcurl -insecure -d "{\"NotificationId\":\"1b6beb5d-68e8-43c7-9bcd-482dc142ea11\"}" -H "noby-user-id: 3048" -H "noby-user-ident: KBUID=A09FK3" -H "Authorization: Basic YTph" 127.0.0.1:30015 CIS.InternalServices.NotificationService.v2.NotificationService/GetResult

// search result
grpcurl -insecure -d "{\"CaseId\":\"1111\"}" -H "noby-user-id: 3048" -H "noby-user-ident: KBUID=A09FK3" -H "Authorization: Basic YTph" 127.0.0.1:30015 CIS.InternalServices.NotificationService.v2.NotificationService/SearchResults

V1
grpcurl -insecure -d "{\"PhoneNumber\":\"+420608967971\",\"ProcessingPriority\":0,\"Type\":\"INSIGN_PROCESS\",\"Text\":\"testovaci text\"}" -H "noby-user-id: 3048" -H "noby-user-ident: KBUID=A09FK3" -H "Authorization: Basic WFhfS0JJTlNHX1JNVF9VU1JfVEVTVDpoZ2ZEUllUSFNESU9KRmQhITM0NDU0Njc4OS4uLi4uLg==" 172.30.35.52:33015 CIS.InternalServices.NotificationService/SendSms

## run batch
dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\InternalServices\NotificationService\Api\CIS.InternalServices.NotificationService.Api.csproj"
