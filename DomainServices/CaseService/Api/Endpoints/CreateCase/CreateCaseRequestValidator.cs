using DomainServices.CaseService.Contracts;
using FluentValidation;

namespace DomainServices.CaseService.Api.Endpoints.CreateCase;

internal class CreateCaseRequestValidator 
    : AbstractValidator<CreateCaseRequest>
{
    public CreateCaseRequestValidator()
    {
        RuleFor(t => t.Data.ProductTypeId)
            .GreaterThan(0)
            .WithMessage(t => "ProductTypeId must be > 0").WithErrorCode("13002");

        RuleFor(t => (decimal)t.Data.TargetAmount)
            .GreaterThan(0)
            .WithMessage("Target amount must be > 0").WithErrorCode("13018");

        RuleFor(t => t.CaseOwnerUserId)
            .GreaterThan(0)
            .WithMessage("CaseOwnerUserId must be > 0").WithErrorCode("13003");

        RuleFor(t => t.Customer.Name)
            .NotEmpty()
            .WithMessage("Customer Name must not be empty").WithErrorCode("13012");
    }
}
