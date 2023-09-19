using DomainServices.ProductService.Api.Database;
using DomainServices.ProductService.Contracts;

namespace DomainServices.ProductService.Api.Endpoints.GetCaseId;

internal sealed class GetCaseIdHandler : IRequestHandler<GetCaseIdRequest, GetCaseIdResponse>
{
    public async Task<GetCaseIdResponse> Handle(GetCaseIdRequest request, CancellationToken cancellation)
    {
        switch (request.RequestParametersCase)
        {
            case GetCaseIdRequest.RequestParametersOneofCase.ContractNumber:
                var caseId1 =  await _loanRepository.GetCaseIdByContractNumber(request.ContractNumber.ContractNumber, cancellation);
                if (!caseId1.HasValue)
                {
                    throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.ContractNumberNotFound, request.ContractNumber.ContractNumber);
                }
                
                return new GetCaseIdResponse { CaseId = caseId1.Value };

            case GetCaseIdRequest.RequestParametersOneofCase.PaymentAccount:
                var caseId2 =  await _loanRepository.GetCaseIdByPaymentAccount(request.PaymentAccount.Prefix, request.PaymentAccount.AccountNumber, cancellation);
                if (!caseId2.HasValue)
                {
                    throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.PaymentAccountNotFound, request.PaymentAccount.AccountNumber);
                }
                
                return new GetCaseIdResponse { CaseId = caseId2.Value };

            case GetCaseIdRequest.RequestParametersOneofCase.PcpId:
            {
                var caseId3 =  await _loanRepository.GetCaseIdByPcpId(request.PcpId.PcpId, cancellation);
                if (!caseId3.HasValue)
                {
                    throw ErrorCodeMapper.CreateNotFoundException(ErrorCodeMapper.PcpIdNotFound, request.PcpId.PcpId);
                }

                return new GetCaseIdResponse { CaseId = caseId3.Value };
            }

            default:
                throw new NotImplementedException();
        }
    }

    private readonly LoanRepository _loanRepository;

    public GetCaseIdHandler(LoanRepository loanRepository)
    {
        _loanRepository = loanRepository;
    }
}
