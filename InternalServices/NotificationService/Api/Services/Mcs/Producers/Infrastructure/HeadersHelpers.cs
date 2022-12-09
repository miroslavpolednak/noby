using System.Text;
using Confluent.Kafka;

namespace CIS.InternalServices.NotificationService.Api.Services.Mcs.Producers.Infrastructure;

public static class HeadersHelpers
{
    public static Headers CreateHeaders(Dictionary<string, string> keyValues)
    {
        var headers = new Headers();
        
        foreach (var keyValue in keyValues)
        {
            var bytes = Encoding.UTF8.GetBytes(keyValue.Value);
            headers.Add(keyValue.Key, bytes);
        }

        return headers;
    }
}