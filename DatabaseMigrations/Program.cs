using CommandLine;
using DatabaseMigrations;
using DatabaseMigrations.ScriptProviders;
using DbUp;
using Spectre.Console;
using System.Reflection;

return (int)Parser.Default
                  .ParseArguments<MigrateOptions>(args)
                  .MapResult(options =>
                  {
                      try
                      {
                          return RunDbUp(options);
                      }
                      catch
                      {
                          return ExitCode.UnknownError;
                      }
                  }, _ => ExitCode.UnknownError);

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

    var upgradeEngineBuilder = DeployChanges.To.SqlDatabase(opts.ConnectionString)
                                     .JournalToSqlTable("dbo", "MigrationHistory")
                                     .WithTransaction()
                                     .LogToConsole()
                                     .WithScriptsFromFileSystem(folder);

    // add C# scripts to migrations
    if (!string.IsNullOrEmpty(opts.CodeScriptAssembly))
    {
        if (!File.Exists(opts.CodeScriptAssembly))
        {
            AnsiConsole.MarkupLine($"[bold red]ERROR:[/] Code scripts assembly [dim]'{opts.CodeScriptAssembly}'[/] does not exist.");
            return ExitCode.CodeAssemblyNotExist;
        }

        var codeAssembly = Assembly.LoadFrom(opts.CodeScriptAssembly);
        upgradeEngineBuilder = upgradeEngineBuilder
            .WithScripts(new ScriptFromScriptClassesScriptProvider(codeAssembly))
            .LogScriptOutput();
    }
    
    var upgradeEngine = upgradeEngineBuilder.Build();

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
