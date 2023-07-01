using CIS.Foms.Enums;
using DomainServices.CaseService.Clients;
using DomainServices.RealEstateValuationService.Clients;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using NOBY.Dto.RealEstateValuation.RealEstateValuationDetailDto;
using __Contracts = DomainServices.RealEstateValuationService.Contracts;

namespace NOBY.Api.Endpoints.RealEstateValuation.UpdateRealEstateValuationDetail;

public class UpdateRealEstateValuationDetailHandler : IRequestHandler<UpdateRealEstateValuationDetailRequest>
{
    private readonly ICaseServiceClient _caseService;
    private readonly IRealEstateValuationServiceClient _realEstateValuationService;

    public UpdateRealEstateValuationDetailHandler(ICaseServiceClient caseService, IRealEstateValuationServiceClient realEstateValuationService)
    {
        _caseService = caseService;
        _realEstateValuationService = realEstateValuationService;
    }

    public async Task Handle(UpdateRealEstateValuationDetailRequest request, CancellationToken cancellationToken)
    {
        //var caseInstance = await _caseService.GetCaseDetail(request.CaseId, cancellationToken);
        //var valuationDetail = await _realEstateValuationService.GetRealEstateValuationDetail(request.RealEstateValuationId, cancellationToken);

        //if (valuationDetail.RealEstateValuationGeneralDetails.CaseId != request.CaseId)
        //    throw new NobyValidationException("The requested RealEstateValuation is not assigned to the requested Case");

        //if (valuationDetail.RealEstateValuationGeneralDetails.ValuationStateId != 7)
        //    throw new NobyValidationException("The valuation is not in progress");

        //if (caseInstance.State == (int)CaseStates.InProgress && request.LoanPurposeDetails is null)
        //    throw new NobyValidationException("The LoanPurposeDetails has to be null when the case is in progress");


        await Task.Delay(300);
    }

    private void CheckHouseAndFlatDetails(UpdateRealEstateValuationDetailRequest request)
    {
    }

    private void CheckParcelDetails(UpdateRealEstateValuationDetailRequest request)
    {
        if (request.RealEstateStateId.HasValue)
            throw new NobyValidationException("RealEstateStateId has to be null for the parcel variant");

        if (ValidateJson<ParcelDetails>(request.SpecificDetails))
            throw new NobyValidationException("SpecificDetails does not contain the parcel object");
    }

    private void CheckObjectDetails(UpdateRealEstateValuationDetailRequest request)
    {
        if (request.RealEstateStateId.HasValue)
            return;

        throw new NobyValidationException("RealEstateStateId is required");
    }

    private static bool ValidateJson<TSchema>(object? obj) where TSchema : class
    {
        if (obj is null)
            return false;

        var schema = new JSchemaGenerator().Generate(typeof(TSchema));
        var jObject = JObject.FromObject(obj);

        return jObject.IsValid(schema);
    }
}