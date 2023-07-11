using DomainServices.CaseService.Contracts;
using FluentValidation;

namespace DomainServices.CaseService.Api.Endpoints.CreateCase;

internal sealed class CreateCaseRequestValidator 
    : AbstractValidator<CreateCaseRequest>
{
    public CreateCaseRequestValidator()
    {
        RuleFor(t => t.Data)
            .NotNull()
            .WithErrorCode(ErrorCodeMapper.ProductTypeIdIsEmpty);

        RuleFor(t => t.Customer)
            .NotNull()
            .WithErrorCode(ErrorCodeMapper.CustomerNameIsEmpty);

        When(t => t.Data is not null, () =>
        {
            RuleFor(t => t.Data.ProductTypeId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.ProductTypeIdIsEmpty);

            RuleFor(t => (decimal)t.Data.TargetAmount)
                .GreaterThan(0)
                .WithErrorCode(ErrorCodeMapper.TargetAmountIsEmpty);
        });
        
        RuleFor(t => t.CaseOwnerUserId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.CaseOwnerIsEmpty);

        When(t => t.Customer is not null, () =>
        {
            RuleFor(t => t.Customer.Name)
            .NotEmpty()
            .WithErrorCode(ErrorCodeMapper.CustomerNameIsEmpty);
        });
    }
}
