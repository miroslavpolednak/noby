﻿// See https://aka.ms/new-console-template for more information

using CIS.Core.Configuration;
using CIS.Core.Security;
using CIS.Infrastructure.Security.ContextUser;
using CIS.InternalServices.NotificationService.Clients;
using CIS.InternalServices.NotificationService.Clients.Interfaces;
using CIS.InternalServices.NotificationService.Contracts.Common;
using CIS.InternalServices.NotificationService.Contracts.Result;
using CIS.InternalServices.NotificationService.Contracts.Sms;
using Console_NotificationService;
using Microsoft.Extensions.DependencyInjection;

Console.WriteLine("run!");

var serviceProvider = new ServiceCollection()
    .AddSingleton<ICisEnvironmentConfiguration>(new CisEnvironmentConfiguration
    {
        EnvironmentName = "uat",
        DefaultApplicationKey = "console",
        ServiceDiscoveryUrl = "https://localhost:5005",
        InternalServicesLogin = "a",
        InternalServicePassword = "a"
    })
    .AddLogging()
    .AddNotificationClient()
    .AddScoped<ICurrentUserAccessor, CisCurrentContextUserAccessor>()
    .AddHttpContextAccessor()
    .BuildServiceProvider();

var client = serviceProvider.GetRequiredService<INotificationClient>();

var token = CancellationToken.None;
var text = "Text";
var type = "Test type";
var priority = 5;

var phone = new Phone
{
    CountryCode = "Code",
    NationalNumber = "Phone"
};

var smsSendRequest = new SmsSendRequest
{
    Phone = phone,
    Type = type,
    ProcessingPriority = priority,
    Text = text,
};

var smsSendResponse =  await client.SendSms(smsSendRequest, token);
Console.WriteLine($"Sms send response: {smsSendResponse.NotificationId}");

var smsFromTemplateSendRequest = new SmsFromTemplateSendRequest
{
    Phone = phone,
    Type = type,
    ProcessingPriority = priority,
    Placeholders = new List<StringKeyValuePair>()
    {
        new () { Key = "a", Value = "b" }
    },
};

var smsFromTemplateSendResponse = await client.SendSmsFromTemplate(smsFromTemplateSendRequest, token);
Console.WriteLine($"Sms from template send response: {smsFromTemplateSendResponse.NotificationId}");

var resultRequest = new ResultGetRequest { NotificationId = smsFromTemplateSendResponse.NotificationId };

var resultResponse = await client.GetResult(resultRequest, CancellationToken.None);
Console.WriteLine($"Result response: {resultResponse.NotificationId}");

Console.WriteLine("Press any key to exit...");
Console.ReadKey();
