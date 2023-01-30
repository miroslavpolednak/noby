using CommandLine;
using DatabaseMigrations;
using DbUp;
using Spectre.Console;

Parser.Default
    .ParseArguments<MigrateOptions>(args)
    .MapResult(
        (MigrateOptions o) => runMigration(o),
        e => -1
    );

int runMigration(MigrateOptions o)
{
    ArgumentNullException.ThrowIfNullOrEmpty(o.ScriptFolder);

    string folder = o.ScriptFolder;
    if (o.ScriptFolder.EndsWith('\\') || o.ScriptFolder.EndsWith('/'))
        folder = o.ScriptFolder[0..^1];

    // check folder
    if (!Directory.Exists(folder))
    {
        AnsiConsole.MarkupLine($"[bold red]ERROR:[/] Folder [dim]'{folder}'[/] does not exist.");
        return -3;
    }

    var upgradeEngine = DeployChanges.To
        .SqlDatabase(o.ConnecitonString)
        .WithScriptsFromFileSystem(folder)
        .LogToConsole()
        .Build();

    var result = upgradeEngine.PerformUpgrade();

    if (!result.Successful)
    {
        AnsiConsole.MarkupLine("[bold red]ERROR:[/] " + result.Error.Message);
        return -2;
    }

    AnsiConsole.MarkupLine("[bold green]Success![/]");
    return 0;
}
