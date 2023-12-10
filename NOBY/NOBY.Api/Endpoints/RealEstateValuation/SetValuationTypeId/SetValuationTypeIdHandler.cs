using DomainServices.RealEstateValuationService.Clients;
using SharedTypes.Enums;

namespace NOBY.Api.Endpoints.RealEstateValuation.SetValuationTypeId;

internal sealed class SetValuationTypeIdHandler
    : IRequestHandler<SetValuationTypeIdRequest>
{
    public async Task Handle(SetValuationTypeIdRequest request, CancellationToken cancellationToken)
    {
        var instance = await _realEstateValuationService.ValidateRealEstateValuationId(request.RealEstateValuationId, true, cancellationToken);

        if ((instance.ValuationStateId == (int)RealEstateValuationStates.DoplneniDokumentu && request.ValuationTypeId != SetValuationTypeIdRequestValuationTypes.Standard)
            || instance.OrderId.HasValue
            || (instance.PossibleValuationTypeId is not null && !instance.PossibleValuationTypeId.Contains((int)request.ValuationTypeId))
            || (request.ValuationTypeId == SetValuationTypeIdRequestValuationTypes.Dts && (instance.PossibleValuationTypeId?.Contains(1) ?? false))
        )
        {
            throw new NobyValidationException(90032);
        }

        await _realEstateValuationService.UpdateValuationTypeByRealEstateValuation(request.RealEstateValuationId, (int)request.ValuationTypeId, cancellationToken);
    }

    private readonly IRealEstateValuationServiceClient _realEstateValuationService;

    public SetValuationTypeIdHandler(IRealEstateValuationServiceClient realEstateValuationService)
    {
        _realEstateValuationService = realEstateValuationService;
    }
}
