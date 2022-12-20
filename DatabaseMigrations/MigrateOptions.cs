using CommandLine;

namespace DatabaseMigrations;

[Verb("migrate")]
internal class MigrateOptions
{
    [Option('c', "connectionstring", Required = true, HelpText = "Connection string to database.")]
    public string? ConnecitonString { get; set; }

    [Option('f', "folder", Required = true, HelpText = "Folder containing sql scripts.")]
    public string? ScriptFolder { get; set; }
}
