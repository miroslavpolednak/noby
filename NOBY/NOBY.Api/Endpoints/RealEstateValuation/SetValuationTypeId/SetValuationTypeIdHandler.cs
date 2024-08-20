using DomainServices.RealEstateValuationService.Clients;

namespace NOBY.Api.Endpoints.RealEstateValuation.SetValuationTypeId;

internal sealed class SetValuationTypeIdHandler(IRealEstateValuationServiceClient _realEstateValuationService)
    : IRequestHandler<RealEstateValuationSetValuationTypeIdRequest>
{
    public async Task Handle(RealEstateValuationSetValuationTypeIdRequest request, CancellationToken cancellationToken)
    {
        var instance = await _realEstateValuationService.ValidateRealEstateValuationId(request.RealEstateValuationId, true, cancellationToken);

        if (!_possibleValuationStateId.Contains(instance.ValuationStateId.GetValueOrDefault()))
        {
            throw new NobyValidationException(90032, "ValuationStateId check failed");
        }

        if (instance.ValuationStateId == (int)WorkflowTaskStates.DoplneniDokumentu && request.ValuationTypeId != EnumRealEstateValuationTypes.Standard)
        {
            throw new NobyValidationException(90032, "ValuationTypeId for ValuationStateId=10 check failed");
        }

        if (!_possibleValuationTypeId.Contains((int)instance.ValuationTypeId))
        {
            throw new NobyValidationException(90032, "ValuationTypeId check failed");
        }

        if (instance.OrderId.GetValueOrDefault() != 0)
        {
            throw new NobyValidationException(90032, "OrderId check failed");
        }

        if (!(instance.PossibleValuationTypeId?.Contains((int)request.ValuationTypeId) ?? true))
        {
            throw new NobyValidationException(90032, "PossibleValuationTypeId check failed");
        }

        if (request.ValuationTypeId == EnumRealEstateValuationTypes.Dts && (instance.PossibleValuationTypeId?.Contains((int)SharedTypes.Enums.EnumRealEstateValuationTypes.Online) ?? false))
        {
            throw new NobyValidationException(90032, "PossibleValuationTypeId for DTS check failed");
        }

        if (instance.ValuationStateId == (int)WorkflowTaskStates.DoplneniDokumentu)
        {
            await _realEstateValuationService.UpdateStateByRealEstateValuation(request.RealEstateValuationId, WorkflowTaskStates.Rozpracovano, cancellationToken);
        }

        await _realEstateValuationService.UpdateValuationTypeByRealEstateValuation(request.RealEstateValuationId, (int)request.ValuationTypeId, cancellationToken);
    }

    private static readonly int[] _possibleValuationTypeId = [(int)SharedTypes.Enums.EnumRealEstateValuationTypes.Unknown, (int)SharedTypes.Enums.EnumRealEstateValuationTypes.Online];
    private static readonly int[] _possibleValuationStateId = [(int)WorkflowTaskStates.DoplneniDokumentu, (int)WorkflowTaskStates.Rozpracovano];
}
