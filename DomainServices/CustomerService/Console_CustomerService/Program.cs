using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using DomainServices.CustomerService.Abstraction;
using System;
using Grpc.Net.Client;
using ProtoBuf.Grpc.Client;
using ProtoBuf.Grpc;
using System.Net.Http;
using CIS.Core.Security;
using CIS.Foms.Enums;
using CIS.Infrastructure.gRPC.CisTypes;
using Console_CustomerService;
using DomainServices.CustomerService.Contracts;
using Mandants = CIS.Infrastructure.gRPC.CisTypes.Mandants;

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

var service = serviceProvider.GetRequiredService<ICustomerServiceAbstraction>();

//var search = await service.SearchCustomers(new SearchCustomersRequest
//{
//    Mandant = Mandants.Mp,
//    IdentificationDocument = new IdentificationDocumentSearch
//    {
//        IssuingCountryId = 16,
//        Number = "205721585",
//        IdentificationDocumentTypeId = 1
//    }
//});

//var test = await service.GetCustomerList(new CustomerListRequest
//{
//    Identities =
//    {
//        new Identity(34, IdentitySchemes.Mp),
//        new Identity(123, IdentitySchemes.Kb)
//    }
//});

//var test = await service.ProfileCheck(new ProfileCheckRequest
//{
//    Identity = new Identity(123, IdentitySchemes.Kb),
//    CustomerProfileCode = "KYC_SUBJECT"
//});

var detail = await service.GetCustomerDetail(new Identity(123, IdentitySchemes.Kb));

Console.ReadKey();