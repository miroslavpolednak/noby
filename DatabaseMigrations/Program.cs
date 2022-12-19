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

Console.ReadKey();

int runMigration(MigrateOptions o)
{
    // check folder
    if (!Directory.Exists(o.ScriptFolder))
    {
        AnsiConsole.MarkupLine($"[bold red]ERROR:[/] Folder [dim]'{o.ScriptFolder}'[/] does not exist.");
        return -3;
    }

    var upgradeEngine = DeployChanges.To
        .SqlDatabase(o.ConnecitonString)
        .WithScriptsFromFileSystem(o.ScriptFolder)
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
