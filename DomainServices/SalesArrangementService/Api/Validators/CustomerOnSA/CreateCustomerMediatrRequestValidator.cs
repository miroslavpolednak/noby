using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Validators;

internal class CreateCustomerMediatrRequestValidator
    : AbstractValidator<Dto.CreateCustomerMediatrRequest>
{
    static DateTime _dateOfBirthMin = new DateTime(1900, 1, 1);

    public CreateCustomerMediatrRequestValidator(CodebookService.Abstraction.ICodebookServiceAbstraction codebookService)
    {
        RuleFor(t => t.Request.SalesArrangementId)
            .GreaterThan(0)
            .WithMessage("SalesArrangementId must be > 0").WithErrorCode("16010");
        
        RuleFor(t => t.Request.CustomerRoleId)
            .GreaterThan(0)
            .WithMessage("CustomerRoleId must be > 0").WithErrorCode("16045");

        RuleFor(t => t.Request.CustomerRoleId)
            .GreaterThan(0)
            .MustAsync(async (t, cancellationToken) => (await codebookService.CustomerRoles(cancellationToken)).Any(c => c.Id == t))
            .WithMessage(t => $"CustomerRoleId {t.Request.CustomerRoleId} does not exist.").WithErrorCode("16021");

        RuleFor(t => t.Request.Customer.DateOfBirthNaturalPerson)
            .Must(d => d > _dateOfBirthMin && d < DateTime.Now)
            .WithMessage("Date of birth is out of range").WithErrorCode("16038")
            .When(t => t.Request.Customer.DateOfBirthNaturalPerson is not null);
    }
}