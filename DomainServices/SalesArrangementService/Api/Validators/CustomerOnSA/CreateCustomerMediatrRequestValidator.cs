using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Validators;

internal class CreateCustomerMediatrRequestValidator
    : AbstractValidator<Dto.CreateCustomerMediatrRequest>
{
    public CreateCustomerMediatrRequestValidator()
    {
        RuleFor(t => t.Request.SalesArrangementId)
            .GreaterThan(0)
            .WithMessage("SalesArrangementId must be > 0").WithErrorCode("13000");
        
        RuleFor(t => t.Request.CustomerRoleId)
            .GreaterThan(0)
            .WithMessage("CustomerRoleId must be > 0").WithErrorCode("13000");
        
        //TODO jake dalsi validace?
    }
}