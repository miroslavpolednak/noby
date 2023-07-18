using CIS.Infrastructure.CisMediatR.GrpcValidation;
using FluentValidation;

namespace DomainServices.ProductService.Api.Endpoints.GetCaseId;

internal sealed class GetCaseIdRequestValidator : AbstractValidator<Contracts.GetCaseIdRequest>
{
    public GetCaseIdRequestValidator()
    {
        When(t => t.RequestParametersCase == Contracts.GetCaseIdRequest.RequestParametersOneofCase.ContractNumber, () =>
        {
            RuleFor(t => t.ContractNumber.ContractNumber)
                .NotEmpty()
                .WithErrorCode(ErrorCodeMapper.ContractNumberNotFound);
        });

        When(t => t.RequestParametersCase == Contracts.GetCaseIdRequest.RequestParametersOneofCase.PaymentAccount, () =>
        {
            RuleFor(t => t.PaymentAccount.AccountNumber)
                .NotEmpty()
                .WithErrorCode(ErrorCodeMapper.PaymentAccountNotFound);
        });

        When(t => t.RequestParametersCase == Contracts.GetCaseIdRequest.RequestParametersOneofCase.PcpId,
             () =>
             {
                 RuleFor(t => t.PcpId.PcpId)
                     .NotEmpty()
                     .WithErrorCode(ErrorCodeMapper.PcpIdNotFound);
             });
    }
}
