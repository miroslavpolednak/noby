using CIS.Core.Attributes;

namespace DomainServices.RealEstateValuationService.Api.Database.DocumentDataEntities.Mappers;

#pragma warning disable CA1822 // Mark members as static
[TransientService, SelfService]
internal sealed class RealEstateValuationDataMapper
{
    public void MapToData(Contracts.UpdateRealEstateValuationDetailRequest request, RealEstateValudationData? data)
    {
        data ??= new RealEstateValudationData();

        data.LoanPurposes = request.LoanPurposeDetails?.LoanPurposes is null ? null : request.LoanPurposeDetails.LoanPurposes.ToList();
        data.HouseAndFlat = null;
        data.Parcel = null;
        
        data.LocalSurveyDetails = MapLocalSurveyDetails(request.LocalSurveyDetails);

        data.OnlinePreorderDetails = MapPreorderDetails(request.OnlinePreorderDetails);
        
        switch (request.SpecificDetailCase)
        {
            case Contracts.UpdateRealEstateValuationDetailRequest.SpecificDetailOneofCase.HouseAndFlatDetails:
                data.HouseAndFlat = new()
                {
                    PoorCondition = request.HouseAndFlatDetails.PoorCondition,
                    OwnershipRestricted = request.HouseAndFlatDetails.OwnershipRestricted
                };

                if (request.HouseAndFlatDetails.FlatOnlyDetails is not null)
                {
                    data.HouseAndFlat.FlatOnlyDetails = new()
                    {
                        SpecialPlacement = request.HouseAndFlatDetails.FlatOnlyDetails.SpecialPlacement,
                        Basement = request.HouseAndFlatDetails.FlatOnlyDetails.Basement
                    };
                }

                if (request.HouseAndFlatDetails.FinishedHouseAndFlatDetails is not null)
                {
                    data.HouseAndFlat.FinishedHouseAndFlatDetails = new()
                    {
                        LeaseApplicable = request.HouseAndFlatDetails.FinishedHouseAndFlatDetails.LeaseApplicable,
                        Leased = request.HouseAndFlatDetails.FinishedHouseAndFlatDetails.Leased
                    };
                }
                break;

            case Contracts.UpdateRealEstateValuationDetailRequest.SpecificDetailOneofCase.ParcelDetails:
                data.Parcel = new()
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

    public Contracts.RealEstateValuationDetail MapFromDataToSingle(RealEstateValudationData? data)
    {
        var result = new Contracts.RealEstateValuationDetail();
        MapFromDataToSingle(data, result);
        return result;
    }
    
    public void MapFromDataToSingle(RealEstateValudationData? data, Contracts.RealEstateValuationDetail realEstateValuation)
    {
        if (data?.LocalSurveyDetails is not null)
        {
            realEstateValuation.LocalSurveyDetails = new()
            {
                RealEstateValuationLocalSurveyFunctionCode = data.LocalSurveyDetails.RealEstateValuationLocalSurveyFunctionCode,
                Email = data.LocalSurveyDetails.Email,
                FirstName = data.LocalSurveyDetails.FirstName,
                LastName = data.LocalSurveyDetails.LastName,
                PhoneNumber = data.LocalSurveyDetails.PhoneNumber,
                PhoneIDC = data.LocalSurveyDetails.PhoneIDC
            };
        }

        if (data?.OnlinePreorderDetails is not null)
        {
            realEstateValuation.OnlinePreorderDetails = new()
            {
                BuildingAgeCode = data.OnlinePreorderDetails.BuildingAgeCode,
                BuildingMaterialStructureCode = data.OnlinePreorderDetails.BuildingMaterialStructureCode,
                BuildingTechnicalStateCode = data.OnlinePreorderDetails.BuildingTechnicalStateCode,
                FlatArea = data.OnlinePreorderDetails.FlatArea,
                FlatSchemaCode = data.OnlinePreorderDetails.FlatSchemaCode
            };
        }

        if (data?.LoanPurposes is not null)
        {
            realEstateValuation.LoanPurposeDetails ??= new Contracts.LoanPurposeDetailsObject();
            realEstateValuation.LoanPurposeDetails.LoanPurposes.AddRange(data.LoanPurposes);
        }

        if (data?.Documents is not null)
        {
            realEstateValuation.Documents.AddRange(data.Documents.Select(t => new Contracts.RealEstateValuationDocument
            {
                DocumentInfoPrice = t.DocumentInfoPrice,
                DocumentRecommendationForClient = t.DocumentRecommendationForClient
            }));
        }

        if (data?.HouseAndFlat is not null)
        {
            realEstateValuation.HouseAndFlatDetails = new()
            {
                PoorCondition = data.HouseAndFlat.PoorCondition,
                OwnershipRestricted = data.HouseAndFlat.OwnershipRestricted,
                FlatOnlyDetails = data.HouseAndFlat.FlatOnlyDetails is null ? null : new()
                {
                    SpecialPlacement = data.HouseAndFlat.FlatOnlyDetails.SpecialPlacement,
                    Basement = data.HouseAndFlat.FlatOnlyDetails.Basement
                },
                FinishedHouseAndFlatDetails = data.HouseAndFlat.FinishedHouseAndFlatDetails is null ? null : new()
                {
                    LeaseApplicable = data.HouseAndFlat.FinishedHouseAndFlatDetails.LeaseApplicable,
                    Leased = data.HouseAndFlat.FinishedHouseAndFlatDetails.Leased
                }
            };
        }
    }

    public RealEstateValudationData.OnlinePreorderData? MapPreorderDetails(Contracts.OnlinePreorderData? onlinePreorderDetails)
    {
        return onlinePreorderDetails is null ? null : new()
        {
            BuildingAgeCode = onlinePreorderDetails.BuildingAgeCode,
            BuildingMaterialStructureCode = onlinePreorderDetails.BuildingMaterialStructureCode,
            BuildingTechnicalStateCode = onlinePreorderDetails.BuildingTechnicalStateCode,
            FlatArea = onlinePreorderDetails.FlatArea,
            FlatSchemaCode = onlinePreorderDetails.FlatSchemaCode
        };
    }

    public RealEstateValudationData.LocalSurveyData? MapLocalSurveyDetails(Contracts.LocalSurveyData? localSurveyDetails)
    {
        return localSurveyDetails is null ? null : new()
        {
            RealEstateValuationLocalSurveyFunctionCode = localSurveyDetails.RealEstateValuationLocalSurveyFunctionCode,
            Email = localSurveyDetails.Email,
            FirstName = localSurveyDetails.FirstName,
            LastName = localSurveyDetails.LastName,
            PhoneNumber = localSurveyDetails.PhoneNumber,
            PhoneIDC = localSurveyDetails.PhoneIDC
        };
    }
}
