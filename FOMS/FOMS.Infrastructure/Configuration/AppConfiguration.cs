namespace FOMS.Infrastructure.Configuration;

public class AppConfiguration
{
    /// <summary>
    /// What kind of authentication provider to use - referes to DefaultScheme from .AddAuthentication().
    /// Possible values: FomsMockAuthentication
    /// </summary>
    public string AuthenticationScheme { get; set; }

    /// <summary>
    /// When set to false, Swagger middleware is not added to pipeline.
    /// </summary>
    public bool EnableSwaggerUI { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public SavingsConfiguration BuildingSavings { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
