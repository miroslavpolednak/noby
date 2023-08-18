// See https://aka.ms/new-console-template for more information

using Console_ClientSMTP;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MimeKit;

Console.WriteLine("Console_ClientSMTP started.");

// Configuration
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", false, true)
    .AddCommandLine(args)
    .Build();

// Services
var services = new ServiceCollection();
services
    .AddSingleton<IConfiguration>(configuration)
    .AddOptions<SmtpConfiguration>()
    .Bind(configuration.GetSection(nameof(SmtpConfiguration)));

var serviceProvider = services.BuildServiceProvider();

var options = serviceProvider.GetRequiredService<IOptions<SmtpConfiguration>>().Value;
using var client = new SmtpClient();

// Console.WriteLine("ServerCertificateValidationCallback = true");
// client.ServerCertificateValidationCallback = (s,c,h,e) => true;

var connectionDetail = $"{options.Host}:{options.Port} (SecureSocketOptions = {options.SecureSocketOptions})";
Console.WriteLine($"Connecting to: {connectionDetail}");

await client.ConnectAsync(options.Host, options.Port, options.SecureSocketOptions);

Console.WriteLine("Connected.");

var message = new MimeMessage();
message.From.Add(MailboxAddress.Parse("smtp-test@mpss.cz"));
message.To.Add(MailboxAddress.Parse("karel.nguyen-trong@mpss.cz"));
message.ReplyTo.Add(MailboxAddress.Parse("karel.nguyen-trong@mpss.cz"));

message.Subject = $"Test from SmtpServer {connectionDetail}";

var bodyBuilder = new BodyBuilder();
bodyBuilder.HtmlBody =  "<h1>Example HTML email with attachments</h1>";

var filenames = new[] { "image.png", "attachment1.txt", "attachment2.txt" };

foreach (var filename in filenames)
{
    var data = File.ReadAllBytes(Path.Combine("./", filename));
    bodyBuilder.Attachments.Add(filename, data);
}

message.Body = bodyBuilder.ToMessageBody();

Console.WriteLine("Sending message.");
await client.SendAsync(message);
Console.WriteLine("Sent.");

Console.WriteLine("Disconnecting.");
await client.DisconnectAsync(true);
Console.WriteLine("Disconnected.");