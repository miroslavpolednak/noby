using CIS.Core.Attributes;

namespace DomainServices.RealEstateValuationService.Api.Database.DocumentDataEntities.Mappers;

#pragma warning disable CA1822 // Mark members as static
[TransientService, SelfService]
internal sealed class RealEstateValudationDataMapper
{
    public void MapToData(Contracts.UpdateRealEstateValuationDetailRequest request, RealEstateValudationData? data)
    {
        data ??= new RealEstateValudationData();

        data.LoanPurposeDetails = request.LoanPurposeDetails?.LoanPurposes is null ? null : request.LoanPurposeDetails.LoanPurposes.ToList();
        data.HouseAndFlatDetails = null;
        data.ParcelDetails = null;

        switch (request.SpecificDetailCase)
        {
            case Contracts.UpdateRealEstateValuationDetailRequest.SpecificDetailOneofCase.HouseAndFlatDetails:
                data.HouseAndFlatDetails = new()
                {
                    PoorCondition = request.HouseAndFlatDetails.PoorCondition,
                    OwnershipRestricted = request.HouseAndFlatDetails.OwnershipRestricted,
                    FlatOnlyDetails = new()
                    {
                        SpecialPlacement = request.HouseAndFlatDetails.FlatOnlyDetails.SpecialPlacement,
                        Basement = request.HouseAndFlatDetails.FlatOnlyDetails.Basement
                    },
                    FinishedHouseAndFlatDetails = new()
                    {
                        LeaseApplicable = request.HouseAndFlatDetails.FinishedHouseAndFlatDetails.LeaseApplicable,
                        Leased = request.HouseAndFlatDetails.FinishedHouseAndFlatDetails.Leased
                    }
                };
                break;

            case Contracts.UpdateRealEstateValuationDetailRequest.SpecificDetailOneofCase.ParcelDetails:
                data.ParcelDetails = new()
                {
                    ParcelNumbers = request.ParcelDetails.ParcelNumbers.Select(t => new RealEstateValudationData.SpecificDetailParcelNumber
                    {
                        Prefix = t.Prefix,
                        Number = t.Number
                    }).ToList()
                };
                break;
        }
    }

    public void MapFromDataToSingle(RealEstateValudationData? data, Contracts.RealEstateValuationDetail realEstateValuation)
    {
        if (data?.LoanPurposeDetails is not null)
        {
            realEstateValuation.LoanPurposeDetails ??= new Contracts.LoanPurposeDetailsObject();
            realEstateValuation.LoanPurposeDetails.LoanPurposes.AddRange(data.LoanPurposeDetails);
        }

        if (data?.Documents is not null)
        {
            realEstateValuation.Documents.AddRange(data.Documents.Select(t => new Contracts.RealEstateValuationDocument
            {
                DocumentInfoPrice = t.DocumentInfoPrice,
                DocumentRecommendationForClient = t.DocumentRecommendationForClient
            }));
        }

        if (data?.HouseAndFlatDetails is not null)
        {
            realEstateValuation.HouseAndFlatDetails = new()
            {
                PoorCondition = data.HouseAndFlatDetails.PoorCondition,
                OwnershipRestricted = data.HouseAndFlatDetails.OwnershipRestricted,
                FlatOnlyDetails = new()
                {
                    SpecialPlacement = data.HouseAndFlatDetails.FlatOnlyDetails?.SpecialPlacement ?? false,
                    Basement = data.HouseAndFlatDetails.FlatOnlyDetails?.Basement ?? false
                },
                FinishedHouseAndFlatDetails = new()
                {
                    LeaseApplicable = data.HouseAndFlatDetails.FinishedHouseAndFlatDetails?.LeaseApplicable ?? false,
                    Leased = data.HouseAndFlatDetails.FinishedHouseAndFlatDetails?.Leased ?? false
                }
            };
        }
    }
}
