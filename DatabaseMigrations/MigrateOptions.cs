using CommandLine;

namespace DatabaseMigrations;

[Verb("migrate")]
internal sealed class MigrateOptions 
    : DatabaseMigrationsSupport.IMigrateOptions
{
    [Option('c', "connectionstring", Required = true, HelpText = "Connection string to database.")]
    public string? ConnectionString { get; set; }

    [Option('f', "folder", Required = true, HelpText = "Folder containing sql scripts.")]
    public string? ScriptFolder { get; set; }

    [Option('a', "codeassembly", Required = false, HelpText = "Folder containing C# scripts.")]
    public string? CodeScriptAssembly { get; set; }

    [Option('e', "checkOnly", Required = false, Default = false, HelpText = "Only performs check that the migrations are available")]
    public bool? MigrationExistsCheckOnly { get; set; }

    [Option('l', "logFile", Required = false, HelpText = "Path to log file")]
    public string? LogFile { get; set; }

    [Option('t', "notransaction", Required = false, HelpText = "Sets DbUp transaction behavior: true = WithoutTransaction()")]
    public bool? NoTransaction { get; set; }
}
