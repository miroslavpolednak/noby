using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using DomainServices.CustomerService.Abstraction;
using CIS.InternalServices.ServiceDiscovery.Abstraction;
using System;
using Grpc.Net.Client;
using ProtoBuf.Grpc.Client;
using ProtoBuf.Grpc;
using System.Net.Http;
using Console_CustomerService;

Console.WriteLine("run!");

var plainTextBytes = System.Text.Encoding.UTF8.GetBytes($"a:a");
var _authorizationHeader = Convert.ToBase64String(plainTextBytes);
var headerValue = new Metadata.Entry("Authorization", "Basic " + _authorizationHeader);
var headers = new Metadata();
headers.Add(headerValue);
var options = new CallOptions(headers: headers);

var serviceProvider = new ServiceCollection()
    .AddLogging()
    .AddSingleton<CIS.Core.Configuration.ICisEnvironmentConfiguration>(new CisEnvironmentConfiguration
    {
        EnvironmentName = "uat",
        DefaultApplicationKey = "console",
        ServiceDiscoveryUrl = "https://localhost:5005",
        InternalServicesLogin = "a",
        InternalServicePassword = "a"
    })
    .AddHttpContextAccessor()
    .AddCustomerService("https://localhost:5052", true)
    .BuildServiceProvider();

var service = serviceProvider.GetService<DomainServices.CustomerService.Abstraction.ICustomerServiceAbstraction>();
//var service = serviceProvider.GetService<DomainServices.CustomerService.Contracts.ICustomerService>();

#pragma warning disable CS8602 // Dereference of a possibly null reference.
//var result = await service.GetBasicDataByIdentifier(new () { Identifier = "123456" });
#pragma warning restore CS8602 // Dereference of a possibly null reference.
//var result = await service.GetDetail(new() { Identity = 1 });
//var result = await service.GetList(new() { Identity = new() { 1 } });
//var result = await service.Create(new() { LastName = "Kojot" });
//var result = await service.UpdateBasicData(new() { Identity = 1, Customer = new() { LastName = "Kojot" } });
//var result = await service.DeleteContact(new() { Identity = 1, ContactId = 1 });

//var o = ((CIS.Core.Results.SuccessfulServiceCallResult<DomainServices.CustomerService.Contracts.GetBasicDataByIdentifierResponse>)result).Model;
//var o = ((CIS.Core.Results.SuccessfulServiceCallResult<DomainServices.CustomerService.Contracts.GetCustomerDetailResponse>)result).Model;
//var o = ((CIS.Core.Results.SuccessfulServiceCallResult<DomainServices.CustomerService.Contracts.GetListResponse>)result).Model;
//var o = ((CIS.Core.Results.SuccessfulServiceCallResult<DomainServices.CustomerService.Contracts.CreateResponse>)result).Model;

//Console.WriteLine($"{result.LastName}");
//Console.WriteLine($"{result.Success}");