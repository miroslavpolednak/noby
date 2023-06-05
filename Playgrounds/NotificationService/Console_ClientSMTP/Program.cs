// See https://aka.ms/new-console-template for more information

using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

Console.WriteLine("run");

using var client = new SmtpClient();
await client.ConnectAsync("relay.mpss.cz", 25, SecureSocketOptions.None);

var message = new MimeMessage();
message.From.Add(MailboxAddress.Parse("notification-service@mpss.cz"));
message.To.Add(MailboxAddress.Parse("karel.nguyen-trong@mpss.cz"));
message.Cc.Add(MailboxAddress.Parse("petr.caisl@mpss.cz"));
message.Bcc.Add(MailboxAddress.Parse("filip.tuma@mpss.cz"));
message.ReplyTo.Add(MailboxAddress.Parse("karel.nguyen-trong@mpss.cz"));
message.Subject = "Test Email Subject";

var bodyBuilder = new BodyBuilder();
bodyBuilder.HtmlBody =  "<h1>Example HTML email with attachments</h1>";

var filenames = new[] { "image.png", "attachment1.txt", "attachment2.txt" };

foreach (var filename in filenames)
{
    var data = File.ReadAllBytes(Path.Combine("./", filename));
    bodyBuilder.Attachments.Add(filename, data);
}

message.Body = bodyBuilder.ToMessageBody();

await client.SendAsync(message);
await client.DisconnectAsync(true);

Console.WriteLine("end");