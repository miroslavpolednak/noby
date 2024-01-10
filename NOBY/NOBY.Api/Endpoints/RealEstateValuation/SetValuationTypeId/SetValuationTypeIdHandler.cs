﻿using DomainServices.RealEstateValuationService.Clients;

namespace NOBY.Api.Endpoints.RealEstateValuation.SetValuationTypeId;

internal sealed class SetValuationTypeIdHandler
    : IRequestHandler<SetValuationTypeIdRequest>
{
    public async Task Handle(SetValuationTypeIdRequest request, CancellationToken cancellationToken)
    {
        var instance = await _realEstateValuationService.ValidateRealEstateValuationId(request.RealEstateValuationId, true, cancellationToken);

        if (!_possibleValuationStateId.Contains(instance.ValuationStateId.GetValueOrDefault()))
        {
            throw new NobyValidationException(90032, "ValuationStateId check failed");
        }

        if (instance.ValuationStateId == (int)RealEstateValuationStates.DoplneniDokumentu && request.ValuationTypeId != RealEstateValuationValuationTypes.Standard)
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

        if (request.ValuationTypeId == RealEstateValuationValuationTypes.Dts && (instance.PossibleValuationTypeId?.Contains((int)RealEstateValuationTypes.Online) ?? false))
        {
            throw new NobyValidationException(90032, "PossibleValuationTypeId for DTS check failed");
        }

        await _realEstateValuationService.UpdateValuationTypeByRealEstateValuation(request.RealEstateValuationId, (int)request.ValuationTypeId, cancellationToken);
    }

    private static int[] _possibleValuationTypeId = [(int)RealEstateValuationTypes.Unknown, (int)RealEstateValuationTypes.Online];
    private static int[] _possibleValuationStateId = [(int)RealEstateValuationStates.DoplneniDokumentu, (int)RealEstateValuationStates.Rozpracovano];

    private readonly IRealEstateValuationServiceClient _realEstateValuationService;

    public SetValuationTypeIdHandler(IRealEstateValuationServiceClient realEstateValuationService)
    {
        _realEstateValuationService = realEstateValuationService;
    }
}
