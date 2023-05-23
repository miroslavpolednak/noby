# DomainServices.UserService.Api

## grpcurl tests
        grpcurl -insecure 127.0.0.1:30010 list
        grpcurl -insecure -d "{\"Identity\":{\"identityScheme\":\"KbUid\",\"identity\":\"A0AXX9\"}}" -H "Authorization: Basic YTph" -H "noby-user-id: 267" 127.0.0.1:30010 DomainServices.UserService.v1.UserService/GetUser
        grpcurl -insecure -d "{\"Identity\":{\"identityScheme\":\"OsCis\",\"identity\":\"614\"}}" -H "Authorization: Basic YTph" -H "noby-user-id: 267" 127.0.0.1:30010 DomainServices.UserService.v1.UserService/GetUser

## run batch
        dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\InternalServices\ServiceDiscovery\Api\CIS.InternalServices.ServiceDiscovery.Api.csproj"
        dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\DomainServices\UserService\Api\DomainServices.UserService.Api.csproj"
