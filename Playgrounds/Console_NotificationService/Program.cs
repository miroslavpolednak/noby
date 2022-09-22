// See https://aka.ms/new-console-template for more information

using CIS.InternalServices.NotificationService.Contracts;
using CIS.InternalServices.NotificationService.Contracts.Sms;
using CIS.InternalServices.NotificationService.Contracts.Sms.Dto;
using Grpc.Net.Client;
using ProtoBuf.Grpc.Client;

Console.WriteLine("run!");

using var channel = GrpcChannel.ForAddress(
    "https://localhost:5003",
    new GrpcChannelOptions
    {
        HttpHandler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
        }
    });
var client = channel.CreateGrpcService<INotificationService>();

var token = CancellationToken.None;
var text = "Text";
var type = "Type";
var priority = 5;

var phone = new Phone
{
    CountryCode = "Code",
    NationalPhoneNumber = "Phone"
};

var smsPushRequest = new SmsSendRequest
{
    Phone = phone,
    Type = type,
    ProcessingPriority = priority,
    Text = text,
};

var smsPushResponse = await client.SendSms(smsPushRequest, token);
Console.WriteLine($"Sms push response: {smsPushResponse.NotificationId}");

var smsFromTemplatePushRequest = new SmsFromTemplateSendRequest
{
    Phone = phone,
    Type = type,
    ProcessingPriority = priority,
    Placeholders = new List<StringKeyValuePair>()
    {
        new () { Key = "a", Value = "b" }
    },
};

var smsFromTemplatePushResponse = await client.SendSmsFromTemplate(smsFromTemplatePushRequest, token);
Console.WriteLine($"Sms from template push response: {smsFromTemplatePushResponse.NotificationId}");

Console.WriteLine("Press any key to exit...");
Console.ReadKey();
