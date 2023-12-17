using System.Globalization;
using CommandLine;
using DatabaseMigrations;
using DatabaseMigrations.ScriptProviders;
using DbUp;
using System.Reflection;
using Serilog;

return (int)Parser.Default
                  .ParseArguments<MigrateOptions>(args)
                  .MapResult(options =>
                  {
                      try
                      {
                          return RunDbUp(options);
                      }
                      catch(Exception ex)
                      {
                          Log.Error(ex, "Unhandled exception");
                          return ExitCode.UnknownError;
                      }
                  }, _ => ExitCode.UnknownError);

static ExitCode RunDbUp(MigrateOptions opts)
{
    ArgumentException.ThrowIfNullOrEmpty(opts.ScriptFolder);

    if (!string.IsNullOrWhiteSpace(opts.LogFile))
    {
        Log.Logger = new LoggerConfiguration().WriteTo.File(opts.LogFile, rollingInterval: RollingInterval.Infinite, formatProvider: CultureInfo.InvariantCulture)
                                              .WriteTo.Console(formatProvider: CultureInfo.InvariantCulture)
                                              .CreateLogger();
    }

    Log.Information("Starting");

    var folder = opts.ScriptFolder.TrimEnd('\\', '/');

    // check folder
    if (!Directory.Exists(folder))
    {
        Log.Error($"Folder '{folder}' does not exist.");

        return ExitCode.DirectoryNotExist;
    }

    var upgradeEngineBuilder = DeployChanges.To.SqlDatabase(opts.ConnectionString)
                                            .JournalToSqlTable("dbo", "MigrationHistory")
                                            .WithTransaction()
                                            .LogToAutodetectedLog()
                                            .LogScriptOutput()
                                            .WithScriptsFromFileSystem(folder);

    // add C# scripts to migrations
    if (!string.IsNullOrEmpty(opts.CodeScriptAssembly))
    {
        if (!File.Exists(opts.CodeScriptAssembly))
        {
            Log.Error($"Code scripts assembly '{opts.CodeScriptAssembly}' does not exist.");

            return ExitCode.CodeAssemblyNotExist;
        }

        var codeAssembly = Assembly.LoadFrom(opts.CodeScriptAssembly);
        upgradeEngineBuilder = upgradeEngineBuilder.WithScripts(new ScriptFromScriptClassesScriptProvider(codeAssembly));
    }
    
    var upgradeEngine = upgradeEngineBuilder.Build();

    if (upgradeEngine.IsUpgradeRequired())
    {
        Log.Information("No migration is available");

        return ExitCode.NoMigrationAvailable;
    }

    var result = upgradeEngine.PerformUpgrade();

    if (!result.Successful)
    {
        Log.Error($"Migration failed with message: {result.Error.Message}");

        return ExitCode.MigrationFailed;
    }

    Log.Information("Success");

    return ExitCode.Success;
}
