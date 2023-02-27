namespace NOBY.Api.SharedDto;

public class PaymentSymbols
{
    public string? VariableSymbol { get; set; }

    [Obsolete]
    public string? ConstantSymbol { get; set; }

    [Obsolete]
    public string? SpecificSymbol { get; set; }
}