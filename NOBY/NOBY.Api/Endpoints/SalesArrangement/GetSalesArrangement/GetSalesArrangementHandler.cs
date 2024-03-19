﻿using CIS.Core.Security;
using DomainServices.CaseService.Clients.v1;
using DomainServices.SalesArrangementService.Clients;
using _SA = DomainServices.SalesArrangementService.Contracts;

namespace NOBY.Api.Endpoints.SalesArrangement.GetSalesArrangement;

internal sealed class GetSalesArrangementHandler
    : IRequestHandler<GetSalesArrangementRequest, GetSalesArrangementResponse>
{
    public async Task<GetSalesArrangementResponse> Handle(GetSalesArrangementRequest request, CancellationToken cancellationToken)
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

        return new GetSalesArrangementResponse()
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
            TaskProcessId = saInstance.TaskProcessId
        };
    }

    static object? getParameters(_SA.SalesArrangement saInstance)
        => saInstance.ParametersCase switch
        {
            _SA.SalesArrangement.ParametersOneofCase.Mortgage => saInstance.Mortgage.ToApiResponse(),
            _SA.SalesArrangement.ParametersOneofCase.Drawing => saInstance.Drawing.ToApiResponse(),
            _SA.SalesArrangement.ParametersOneofCase.GeneralChange => saInstance.GeneralChange.ToApiResponse(),
            _SA.SalesArrangement.ParametersOneofCase.HUBN => saInstance.HUBN.ToApiResponse(),
            _SA.SalesArrangement.ParametersOneofCase.CustomerChange => saInstance.CustomerChange.ToApiResponse(),
            _SA.SalesArrangement.ParametersOneofCase.CustomerChange3602A => saInstance.CustomerChange3602A.ToApiResponse(),
            _SA.SalesArrangement.ParametersOneofCase.CustomerChange3602B => saInstance.CustomerChange3602B.ToApiResponse(),
            _SA.SalesArrangement.ParametersOneofCase.CustomerChange3602C => saInstance.CustomerChange3602C.ToApiResponse(),
            _SA.SalesArrangement.ParametersOneofCase.None => null,
            _ => throw new NotImplementedException($"getParameters for {saInstance.ParametersCase} not implemented")
        };

    private readonly Services.SalesArrangementAuthorization.ISalesArrangementAuthorizationService _salesArrangementAuthorization;
    private readonly ICurrentUserAccessor _currentUser;
    private readonly ICaseServiceClient _caseService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;

    public GetSalesArrangementHandler(
        ICurrentUserAccessor currentUser,
        ICaseServiceClient caseService,
        ISalesArrangementServiceClient salesArrangementService,
        Services.SalesArrangementAuthorization.ISalesArrangementAuthorizationService salesArrangementAuthorization)
    {
        _currentUser = currentUser;
        _caseService = caseService;
        _salesArrangementService = salesArrangementService;
        _salesArrangementAuthorization = salesArrangementAuthorization;
    }
}