using CIS.Core.Attributes;
using Microsoft.AspNetCore.Components;

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
        data.LocalSurveyDetails = null;
        data.OnlinePreorderDetails = null;

        if (request.LocalSurveyDetails is not null)
        {
            data.LocalSurveyDetails = new()
            {
                RealEstateValuationLocalSurveyFunctionCode = request.LocalSurveyDetails.RealEstateValuationLocalSurveyFunctionCode,
                Email = request.LocalSurveyDetails.Email,
                FirstName = request.LocalSurveyDetails.FirstName,
                LastName = request.LocalSurveyDetails.LastName,
                PhoneNumber = request.LocalSurveyDetails.PhoneNumber,
                PhoneIDC = request.LocalSurveyDetails.PhoneIDC
            };
        }

        if (request.OnlinePreorderDetails is not null)
        {
            data.OnlinePreorderDetails = new()
            {
                BuildingAgeCode = request.OnlinePreorderDetails.BuildingAgeCode,
                BuildingMaterialStructureCode = request.OnlinePreorderDetails.BuildingMaterialStructureCode,
                BuildingTechnicalStateCode = request.OnlinePreorderDetails.BuildingTechnicalStateCode,
                FlatArea = request.OnlinePreorderDetails.FlatArea,
                FlatSchemaCode = request.OnlinePreorderDetails.FlatSchemaCode
            };
        }
        
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
}
