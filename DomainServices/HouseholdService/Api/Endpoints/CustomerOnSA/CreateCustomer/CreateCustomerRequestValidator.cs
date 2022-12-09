using DomainServices.HouseholdService.Contracts;
using FluentValidation;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.CreateCustomer;

internal class CreateCustomerRequestValidator
    : AbstractValidator<CreateCustomerRequest>
{
    static DateTime _dateOfBirthMin = new DateTime(1900, 1, 1);

    public CreateCustomerRequestValidator(CodebookService.Clients.ICodebookServiceClients codebookService)
    {
        RuleFor(t => t.SalesArrangementId)
            .GreaterThan(0)
            .WithMessage("SalesArrangementId must be > 0").WithErrorCode("16010");

        RuleFor(t => t.CustomerRoleId)
            .GreaterThan(0)
            .WithMessage("CustomerRoleId must be > 0").WithErrorCode("16045");

        RuleFor(t => t.CustomerRoleId)
            .GreaterThan(0)
            .MustAsync(async (t, cancellationToken) => (await codebookService.CustomerRoles(cancellationToken)).Any(c => c.Id == t))
            .WithMessage(t => $"CustomerRoleId {t.CustomerRoleId} does not exist.").WithErrorCode("16021");

        RuleFor(t => t.Customer.DateOfBirthNaturalPerson)
            .Must(d => d > _dateOfBirthMin && d < DateTime.Now)
            .WithMessage("Date of birth is out of range").WithErrorCode("16038")
            .When(t => t.Customer.DateOfBirthNaturalPerson is not null);
    }
}