﻿using NOBY.Api.Endpoints.SalesArrangement.SharedDto;
using _SA = DomainServices.SalesArrangementService.Contracts;

namespace NOBY.Api.Endpoints.SalesArrangement.ValidateSalesArrangement;

internal sealed class ValidateSalesArrangementHandler
    : IRequestHandler<ValidateSalesArrangementRequest, ValidateSalesArrangementResponse>
{
    public async Task<ValidateSalesArrangementResponse> Handle(ValidateSalesArrangementRequest request, CancellationToken cancellationToken)
    {
        // validace opravneni
        await _salesArrangementAuthorization.ValidateSaAccessBySaType213And248BySAId(request.SalesArrangementId, cancellationToken);

        var response = await _salesArrangementService.ValidateSalesArrangement(request.SalesArrangementId, cancellationToken);

        return new ValidateSalesArrangementResponse
        {
            Categories = response
                .ValidationMessages
                ?.Where(t => t.NobyMessageDetail.Severity != _SA.ValidationMessageNoby.Types.NobySeverity.None)
                .GroupBy(t => t.NobyMessageDetail.Category)
                .OrderBy(t => t.Min(x => x.NobyMessageDetail.CategoryOrder))
                .Select(t => new ValidateCategory
                {
                    CategoryName = t.Key,
                    ValidationMessages = t.Select(t2 => new ValidateMessage
                    {
                        Message = t2.NobyMessageDetail.Message,
                        Parameter = t2.NobyMessageDetail.ParameterName,
                        Severity = t2.NobyMessageDetail.Severity == _SA.ValidationMessageNoby.Types.NobySeverity.Error ? MessageSeverity.Error : MessageSeverity.Warning
                    }).ToList()
                }).ToList()
        };   
    }

    private readonly Services.SalesArrangementAuthorization.ISalesArrangementAuthorizationService _salesArrangementAuthorization;
    private readonly DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClient _salesArrangementService;

    public ValidateSalesArrangementHandler(DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClient salesArrangementService, Services.SalesArrangementAuthorization.ISalesArrangementAuthorizationService salesArrangementAuthorization)
    {
        _salesArrangementService = salesArrangementService;
        _salesArrangementAuthorization = salesArrangementAuthorization;
    }
}
