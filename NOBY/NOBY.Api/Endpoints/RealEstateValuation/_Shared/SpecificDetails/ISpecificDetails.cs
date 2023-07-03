namespace NOBY.Api.Endpoints.RealEstateValuation.Shared.SpecificDetails;

/// <summary>
/// Objekty SpecificDetails jsou řízeny business logikou <a href="https://wiki.kb.cz/pages/viewpage.action?pageId=644560135">Ocenění nemovitosti - varianty nemovitostí</a>.<br />
/// Objekt HouseAndFlatDetails bude použit v případě, že jde o variantu nemovitosti HF.<br />
/// Objekt ParcelDetails bude použit v případě, že jde o variantu nemovitosti P.<br />
/// Pokud jde o variantu nemovitosti O, nebude použit ani jeden z objektů SpecificDetails.
/// </summary>
public interface ISpecificDetails
{
}