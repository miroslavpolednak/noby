﻿using DomainServices.ProductService.Clients;

namespace NOBY.Api.Endpoints.Cases.GetCovenant;

internal sealed class GetCovenantHandler(IProductServiceClient _productService)
        : IRequestHandler<GetCovenantRequest, CasesGetCovenantResponse>
{
    public async Task<CasesGetCovenantResponse> Handle(GetCovenantRequest request, CancellationToken cancellationToken)
    {
        var covenant = await _productService.GetCovenantDetail(request.CaseId, request.CovenantOrder, cancellationToken);

        return new CasesGetCovenantResponse
        {
            Name = covenant.Name,
            IsFulfilled = covenant.IsFulfilled,
            FulfillDate = covenant.FulfillDate,
            Text = covenant.Text,
            Description = covenant.Description
        };
    }
}
