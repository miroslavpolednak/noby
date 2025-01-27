﻿using _V2 = DomainServices.RiskIntegrationService.Contracts.CreditWorthiness.V2;
using _C4M = DomainServices.RiskIntegrationService.ExternalServices.CreditWorthiness.V3;
using DomainServices.CodebookService.Contracts.v1;

namespace DomainServices.RiskIntegrationService.Api.Endpoints.CreditWorthiness.V2.SimpleCalculate;

internal sealed class SimpleCalculateHandler
    : IRequestHandler<_V2.CreditWorthinessSimpleCalculateRequest, _V2.CreditWorthinessSimpleCalculateResponse>
{
    public async Task<_V2.CreditWorthinessSimpleCalculateResponse> Handle(_V2.CreditWorthinessSimpleCalculateRequest request, CancellationToken cancellationToken)
    {
        // appl type pro aktualni produkt
        var riskApplicationType = Helpers.GetRiskApplicationType(await _codebookService.RiskApplicationTypes(cancellationToken), request.Product!.ProductTypeId);

        // request pro hlavni bonita sluzbu
        var requestModel = await _requestMapper.MapToC4m(request.ToFullRequest(), riskApplicationType, cancellationToken);

        // volani c4m hlavni bonita sluzby
        var response = await _client.Calculate(requestModel, cancellationToken);

        return response.ToServiceResponse(request.Product.LoanPaymentAmount);
    }

    private readonly CodebookService.Clients.ICodebookServiceClient _codebookService;
    private readonly _C4M.ICreditWorthinessClient _client;
    private readonly Calculate.Mappers.CalculateRequestMapper _requestMapper;

    public SimpleCalculateHandler(
        CodebookService.Clients.ICodebookServiceClient codebookService,
        _C4M.ICreditWorthinessClient client,
        Calculate.Mappers.CalculateRequestMapper requestMapper)
    {
        _codebookService = codebookService;
        _client = client;
        _requestMapper = requestMapper;
    }
}
