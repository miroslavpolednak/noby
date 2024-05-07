﻿using DomainServices.CodebookService.Clients;
using DomainServices.OfferService.Clients.v1;

namespace NOBY.Api.Endpoints.Refinancing.CreateMortgageResponseCode;

internal sealed class CreateMortgageResponseCodeHandler
    : IRequestHandler<CreateMortgageResponseCodeRequest>
{
    public async Task Handle(CreateMortgageResponseCodeRequest request, CancellationToken cancellationToken)
    {
        var responseCode = (await _codebookService.ResponseCodeTypes(cancellationToken))
            .First(t => t.Id == request.ResponseCodeTypeId);

        var serviceRequest = new DomainServices.OfferService.Contracts.CreateResponseCodeRequest
        {
            CaseId = request.CaseId,
            ResponseCodeTypeId = request.ResponseCodeTypeId,
            ResponseCodeCategory = DomainServices.OfferService.Contracts.ResponseCodeCategories.BusinessResponseCode,
            Data = responseCode.DataType switch
            {
                DomainServices.CodebookService.Contracts.v1.ResponseCodeTypesResponse.Types.ResponseCodesItemDataTypes.Date => request.DataDateTime!.Value.ToString("s"),
                DomainServices.CodebookService.Contracts.v1.ResponseCodeTypesResponse.Types.ResponseCodesItemDataTypes.BankCode => request.DataBankCode,
                _ => request.DataString
            }
        };

        await _offerService.CreateResponseCode(serviceRequest, cancellationToken);
    }

    private readonly IOfferServiceClient _offerService;
    private readonly ICodebookServiceClient _codebookService;

    public CreateMortgageResponseCodeHandler(IOfferServiceClient offerService, ICodebookServiceClient codebookService)
    {
        _offerService = offerService;
        _codebookService = codebookService;
    }
}