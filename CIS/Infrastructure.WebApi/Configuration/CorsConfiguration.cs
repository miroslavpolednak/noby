namespace CIS.Infrastructure.WebApi.Configuration;

public class CorsConfiguration : Core.Configuration.ICorsConfiguration
{
    public const string AppsettingsConfigurationKey = "CisCors";
    
    public bool EnableCors { get; set; }

    public string[]? AllowedOrigins { get; set; }
}