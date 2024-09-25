using DomainServices.CodebookService.Clients;

namespace DomainServices.ProductService.Api.Endpoints.GetCaseId;

internal sealed class GetCaseIdHandler(IMpHomeClient _mpHomeClient, ICodebookServiceClient _codebookService)
    : IRequestHandler<GetCaseIdRequest, GetCaseIdResponse>
{
    public async Task<GetCaseIdResponse> Handle(GetCaseIdRequest request, CancellationToken cancellationToken)
    {
        var caseId = request.RequestParametersCase switch
        {
            GetCaseIdRequest.RequestParametersOneofCase.ContractNumber => await getCaseIdByContractNumber(request.ContractNumber.ContractNumber, cancellationToken),
            GetCaseIdRequest.RequestParametersOneofCase.PaymentAccount => await getCaseIdByPaymentAccount(request.PaymentAccount, cancellationToken),
            GetCaseIdRequest.RequestParametersOneofCase.PcpId => await getCaseIdByPcpId(request.PcpId.PcpId, cancellationToken),
            _ => throw new NotImplementedException()
        };

        return new GetCaseIdResponse
        {
            CaseId = caseId
        };
    }

    private async Task<long> getCaseIdByContractNumber(string contractNumber, CancellationToken cancellationToken)
    {
        var results = await _mpHomeClient.SearchCases(new CaseSearchRequest { ContractNumber = contractNumber }, cancellationToken);
        return await extractCaseId(results, cancellationToken) ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.ContractNumberNotFound, contractNumber);
    }

    private async Task<long> getCaseIdByPaymentAccount(PaymentAccountObject paymentAccount, CancellationToken cancellationToken)
    {
        var results = await _mpHomeClient.SearchCases(new CaseSearchRequest
        {
            AccountPrefix = paymentAccount?.Prefix?.PadLeft(6, '0'),
            AccountNumber = paymentAccount?.AccountNumber?.PadLeft(10, '0')
        },
          cancellationToken);

        return await extractCaseId(results, cancellationToken) ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.PaymentAccountNotFound, paymentAccount?.AccountNumber);
    }

    private async Task<long> getCaseIdByPcpId(string pcpId, CancellationToken cancellationToken)
    {
        var results = await _mpHomeClient.SearchCases(new CaseSearchRequest { PcpInstId = pcpId }, cancellationToken);
        return await extractCaseId(results, cancellationToken) ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.PcpIdNotFound, pcpId);
    }

    private async Task<long?> extractCaseId(List<CaseSearchResponse>? results, CancellationToken cancellationToken)
    {
        var productTypes = await _codebookService.ProductTypes(cancellationToken);

        results = FilterResult();

        if (!results.Any())
        {
            return null;
        }

        return results.First().CaseId;

        List<CaseSearchResponse> FilterResult() => results?.Where(t => productTypes.Any(p => p.Id == t.ProductTypeId)).ToList() ?? [];
    }
}
