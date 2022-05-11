using Refit;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace DomainServices.RiskIntegrationService.Api;

/// <summary>
/// Registrace API klienta
/// </summary>
public static class CustomRemoteServiceC4m
{
    public static IServiceCollection AddCustomRemoteServiceC4m(this IServiceCollection services, IConfiguration configuration)
    {
        string section = "ApiKbC4m:CreditWorthiness";
        string baseUrl = configuration.GetValue<string>($"{section}:BaseUrl");
        string userName = configuration.GetValue<string>($"{section}:UserName");
        string password = configuration.GetValue<string>($"{section}:Password");

        string authHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes(string.Format($"{userName}:{password}")));

        var jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        services.
            AddRefitClient<Mpss.Rip.Infrastructure.RemoteServices.IC4M.ICreditWorthinessServices>(new RefitSettings
            {
                AuthorizationHeaderValueGetter = () => Task.FromResult(authHeader),
                ContentSerializer = new SystemTextJsonContentSerializer(jsonSerializerOptions)
                //AuthorizationHeaderValueGetter = GetAuthorizationHeader
            })
            .ConfigureHttpClient(x =>
            {
                x.BaseAddress = new Uri(baseUrl);
                x.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //x.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authHeader);
            });

        return services;
    }
}
