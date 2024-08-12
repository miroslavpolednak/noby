namespace NOBY.ApiContracts;

public partial class RealEstateValuationSharedSpecificDetailsOneOf
{
    public static RealEstateValuationSharedSpecificDetailsOneOf? Create(RealEstateValuationSharedSpecificDetailsHouseAndFlat model)
    {
        return new()
        {
            Discriminator = nameof(RealEstateValuationSharedSpecificDetailsOneOf.HouseAndFlat),
            HouseAndFlat = model
        };
    }

    public static RealEstateValuationSharedSpecificDetailsOneOf? Create(RealEstateValuationSharedSpecificDetailsParcel model)
    {
        return new()
        {
            Discriminator = nameof(RealEstateValuationSharedSpecificDetailsOneOf.Parcel),
            Parcel = model
        };
    }
}
