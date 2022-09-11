namespace ExternalServices.AddressWhisperer.Shared;

public sealed class FoundSuggestion
{
    public long AddressId { get; set; }

    public string Title { get; set; } = null!;
}
