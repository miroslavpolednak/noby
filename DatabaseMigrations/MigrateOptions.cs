using CommandLine;

namespace DatabaseMigrations;

[Verb("migrate")]
internal sealed class MigrateOptions
{
    [Option('c', "connectionstring", Required = true, HelpText = "Connection string to database.")]
    public string? ConnectionString { get; set; }

    [Option('f', "folder", Required = true, HelpText = "Folder containing sql scripts.")]
    public string? ScriptFolder { get; set; }

    [Option('e', "checkOnly", Required = false, Default = false, HelpText = "Only performs check that the migrations are available")]
    public bool? MigrationExistsCheckOnly { get; set; }
}
