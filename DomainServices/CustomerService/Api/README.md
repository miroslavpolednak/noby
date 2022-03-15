# DomainServices.CustomerService.Api

## grpcurl tests
        grpcurl -insecure 172.30.35.51:5005 list
        
		grpcurl -insecure -d "{\"NaturalPerson\":{\"LastName\":\"Dvořák\"}}" -H "Authorization: Basic YTph" 172.30.35.51:5005 DomainServices.CustomerService.V1.CustomerService/SearchCustomers
        grpcurl -insecure -d "{\"Identity\":{\"identityId\":928532258,\"identityScheme\":2}}" -H "Authorization: Basic YTph" 172.30.35.51:5005 DomainServices.CustomerService.V1.CustomerService/SearchCustomers
        grpcurl -insecure -d "{\"IdentificationDocument\":{\"IdentificationDocumentTypeId\":1,\"IssuingCountryId\":16,\"Number\":\"54324525432\"}}" -H "Authorization: Basic YTph" 172.30.35.51:5005 DomainServices.CustomerService.V1.CustomerService/SearchCustomers
        
        grpcurl -insecure -d "{\"Identities\":[{\"identityId\":145212071,\"identityScheme\":2}]}" -H "Authorization: Basic YTph" 172.30.35.51:5005 DomainServices.CustomerService.V1.CustomerService/GetCustomerList
		
		grpcurl -insecure -d "{\"Identity\":{\"identityId\":145212071,\"identityScheme\":2}}" -H "Authorization: Basic YTph" 172.30.35.51:5005 DomainServices.CustomerService.V1.CustomerService/GetCustomerDetail