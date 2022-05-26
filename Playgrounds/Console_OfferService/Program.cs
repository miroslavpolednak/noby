using Grpc.Core;
using Microsoft.Extensions.DependencyInjection;
using DomainServices.OfferService.Abstraction;
using CIS.InternalServices.ServiceDiscovery.Abstraction;
using System;
using Grpc.Net.Client;
using ProtoBuf.Grpc.Client;
using ProtoBuf.Grpc;
using System.Net.Http;
using DomainServices.OfferService.Contracts;

Console.WriteLine("run!");

var plainTextBytes = System.Text.Encoding.UTF8.GetBytes($"a:a");
var _authorizationHeader = Convert.ToBase64String(plainTextBytes);
var headerValue = new Metadata.Entry("Authorization", "Basic " + _authorizationHeader);
var headers = new Metadata();
headers.Add(headerValue);
var options = new CallOptions(headers: headers);

// save
/*using (var channel = GrpcChannel.ForAddress("https://localhost:5051"))
{
    var svc = channel.CreateGrpcService<DomainServices.OfferService.Contracts.IOfferService>();
    var result = await svc.SimulateHousingsSavingsWithLoan(new DomainServices.OfferService.Contracts.SimulateHousingsSavingsLoanRequest
    {
        ClientIsSVJ = false,
        ActionCode = default(int?),
        ClientIsNaturalPerson = true,
        ProductCode = 61,
        TargetAmount = 300000
    }, new CallContext(options));

    Console.WriteLine(result.OfferInstanceId);
}*/

//get
/*using (var channel = GrpcChannel.ForAddress("https://localhost:5051", new GrpcChannelOptions
{
    HttpHandler = new HttpClientHandler() 
    {
        ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    }
}))
{
    var svc = channel.CreateGrpcService<DomainServices.OfferService.Contracts.IOfferService>();
    var result = await svc.PrintBuildingSavingsOffer(
        new DomainServices.OfferService.Contracts.PrintBuildingSavingsOfferRequest { OfferInstanceId = 1, Dealer = new DomainServices.OfferService.DealerData() { FirstName = "Filip" } }, 
        new CallContext(options));

    Console.WriteLine(result.OfferInstanceId);
    System.IO.File.WriteAllBytes("d:/test.pdf", result.FileData);
}*/

// call with abstraction layer
/*var serviceProvider = new ServiceCollection()
    .AddLogging()
    .AddSingleton<CIS.Core.Configuration.ICisEnvironmentConfiguration>(new CIS.Infrastructure.Configuration.CisEnvironmentConfiguration
    {
        EnvironmentName = args[0],
        DefaultApplicationKey = "console",
        ServiceDiscoveryUrl = args[1],
        InternalServicesLogin = "a",
        InternalServicePassword = "a"
    })
    .AddHttpContextAccessor()
    .AddCisServiceDiscovery(true)
    .AddOfferService(true)//"https://localhost:5051" "https://172.30.35.51:5051
    .BuildServiceProvider();*/
var serviceProvider = new ServiceCollection()
    .AddLogging()
    .AddSingleton<CIS.Core.Configuration.ICisEnvironmentConfiguration>(new CIS.Infrastructure.Configuration.CisEnvironmentConfiguration
    {
        EnvironmentName = "uat",
        DefaultApplicationKey = "console",
        ServiceDiscoveryUrl = "https://localhost:5005",
        InternalServicesLogin = "a",
        InternalServicePassword = "a"
    })
    .AddHttpContextAccessor()
    .AddOfferService("https://localhost:5010", true)
    .BuildServiceProvider();

var service = serviceProvider.GetService<DomainServices.OfferService.Abstraction.IOfferServiceAbstraction>();

var inputData = new SimulateMortgageRequest
{
    SimulationInputs = new SimulationInputs
    {

    }
};
var result = await service.SimulateMortgage(inputData);
//var result = await service.PrintBuildingSavingsOffer(new() { OfferInstanceId = 1, Dealer = new() { FirstName = "Filip" } });

Console.WriteLine($"{result.Success}");
Console.WriteLine(((CIS.Core.Results.SuccessfulServiceCallResult<DomainServices.OfferService.Contracts.SimulateMortgageResponse>)result).Model.OfferId);

//System.IO.File.WriteAllBytes("d:/test.pdf", filedata.ToByteArray());
