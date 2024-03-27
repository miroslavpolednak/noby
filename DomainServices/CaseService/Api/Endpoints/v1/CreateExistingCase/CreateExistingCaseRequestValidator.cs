using DomainServices.CaseService.Contracts;
using FastEnumUtility;
using FluentValidation;
using SharedTypes.Enums;

namespace DomainServices.CaseService.Api.Endpoints.v1.CreateExistingCase;

internal sealed class CreateExistingCaseRequestValidator
    : AbstractValidator<CreateExistingCaseRequest>
{
    public CreateExistingCaseRequestValidator()
    {
        RuleFor(t => t.CaseId)
            .NotEmpty()
            .WithErrorCode(ErrorCodeMapper.CaseIdIsEmpty);

        RuleFor(t => t.State)
            .NotEmpty()
            .Must(t => FastEnum.IsDefined((CaseStates)t))
            .WithErrorCode(ErrorCodeMapper.InvalidCaseState);

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
