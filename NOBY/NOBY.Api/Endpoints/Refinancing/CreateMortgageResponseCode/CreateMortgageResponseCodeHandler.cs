﻿using DomainServices.CodebookService.Clients;
using DomainServices.OfferService.Clients.v1;
using DomainServices.ProductService.Clients;

namespace NOBY.Api.Endpoints.Refinancing.CreateMortgageResponseCode;

internal sealed class CreateMortgageResponseCodeHandler(
    IProductServiceClient _productService,
    IOfferServiceClient _offerService, 
    ICodebookServiceClient _codebookService)
        : IRequestHandler<CreateMortgageResponseCodeRequest>
{
    public async Task Handle(CreateMortgageResponseCodeRequest request, CancellationToken cancellationToken)
    {
        var responseCode = (await _codebookService.ResponseCodeTypes(cancellationToken))
            .First(t => t.Id == request.ResponseCodeTypeId);

        var mortgage = await _productService.GetMortgage(request.CaseId, cancellationToken);

        var serviceRequest = new DomainServices.OfferService.Contracts.CreateResponseCodeRequest
        {
            CaseId = request.CaseId,
            ResponseCodeTypeId = request.ResponseCodeTypeId,
            ResponseCodeCategory = DomainServices.OfferService.Contracts.ResponseCodeCategories.BusinessResponseCode,
            ValidTo = (DateTime?)mortgage.Mortgage.FixedRateValidTo ?? DateTime.Now.AddYears(1),
            Data = responseCode.DataType switch
            {
                DomainServices.CodebookService.Contracts.v1.ResponseCodeTypesResponse.Types.ResponseCodesItemDataTypes.Date => request.DataDateTime!.Value.ToString("s"),
                DomainServices.CodebookService.Contracts.v1.ResponseCodeTypesResponse.Types.ResponseCodesItemDataTypes.BankCode => request.DataBankCode,
                _ => request.DataString
            }
        };

        await _offerService.CreateResponseCode(serviceRequest, cancellationToken);
    }
}
