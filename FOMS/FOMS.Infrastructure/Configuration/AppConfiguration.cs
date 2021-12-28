namespace FOMS.Infrastructure.Configuration;

public class AppConfiguration
{
    public string? MyTest { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public SavingsConfiguration BuildingSavings { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
}
