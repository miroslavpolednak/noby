using DomainServices.CaseService.Contracts;
using FluentValidation;

namespace DomainServices.CaseService.Api.Endpoints.CreateCase;

internal sealed class CreateCaseRequestValidator 
    : AbstractValidator<CreateCaseRequest>
{
    public CreateCaseRequestValidator()
    {
        RuleFor(t => t.Data.ProductTypeId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.ProductTypeIdIsEmpty);

        RuleFor(t => (decimal)t.Data.TargetAmount)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.TargetAmountIsEmpty);

        RuleFor(t => t.CaseOwnerUserId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.CaseOwnerIsEmpty);

        RuleFor(t => t.Customer.Name)
            .NotEmpty()
            .WithErrorCode(ErrorCodeMapper.CustomerNameIsEmpty);
    }
}
