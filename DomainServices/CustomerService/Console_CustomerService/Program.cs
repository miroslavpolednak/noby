using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using System;
using Grpc.Net.Client;
using ProtoBuf.Grpc.Client;
using ProtoBuf.Grpc;
using System.Net.Http;
using CIS.Core.Security;
using CIS.Foms.Enums;
using CIS.Infrastructure.gRPC.CisTypes;
using Console_CustomerService;
using DomainServices.CustomerService.Clients;
using DomainServices.CustomerService.Contracts;

Console.WriteLine("run!");

var plainTextBytes = System.Text.Encoding.UTF8.GetBytes($"a:a");
var _authorizationHeader = Convert.ToBase64String(plainTextBytes);
var headerValue = new Metadata.Entry("Authorization", "Basic " + _authorizationHeader);
var headers = new Metadata();
headers.Add(headerValue);
var options = new CallOptions(headers: headers);

var serviceProvider = new ServiceCollection()
    .AddLogging()
    .AddTransient<ICurrentUserAccessor, MockCurrentUserAccessor>()
    .AddSingleton<CIS.Core.Configuration.ICisEnvironmentConfiguration>(new CisEnvironmentConfiguration
    {
        EnvironmentName = "uat",
        DefaultApplicationKey = "console",
        ServiceDiscoveryUrl = "https://localhost:5005",
        InternalServicesLogin = "a",
        InternalServicePassword = "a"
    })
    .AddCustomerService("https://localhost:5100")
    .BuildServiceProvider();

var service = serviceProvider.GetRequiredService<ICustomerServiceClient>();

//var search = await service.SearchCustomers(new SearchCustomersRequest
//{
//    Mandant = Mandants.Mp,
//    NaturalPerson = new NaturalPersonSearch
//    {
//        LastName = "Novák",
//        FirstName = "Adam"
//    }
//});

//var test = await service.GetCustomerList(new Identity[]
//{
//    new(951011020, IdentitySchemes.Kb),
//    new(123, IdentitySchemes.Kb),
//    new(134, IdentitySchemes.Mp)
//});

//var test = await service.ProfileCheck(new ProfileCheckRequest
//{
//    Identity = new Identity(123, IdentitySchemes.Kb),
//    CustomerProfileCode = "KYC_SUBJECT"
//});

//var detail = await service.GetCustomerDetail(new Identity(926949615, IdentitySchemes.Kb));

var create = await service.CreateCustomer(new CreateCustomerRequest
{
    Identity = new Identity(default, IdentitySchemes.Kb),
    NaturalPerson = new NaturalPerson
    {
        FirstName = "Qvratek",
        LastName = "Qliteks",
        BirthCountryId = 16,
        BirthNumber = "8105144322",
        BirthName = "Prouza",
        DateOfBirth = new NullableGrpcDate(1981, 5, 14),
        PlaceOfBirth = "Ostrava",
        GenderId = 1
    },
    Addresses =
    {
        new GrpcAddress
        {
            AddressTypeId = 1,
            City = "Praha",
            Postcode = "19017",
            CountryId = 1,
            Street = "Masarykova",
            HouseNumber = "458",
            StreetNumber = "9A",
            CityDistrict = "Vinoř",
            PragueDistrict = "Praha 9",
            DeliveryDetails = "Marketing Department",
            AddressPointId = "465465465",
            PrimaryAddressFrom = DateTime.Now.AddYears(-5)
        }
    },
    IdentificationDocument = new IdentificationDocument
    {
        IdentificationDocumentTypeId = 1,
        Number = "893123457",
        IssuedBy = "Praha",
        IssuingCountryId = 16,
        IssuedOn = DateTime.Now.AddYears(-5),
        ValidTo = DateTime.Now.AddYears(3)
    }
});

Console.ReadKey();