namespace DatabaseMigrationsSupport;

public interface IMigrateOptions
{
    string? CodeScriptAssembly { get; }
    string? ConnectionString { get; }
    string? LogFile { get; }
    bool? MigrationExistsCheckOnly { get; }
    string? ScriptFolder { get; }
}