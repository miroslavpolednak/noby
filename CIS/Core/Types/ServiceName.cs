namespace CIS.Core.Types;

public record ServiceName
{
    public string Name { get; init; }

    public ServiceName(string name)
    {
        if (!name.All(t => Char.IsLetterOrDigit(t) || t == ':'))
            throw new Exceptions.CisArgumentException(101, $"'{name}' is not valid Service Name", "name");

        this.Name = name;
    }

    public ServiceName(WellKnownServices name) 
        : this(getServiceName(name))
    { }

    public static implicit operator string(ServiceName d) => d.Name;

    public override string ToString() => $"{Name}";

    private static string getServiceName(WellKnownServices name) => name switch
    {
        WellKnownServices.Redis => "CIS:GlobalCache:Redis",
        WellKnownServices.Storage => "CIS:Storage",
        _ => throw new NotImplementedException("Service name is unknown")
    };

    public enum WellKnownServices
    {
        Unknown = 0,
        Redis = 1,
        Storage = 2
    }
}
