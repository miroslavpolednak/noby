using DomainServices.CodebookService.Contracts.v1;

namespace DomainServices.CodebookService.Api.Endpoints;

internal partial class CodebookService
{
    public override async Task<GetACVAndBagmanRealEstateTypeResponse> GetACVAndBagmanRealEstateType(GetACVAndBagmanRealEstateTypeRequest request, ServerCallContext context)
    {
        return (await _db.GetFirstOrDefault<GetACVAndBagmanRealEstateTypeResponse>(new
        {
            request.RealEstateStateId,
            request.RealEstateSubtypeId,
            request.RealEstateTypeId
        }))
            ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.AcvRealEstateTypeNotFound);
    }

    public override Task<GenericCodebookResponse> RealEstatePurchaseTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _db.GetGenericItems();

    public override Task<GenericCodebookResponse> RealEstateStates(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _db.GetGenericItems();

    public override Task<RealEstateSubtypesResponse> RealEstateSubtypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _db.GetItems<RealEstateSubtypesResponse, RealEstateSubtypesResponse.Types.RealEstateSubtypesResponseItem>();

    public override Task<RealEstateTypesResponse> RealEstateTypes(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => _db.GetItems<RealEstateTypesResponse, RealEstateTypesResponse.Types.RealEstateTypesResponseItem>();

    public override Task<GenericCodebookResponse> RealEstateValuationFlatSchemas(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Task.FromResult(_acvEnumService.GetItems(ExternalServices.AcvEnumService.V1.Categories.ModelFlatType));

    public override Task<GenericCodebookResponse> RealEstateValuationBuildingMaterialStructures(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Task.FromResult(_acvEnumService.GetItems(ExternalServices.AcvEnumService.V1.Categories.ModelMaterialStructure));

    public override Task<GenericCodebookResponse> RealEstateValuationBuildingAges(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Task.FromResult(_acvEnumService.GetItems(ExternalServices.AcvEnumService.V1.Categories.ModelAge));

    public override Task<GenericCodebookResponse> RealEstateValuationBuildingTechnicalStates(Google.Protobuf.WellKnownTypes.Empty request, ServerCallContext context)
        => Task.FromResult(_acvEnumService.GetItems(ExternalServices.AcvEnumService.V1.Categories.ModelTechnicalState));
}
