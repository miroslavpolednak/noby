﻿# DomainServices.CustomerService.Api

## grpcurl tests
grpcurl -insecure 172.30.35.51:5005 list
        
grpcurl -insecure -d "{\"CustomerProfileCode\":\"IDENTIFIED_SUBJECT\",\"Identity\":{\"identityId\":939793250,\"identityScheme\":2}}" -H "Authorization: Basic YTph" 127.0.0.1:30004 DomainServices.CustomerService.V1.CustomerService/ProfileCheck

grpcurl -insecure -d "{\"Identity\":{\"identityId\":300549515,\"identityScheme\":1},\"Mandant\":1}" -H "Authorization: Basic YTph" 127.0.0.1:30004 DomainServices.CustomerService.V1.CustomerService/SearchCustomers
grpcurl -insecure -d "{\"Identity\":{\"identityId\":300549515,\"identityScheme\":1},\"Mandant\":1}" -H "Authorization: Basic WFhfTk9CWV9STVRfVVNSX1RFU1Q6cHBtbGVzbnJUV1lTRFlHRFIhOTg1Mzg1MzU2MzQ1NDQ=" 172.30.35.51:30004 DomainServices.CustomerService.V1.CustomerService/SearchCustomers
grpcurl -insecure -d "{\"Mandant\":1,\"NaturalPerson\":{\"FirstName\":\"ALEX\",\"LastName\":\"MADAGASKAR\"}}" -H "Authorization: Basic WFhfTk9CWV9STVRfVVNSX1RFU1Q6cHBtbGVzbnJUV1lTRFlHRFIhOTg1Mzg1MzU2MzQ1NDQ=" 172.30.35.51:30004 DomainServices.CustomerService.V1.CustomerService/SearchCustomers
grpcurl -insecure -d "{\"Identity\":{\"identityId\":928532258,\"identityScheme\":2}}" -H "Authorization: Basic YTph" 172.30.35.51:5005 DomainServices.CustomerService.V1.CustomerService/SearchCustomers
grpcurl -insecure -d "{\"IdentificationDocument\":{\"IdentificationDocumentTypeId\":1,\"IssuingCountryId\":16,\"Number\":\"54324525432\"}}" -H "Authorization: Basic YTph" 172.30.35.51:5005 DomainServices.CustomerService.V1.CustomerService/SearchCustomers
        
grpcurl -insecure -d "{\"Identities\":[{\"identityId\":145212071,\"identityScheme\":2}]}" -H "Authorization: Basic YTph" 172.30.35.51:5005 DomainServices.CustomerService.V1.CustomerService/GetCustomerList
		
grpcurl -insecure -d "{\"Identity\":{\"identityId\":145212071,\"identityScheme\":2}}" -H "Authorization: Basic YTph" 127.0.0.1:30004 DomainServices.CustomerService.V1.CustomerService/GetCustomerDetail
grpcurl -insecure -d "{\"Identity\":{\"identityId\":300548609,\"identityScheme\":1}}" -H "Authorization: Basic YTph" 127.0.0.1:30004 DomainServices.CustomerService.V1.CustomerService/GetCustomerDetail
grpcurl -insecure -d "{\"Identity\":{\"identityId\":145212071,\"identityScheme\":2}}" -H "noby-user-id: 3048" -H "noby-user-ident: KBUID=A09FK3" -H "Authorization: Basic YTph" 127.0.0.1:30004 DomainServices.CustomerService.V1.CustomerService/GetCustomerDetail

grpcurl -insecure -d "{\"Identities\":[{\"identityId\":300527075,\"identityScheme\":1},{\"identityId\":951078698,\"identityScheme\":2}],\"HardCreate\":true,\"NaturalPerson\":{\"FirstName\":\"CREATE\",\"LastName\":\"CASE\",\"DateOfBirth\":{\"year\":1989,\"month\":11,\"day\":7},\"BirthNumber\":\"8911070377\",\"GenderId\":1,\"BirthName\":\"\",\"PlaceOfBirth\":\"Praha\",\"BirthCountryId\":16,\"CitizenshipCountriesId\":[16],\"MaritalStatusStateId\":2,\"DegreeBeforeId\":20,\"DegreeAfterId\":null,\"IsPoliticallyExposed\":null,\"EducationLevelId\":0,\"IsBrSubscribed\":false,\"KbRelationshipCode\":\"N\",\"Segment\":\"\",\"IsUSPerson\":false,\"LegalCapacity\":{\"RestrictionTypeId\":null,\"RestrictionUntil\":null},\"TaxResidence\":{\"ValidFrom\":null,\"ResidenceCountries\":[]},\"ProfessionCategoryId\":null,\"ProfessionId\":null,\"NetMonthEarningAmountId\":null,\"NetMonthEarningTypeId\":null},\"IdentificationDocument\":{\"IdentificationDocumentTypeId\":1,\"IssuingCountryId\":16,\"Number\":\"998001009\",\"ValidTo\":{\"year\":2030,\"month\":1,\"day\":1},\"IssuedOn\":{\"year\":2020,\"month\":1,\"day\":1},\"IssuedBy\":\"Olomouc\",\"RegisterPlace\":\"\"},\"Addresses\":[{\"Street\":\"Generála Fajtla\",\"StreetNumber\":\"2\",\"HouseNumber\":\"943\",\"Postcode\":\"77900\",\"City\":\"Olomouc\",\"CountryId\":null,\"AddressTypeId\":1,\"EvidenceNumber\":\"\",\"IsPrimary\":false,\"DeliveryDetails\":\"\",\"CityDistrict\":\"\",\"PragueDistrict\":\"\",\"CountrySubdivision\":\"\",\"PrimaryAddressFrom\":null,\"AddressPointId\":\"\"}],\"Contacts\":[],\"Mandant\":1,\"CustomerIdentification\":null}" -H "Authorization: Basic YTph" 127.0.0.1:30004 DomainServices.CustomerService.V1.CustomerService/CreateCustomer


## run batch
dotnet run --project "d:\Visual Studio Projects\MPSS-FOMS\DomainServices\CustomerService\Api\DomainServices.CustomerService.Api.csproj"