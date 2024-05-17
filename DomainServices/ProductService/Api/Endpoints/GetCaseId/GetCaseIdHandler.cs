namespace DomainServices.ProductService.Api.Endpoints.GetCaseId;

internal sealed class GetCaseIdHandler(IMpHomeClient _mpHomeClient)
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
        var caseId = await _mpHomeClient.SearchCases(new CaseSearchRequest { ContractNumber = contractNumber }, cancellationToken);
        return caseId ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.ContractNumberNotFound, contractNumber);
    }

    private async Task<long> getCaseIdByPaymentAccount(PaymentAccountObject paymentAccount, CancellationToken cancellationToken)
    {
		var caseId = await _mpHomeClient.SearchCases(new CaseSearchRequest { AccountPrefix = paymentAccount.Prefix, AccountNumber = paymentAccount.AccountNumber }, cancellationToken);
		return caseId ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.PaymentAccountNotFound, paymentAccount.AccountNumber);
    }

    private async Task<long> getCaseIdByPcpId(string pcpId, CancellationToken cancellationToken)
    {
		var caseId = await _mpHomeClient.SearchCases(new CaseSearchRequest { PcpInstId = pcpId }, cancellationToken);
		return caseId ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.PcpIdNotFound, pcpId);
    }
}
