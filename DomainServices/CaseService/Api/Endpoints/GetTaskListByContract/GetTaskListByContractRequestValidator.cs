using FluentValidation;

namespace DomainServices.CaseService.Api.Endpoints.GetTaskListByContract;

internal sealed class GetTaskListByContractRequestValidator
    : AbstractValidator<Contracts.GetTaskListByContractRequest>
{
    public GetTaskListByContractRequestValidator()
    {
        RuleFor(t => t.ContractNumber)
            .NotEmpty()
            .WithErrorCode(ErrorCodeMapper.ContractNumberIsEmpty);
    }
}
