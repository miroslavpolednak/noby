using CIS.Core.Security;
using DomainServices.CaseService.Clients.v1;
using DomainServices.SalesArrangementService.Clients;
using _SA = DomainServices.SalesArrangementService.Contracts;

namespace NOBY.Api.Endpoints.SalesArrangement.GetSalesArrangement;

internal sealed class GetSalesArrangementHandler(
    ICurrentUserAccessor _currentUser,
    ICaseServiceClient _caseService,
    ISalesArrangementServiceClient _salesArrangementService,
    Services.SalesArrangementAuthorization.ISalesArrangementAuthorizationService _salesArrangementAuthorization)
        : IRequestHandler<GetSalesArrangementRequest, SalesArrangementGetSalesArrangementResponse>
{
    public async Task<SalesArrangementGetSalesArrangementResponse> Handle(GetSalesArrangementRequest request, CancellationToken cancellationToken)
    {
        // instance SA
        var saInstance = await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken);
        var caseInstance = await _caseService.GetCaseDetail(saInstance.CaseId, cancellationToken);

        // perm check
        _salesArrangementAuthorization.ValidateSaAccessBySaType213And248(saInstance.SalesArrangementTypeId);
        if (caseInstance.CaseOwner.UserId != _currentUser.User!.Id && !_currentUser.HasPermission(UserPermissions.DASHBOARD_AccessAllCases))
        {
            throw new CisAuthorizationException("Case owner check failed");
        }

        var parameters = getParameters(saInstance);

        return new()
        {
            ProductTypeId = caseInstance.Data.ProductTypeId,
            SalesArrangementId = saInstance.SalesArrangementId,
            SalesArrangementTypeId = saInstance.SalesArrangementTypeId,
            LoanApplicationAssessmentId = saInstance.LoanApplicationAssessmentId,
            CreatedBy = saInstance.Created.UserName,
            CreatedTime = saInstance.Created.DateTime,
            OfferGuaranteeDateFrom = saInstance.OfferGuaranteeDateFrom,
            OfferGuaranteeDateTo = saInstance.OfferGuaranteeDateTo,
            Parameters = parameters,
            State = saInstance.State,
            OfferId = saInstance.OfferId,
            ProcessId = saInstance.ProcessId
        };
    }

    static SalesArrangementGetSalesArrangementParameters? getParameters(_SA.SalesArrangement saInstance)
        => saInstance.ParametersCase switch
        {
            _SA.SalesArrangement.ParametersOneofCase.Mortgage => new SalesArrangementGetSalesArrangementParameters
            {
                Mortgage = saInstance.Mortgage.ToApiResponse()
            },
            _SA.SalesArrangement.ParametersOneofCase.Drawing => new SalesArrangementGetSalesArrangementParameters
            {
                Drawing = saInstance.Drawing.ToApiResponse()
            },
            _SA.SalesArrangement.ParametersOneofCase.GeneralChange => new SalesArrangementGetSalesArrangementParameters
            {
                GeneralChange = saInstance.GeneralChange.ToApiResponse()
            },
            _SA.SalesArrangement.ParametersOneofCase.HUBN => new SalesArrangementGetSalesArrangementParameters
            {
                Hubn = saInstance.HUBN.ToApiResponse()
            },
            _SA.SalesArrangement.ParametersOneofCase.CustomerChange => new SalesArrangementGetSalesArrangementParameters
            {
                CustomerChange = saInstance.CustomerChange.ToApiResponse()
            },
            _SA.SalesArrangement.ParametersOneofCase.CustomerChange3602A => new SalesArrangementGetSalesArrangementParameters
            {
                CustomerChange3602 = saInstance.CustomerChange3602A.ToApiResponse()
            },
            _SA.SalesArrangement.ParametersOneofCase.CustomerChange3602B => new SalesArrangementGetSalesArrangementParameters
            {
                CustomerChange3602 = saInstance.CustomerChange3602B.ToApiResponse()
            },
            _SA.SalesArrangement.ParametersOneofCase.CustomerChange3602C => new SalesArrangementGetSalesArrangementParameters
            {
                CustomerChange3602 = saInstance.CustomerChange3602C.ToApiResponse()
            },
            //TODO tohle tu fakt ma byt?
            //_SA.SalesArrangement.ParametersOneofCase.Retention => saInstance.Retention,
            //_SA.SalesArrangement.ParametersOneofCase.Refixation => saInstance.Refixation,
            //_SA.SalesArrangement.ParametersOneofCase.ExtraPayment => saInstance.ExtraPayment,
            _SA.SalesArrangement.ParametersOneofCase.None => null,
            _ => throw new NotImplementedException($"getParameters for {saInstance.ParametersCase} not implemented")
        };
}