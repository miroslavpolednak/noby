using DomainServices.CodebookService.Clients;
using DomainServices.HouseholdService.Contracts;
using FluentValidation;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.CreateCustomer;

internal sealed class CreateCustomerRequestValidator
    : AbstractValidator<CreateCustomerRequest>
{
    public CreateCustomerRequestValidator(ICodebookServiceClient codebookService)
    {
        RuleFor(t => t.SalesArrangementId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.SalesArrangementIdIsEmpty);

        RuleFor(t => t.CustomerRoleId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.CustomerRoleIdIsEmpty)
            .MustAsync(async (t, cancellationToken) => (await codebookService.CustomerRoles(cancellationToken)).Any(c => c.Id == t))
            .WithErrorCode(ErrorCodeMapper.CustomerRoleNotFound);

        RuleFor(t => t.Customer)
            .SetInheritanceValidator(v =>
            {
                v.Add(new Validators.CustomerOnSAValidator());
            });
    }
}