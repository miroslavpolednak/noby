using CIS.Core.Security;
using DomainServices.OfferService.Clients.v1;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.UserService.Clients;

namespace NOBY.Api.Endpoints.Offer.SimulateMortgage;

internal sealed class SimulateMortgageHandler(
    ICurrentUserAccessor _userAccessor,
    IOfferServiceClient _offerService,
    ISalesArrangementServiceClient _salesArrangementService,
    IUserServiceClient _userService)
        : IRequestHandler<SimulateMortgageRequest, SimulateMortgageResponse>
{
    public async Task<SimulateMortgageResponse> Handle(SimulateMortgageRequest request, CancellationToken cancellationToken)
    {
        // HFICH-5024
        if ((request.Developer?.DeveloperId != null && request.Developer?.ProjectId != null && !string.IsNullOrEmpty(request.Developer?.Description))
            || (request.Developer?.DeveloperId != null && request.Developer?.ProjectId == null && string.IsNullOrEmpty(request.Developer?.Description)))
        {
            throw new CisValidationException("Invalid developer parameters combination");
        }

        // validate permissions
        if (request.IsEmployeeBonusRequested.GetValueOrDefault() && !_userAccessor.HasPermission(UserPermissions.LOANMODELING_EmployeeMortgageAccess))
        {
            throw new CisAuthorizationException("IsEmployeeBonusRequested check failed");
        }

        // datum garance
        DateTime guaranteeDateFrom;
        if (!request.WithGuarantee.GetValueOrDefault())
            guaranteeDateFrom = DateTime.Now;
        else
        {
            if (!request.SalesArrangementId.HasValue)
                throw new CisValidationException("withGuarantee=true, but SalesArrangementId is not set");

            var saInstance = await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId.Value, cancellationToken);
            guaranteeDateFrom = saInstance.OfferGuaranteeDateFrom;
        }

        // predelat na DS request
        bool vipFlag = await getVipFlag(request.SalesArrangementId, cancellationToken);
		var model = request.ToDomainServiceRequest(guaranteeDateFrom, vipFlag);

        // zavolat DS
        var result = await _offerService.SimulateMortgage(model, cancellationToken);

        return new()
        {
            OfferId = result.OfferId,
            OfferGuaranteeDateTo = result.BasicParameters.GuaranteeDateTo,
            ResourceProcessId = result.ResourceProcessId,
            SimulationResults = result.SimulationResults.ToApiResponse(model.SimulationInputs, result.AdditionalSimulationResults),
            CreditWorthinessSimpleResults = result.CreditWorthinessSimpleResults.ToApiResponse()
        };
    }

    private async Task<bool> getVipFlag(int? salesArrangementId, CancellationToken cancellationToken)
    {
        int? userId = null;
        if (salesArrangementId.HasValue)
        {
			var saInstance = await _salesArrangementService.GetSalesArrangement(salesArrangementId.Value, cancellationToken);
			userId = saInstance.Created?.UserId ?? _userAccessor.User!.Id;
        }
		else
        {
            userId = _userAccessor.User!.Id;
		}

		var user = await _userService.GetUser(userId.Value, cancellationToken);
		return user.UserInfo.IsUserVIP;
	}
}
