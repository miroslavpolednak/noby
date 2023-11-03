namespace DomainServices.ProductService.Api.Endpoints.GetCaseId;

internal sealed class GetCaseIdHandler : IRequestHandler<GetCaseIdRequest, GetCaseIdResponse>
{
    private readonly LoanRepository _repository;

    public GetCaseIdHandler(LoanRepository repository)
    {
        _repository = repository;
    }

    public async Task<GetCaseIdResponse> Handle(GetCaseIdRequest request, CancellationToken cancellationToken)
    {
        var caseId = request.RequestParametersCase switch
        {
            GetCaseIdRequest.RequestParametersOneofCase.ContractNumber => await GetCaseIdByContractNumber(request.ContractNumber.ContractNumber, cancellationToken),
            GetCaseIdRequest.RequestParametersOneofCase.PaymentAccount => await GetCaseIdByPaymentAccount(request.PaymentAccount, cancellationToken),
            GetCaseIdRequest.RequestParametersOneofCase.PcpId => await GetCaseIdByPcpId(request.PcpId.PcpId, cancellationToken),
            GetCaseIdRequest.RequestParametersOneofCase.None => throw new NotImplementedException(),
            _ => throw new NotImplementedException()
        };

        return new GetCaseIdResponse { CaseId = caseId };
    }

    private async Task<long> GetCaseIdByContractNumber(string contractNumber, CancellationToken cancellationToken)
    {
        var caseId = await _repository.GetCaseIdByContractNumber(contractNumber, cancellationToken);

        return caseId ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.ContractNumberNotFound, contractNumber);
    }

    private async Task<long> GetCaseIdByPaymentAccount(PaymentAccountObject paymentAccount, CancellationToken cancellationToken)
    {
        var caseId = await _repository.GetCaseIdByPaymentAccount(paymentAccount.Prefix, paymentAccount.AccountNumber, cancellationToken);

        return caseId ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.PaymentAccountNotFound, paymentAccount.AccountNumber);
    }

    private async Task<long> GetCaseIdByPcpId(string pcpId, CancellationToken cancellationToken)
    {
        var caseId = await _repository.GetCaseIdByPcpId(pcpId, cancellationToken);

        return caseId ?? throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.PcpIdNotFound, pcpId);
    }
}
