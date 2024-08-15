# DomainServices.UserService.Api

## grpcurl tests
grpcurl -insecure 127.0.0.1:30010 list
grpcurl -insecure -d "{\"Identity\":{\"identityScheme\":\"KbUid\",\"identity\":\"A02DGZ\"}}" -H "Authorization: Basic YTph" -H "noby-user-id: 267" 127.0.0.1:30010 DomainServices.UserService.v1.UserService/GetUser
grpcurl -insecure -d "{\"Identity\":{\"identityScheme\":\"V33Id\",\"identity\":\"44361\"}}" -H "Authorization: Basic WFhfTk9CWV9STVRfVVNSX1RFU1Q6cHBtbGVzbnJUV1lTRFlHRFIhOTg1Mzg1MzU2MzQ1NDQ=" -H "noby-user-id: 267" 172.30.35.51:30010 DomainServices.UserService.v1.UserService/GetUser
grpcurl -insecure -d "{\"Identity\":{\"identityScheme\":\"OsCis\",\"identity\":\"614\"}}" -H "Authorization: Basic YTph" -H "noby-user-id: 267" 127.0.0.1:30010 DomainServices.UserService.v1.UserService/GetUser
grpcurl -insecure -d "{\"Identity\":\"990614w\",\"IdentityScheme\":\"MPAD\"}" -H "Authorization: Basic WFhfTk9CWV9STVRfVVNSX1RFU1Q6cHBtbGVzbnJUV1lTRFlHRFIhOTg1Mzg1MzU2MzQ1NDQ=" -H "noby-user-id: 267" 172.30.35.51:30010 DomainServices.UserService.v1.UserService/GetUserRIPAttributes
grpcurl -insecure -d "{\"Identity\":\"990614w\",\"IdentityScheme\":\"MPAD\"}" -H "Authorization: Basic WFhfTk9CWV9STVRfVVNSX1RFU1Q6cHBtbGVzbnJUV1lTRFlHRFIhOTg1Mzg1MzU2MzQ1NDQ=" -H "noby-user-id: 267" 127.0.0.1:30010 DomainServices.UserService.v1.UserService/GetUserRIPAttributes
grpcurl -insecure -d "{\"UserId\":267}" -H "Authorization: Basic YTph" -H "noby-user-id: 267" 127.0.0.1:30010 DomainServices.UserService.v1.UserService/GetUserBasicInfo
grpcurl -insecure -d "{\"UserId\":69466}" -H "Authorization: Basic YTph" -H "noby-user-id: 267" 127.0.0.1:30010 DomainServices.UserService.v1.UserService/GetUserPermissions
grpcurl -insecure -d "{\"IdentityScheme\":\"KbUid\",\"Identity\":\"A02DGZ\"}" -H "Authorization: Basic YTph" -H "noby-user-id: 267" 127.0.0.1:30010 DomainServices.UserService.v1.UserService/GetUserRIPAttributes
grpcurl -insecure -d "{\"UserId\":75248}" -H "Authorization: Basic YTph" -H "noby-user-id: 267" 127.0.0.1:30010 DomainServices.UserService.v1.UserService/GetUserMortgageSpecialist

grpcurl -insecure -d "{\"Identity\":{\"identityScheme\":\"V33Id\",\"identity\":\"44361\"}}" -H "Authorization: Basic YTph" -H "noby-user-id: 267" 127.0.0.1:30010 DomainServices.UserService.v1.UserService/GetUser

## run batch
dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\InternalServices\ServiceDiscovery\Api\CIS.InternalServices.ServiceDiscovery.Api.csproj"
dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\DomainServices\UserService\Api\DomainServices.UserService.Api.csproj"
