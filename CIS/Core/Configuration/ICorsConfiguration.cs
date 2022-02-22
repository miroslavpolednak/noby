namespace CIS.Core.Configuration;

public interface ICorsConfiguration
{
    bool EnableCors { get; set; }
    
    string[]? AllowedOrigins { get; set; }
}