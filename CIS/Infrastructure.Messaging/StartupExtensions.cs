using Microsoft.AspNetCore.Builder;

namespace CIS.Infrastructure.Messaging;

public static class MessagingStartupExtensions
{
    public static ICisMessagingBuilder AddCisMessaging(this WebApplicationBuilder builder)
    {
        return new CisMessagingBuilder(builder);
    }
}
