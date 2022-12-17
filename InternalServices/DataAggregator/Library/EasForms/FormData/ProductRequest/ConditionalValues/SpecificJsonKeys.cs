namespace CIS.InternalServices.DataAggregator.EasForms.FormData.ProductRequest.ConditionalValues;

internal class SpecificJsonKeys
{
    private readonly ProductTypeKind _productTypeKind;

    /// <summary>
    /// Mapa atributů, které jsou závislé na produktovém typu
    /// </summary>
    private static readonly Dictionary<JsonKey, ProductTypeKind> _jsonKeysMap = new()
    {
        {JsonKey.UvDruh, ProductTypeKind.KBMortgage | ProductTypeKind.KBMortgageWithoutRealEstate },
        {JsonKey.PojisteniNemSuma, ProductTypeKind.KBMortgage | ProductTypeKind.KBAmericanMortgage },
        {JsonKey.DeveloperId, ProductTypeKind.KBMortgage },
        {JsonKey.DeveloperProjektId, ProductTypeKind.KBMortgage },
        {JsonKey.DeveloperPopis, ProductTypeKind.KBMortgage },
        {JsonKey.SeznamUcelu, ProductTypeKind.KBMortgage | ProductTypeKind.KBAmericanMortgage },
        {JsonKey.SeznamObjektu, ProductTypeKind.KBMortgage },
        {JsonKey.FinKrytiVlastniZdroje, ProductTypeKind.KBMortgage | ProductTypeKind.KBMortgageWithoutRealEstate },
        {JsonKey.FinKrytiCiziZdroje, ProductTypeKind.KBMortgage | ProductTypeKind.KBMortgageWithoutRealEstate },
        {JsonKey.FinKrytiCelkem, ProductTypeKind.KBMortgage | ProductTypeKind.KBMortgageWithoutRealEstate },
    };

    private SpecificJsonKeys(ProductTypeKind productTypeKind)
    {
        _productTypeKind = productTypeKind;
    }

    public static SpecificJsonKeys Create(int productTypeId, int loanKindId)
    {
        var productTypeKind = productTypeId switch
        {
            20001 when loanKindId == 2000 => ProductTypeKind.KBMortgage,
            20001 when loanKindId == 2001 => ProductTypeKind.KBMortgageWithoutRealEstate,
            20010 when loanKindId == 2000 => ProductTypeKind.KBAmericanMortgage,
            _ => ProductTypeKind.Unknown
        };

        return new SpecificJsonKeys(productTypeKind);
    }

    /// <summary>
    /// Pokud má být atribut [key] obsažen ve výsledném JSONu, vrátí hodnotu [value]. V opačném případě vrátí [null]. 
    /// </summary>
    public TValue? GetValueOrDefault<TValue>(JsonKey jsonKey, TValue? value)
    {
        if (_productTypeKind == ProductTypeKind.Unknown || !_jsonKeysMap.ContainsKey(jsonKey))
            return value;

        return _jsonKeysMap[jsonKey].HasFlag(_productTypeKind) ? value : default;
    }

    public TValue? GetValueOrNull<TValue>(JsonKey jsonKey, TValue value) where TValue : struct =>
        GetValueOrDefault(jsonKey, (TValue?)value);
}