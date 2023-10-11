using CommandLine;
using DatabaseMigrations;
using DbUp;
using Spectre.Console;

return (int)Parser.Default
                  .ParseArguments<MigrateOptions>(args)
                  .MapResult(RunDbUp, _ => ExitCode.UnknownError);

static ExitCode RunDbUp(MigrateOptions opts)
{
    ArgumentException.ThrowIfNullOrEmpty(opts.ScriptFolder);

    var folder = opts.ScriptFolder;
    if (opts.ScriptFolder.EndsWith('\\') || opts.ScriptFolder.EndsWith('/'))
        folder = opts.ScriptFolder[..^1];

    // check folder
    if (!Directory.Exists(folder))
    {
        AnsiConsole.MarkupLine($"[bold red]ERROR:[/] Folder [dim]'{folder}'[/] does not exist.");
        return ExitCode.DirectoryNotExist;
    }

    var upgradeEngine = DeployChanges.To.SqlDatabase(opts.ConnectionString)
                                     .JournalToSqlTable("dbo", "MigrationHistory")
                                     .WithScriptsFromFileSystem(folder)
                                     .WithTransaction()
                                     .LogToConsole()
                                     .Build();

    if (opts.MigrationExistsCheckOnly ?? false)
        return upgradeEngine.IsUpgradeRequired() ? ExitCode.Success : ExitCode.NoMigrationAvailable;

    var result = upgradeEngine.PerformUpgrade();

    if (!result.Successful)
    {
        AnsiConsole.MarkupLine("[bold red]ERROR:[/] " + result.Error.Message);
        return ExitCode.MigrationFailed;
    }

    AnsiConsole.MarkupLine("[bold green]Success![/]");
    return ExitCode.Success;
}
