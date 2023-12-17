namespace DatabaseMigrationsSupport;

public static class Settings
{
    public static IMigrateOptions Options { get; private set; } = null!;

    public static void SetOptions(IMigrateOptions options)
    {
        if (Options == null)
        {
            Options = options;
        }
    }
}
