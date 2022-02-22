namespace FOMS.Infrastructure.Configuration;

public class AppConfiguration
{
    /// <summary>
    /// What kind of authentication provider to use - referes to DefaultScheme from .AddAuthentication().
    /// Possible values: FomsMockAuthentication
    /// </summary>
    public string AuthenticationScheme { get; set; } = "";

    /// <summary>
    /// When set to false, Swagger middleware is not added to pipeline.
    /// </summary>
    public bool EnableSwaggerUi { get; set; }
}
