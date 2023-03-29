# DomainServices.UserService.Api

## grpcurl tests
        grpcurl -insecure 127.0.0.1:5095 list
        grpcurl -insecure -d "{\"Login\":\"990614w\"}" -H "Authorization: Basic YTph" 127.0.0.1:5095 DomainServices.UserService.v1.UserService/GetUserByLogin
        grpcurl -insecure -d "{\"UserId\":267}" -H "Authorization: Basic OTkwNjE0dzpQcmlwb3NyYW5lMCk=" -H "noby-user-id: 267" 127.0.0.1:30010 DomainServices.UserService.v1.UserService/GetUser

## run batch
        dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\InternalServices\ServiceDiscovery\Api\CIS.InternalServices.ServiceDiscovery.Api.csproj"
        dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\DomainServices\UserService\Api\DomainServices.UserService.Api.csproj"
