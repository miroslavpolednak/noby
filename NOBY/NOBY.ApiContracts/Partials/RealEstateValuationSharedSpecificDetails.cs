namespace NOBY.ApiContracts;

public partial class RealEstateValuationSharedSpecificDetails
{
    public static RealEstateValuationSharedSpecificDetails? Create(RealEstateValuationSharedSpecificDetailsHouseAndFlat? model)
    {
        return new RealEstateValuationSharedSpecificDetails
        {
            Discriminator = nameof(RealEstateValuationSharedSpecificDetails.HouseAndFlat),
            HouseAndFlat = model
        };
    }

    public static RealEstateValuationSharedSpecificDetails? Create(RealEstateValuationSharedSpecificDetailsParcel? model)
    {
        return new RealEstateValuationSharedSpecificDetails
        {
            Discriminator = nameof(RealEstateValuationSharedSpecificDetails.Parcel),
            Parcel = model
        };
    }
}
