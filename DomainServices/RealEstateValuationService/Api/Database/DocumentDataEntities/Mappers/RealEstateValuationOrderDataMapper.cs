using CIS.Core.Attributes;

namespace DomainServices.RealEstateValuationService.Api.Database.DocumentDataEntities.Mappers;

#pragma warning disable CA1822 // Mark members as static
[TransientService, SelfService]
internal sealed class RealEstateValuationOrderDataMapper
{
    public RealEstateValuationOrderData MapToData(Contracts.OrdersStandard? data)
    {
        return new RealEstateValuationOrderData()
        {
            RealEstateValuationOrderType = SharedTypes.Enums.RealEstateValuationOrderTypes.Standard,
            Standard = new()
            {
                RealEstateValuationLocalSurveyFunctionCode = data?.RealEstateValuationLocalSurveyFunctionCode,
                Email = data?.Email,
                FirstName = data?.FirstName,
                LastName = data?.LastName,
                PhoneNumber = data?.PhoneNumber,
                PhoneIDC = data?.PhoneIDC
            }
        };
    }

    public RealEstateValuationOrderData MapToData(Contracts.OrdersOnlinePreorder? data)
    {
        return new RealEstateValuationOrderData()
        {
            RealEstateValuationOrderType = SharedTypes.Enums.RealEstateValuationOrderTypes.OnlinePreorder,
            OnlinePreorder = new()
            {
                BuildingAgeCode = data?.BuildingAgeCode,
                BuildingMaterialStructureCode = data?.BuildingMaterialStructureCode,
                BuildingTechnicalStateCode = data?.BuildingTechnicalStateCode,
                FlatArea = data?.FlatArea,
                FlatSchemaCode = data?.FlatSchemaCode
            }
        };
    }
}
