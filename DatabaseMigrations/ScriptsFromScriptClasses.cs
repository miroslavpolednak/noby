using DbUp.Engine.Transactions;
using DbUp.Engine;
using System.Reflection;

namespace DatabaseMigrations.ScriptProviders;

internal sealed class ScriptFromScriptClassesScriptProvider
    : IScriptProvider
{
    private readonly Assembly _assembly;

    public ScriptFromScriptClassesScriptProvider(Assembly assembly)
    {
        _assembly = assembly;
    }

    public IEnumerable<SqlScript> GetScripts(IConnectionManager connectionManager)
    {
        var script = typeof(IScript);
        return connectionManager.ExecuteCommandsWithManagedConnection(dbCommandFactory => _assembly
            .GetTypes()
            .Where(type => script.IsAssignableFrom(type) && type.IsClass)
            .Select(s =>
            {
                var scriptNameAttribute = s.GetCustomAttribute<DatabaseMigrationsSupport.DbUpScriptNameAttribute>(false);
                var scriptName = scriptNameAttribute != null
                    ? scriptNameAttribute.ScriptName + ".cs"
                    : s.FullName + ".cs";

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                return (SqlScript)new LazySqlScript(scriptName, () => ((IScript)Activator.CreateInstance(s)).ProvideScript(dbCommandFactory));
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            })
            .ToList());
    }
}

