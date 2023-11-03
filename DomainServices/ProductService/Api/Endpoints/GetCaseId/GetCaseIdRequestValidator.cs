using FluentValidation;

namespace DomainServices.ProductService.Api.Endpoints.GetCaseId;

internal sealed class GetCaseIdRequestValidator : AbstractValidator<GetCaseIdRequest>
{
    public GetCaseIdRequestValidator()
    {
        When(t => t.RequestParametersCase == GetCaseIdRequest.RequestParametersOneofCase.ContractNumber, () =>
        {
            RuleFor(t => t.ContractNumber.ContractNumber)
                .NotEmpty()
                .WithErrorCode(ErrorCodeMapper.ContractNumberNotFound);
        });

        When(t => t.RequestParametersCase == GetCaseIdRequest.RequestParametersOneofCase.PaymentAccount, () =>
        {
            RuleFor(t => t.PaymentAccount.AccountNumber)
                .NotEmpty()
                .WithErrorCode(ErrorCodeMapper.PaymentAccountNotFound);
        });

        When(t => t.RequestParametersCase == GetCaseIdRequest.RequestParametersOneofCase.PcpId,
             () =>
             {
                 RuleFor(t => t.PcpId.PcpId)
                     .NotEmpty()
                     .WithErrorCode(ErrorCodeMapper.PcpIdNotFound);
             });
    }
}
