namespace DatabaseMigrationsSupport;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class DbUpScriptNameAttribute
    : Attribute
{
    public string ScriptName { get; init; }

    public DbUpScriptNameAttribute(string scriptName)
    {
        ScriptName = scriptName;
    }
}
