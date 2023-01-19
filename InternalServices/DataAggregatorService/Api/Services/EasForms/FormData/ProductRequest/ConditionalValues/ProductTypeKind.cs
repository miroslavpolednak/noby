namespace CIS.InternalServices.DataAggregatorService.Api.Services.EasForms.FormData.ProductRequest.ConditionalValues;

[Flags]
internal enum ProductTypeKind
{
    // Pokud není uvedeno, plnění atributů celého objektu není produktově závislé
    Unknown = 0,

    // Hypoteční úvěr(standard) - Produkt: 20001 / Druh: 2000
    KBMortgage = 1,

    // Americká hypotéka - Produkt: 20010 / Druh: 2000
    KBAmericanMortgage = 2,

    // HÚ bez nemovitosti - Produkt: 20001 / Druh: 2001
    KBMortgageWithoutRealEstate = 4,
}