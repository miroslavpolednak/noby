using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Validators;

internal class CreateCustomerMediatrRequestValidator
    : AbstractValidator<Dto.CreateCustomerMediatrRequest>
{
    public CreateCustomerMediatrRequestValidator(CodebookService.Abstraction.ICodebookServiceAbstraction codebookService)
    {
        RuleFor(t => t.Request.SalesArrangementId)
            .GreaterThan(0)
            .WithMessage("SalesArrangementId must be > 0").WithErrorCode("13000");
        
        RuleFor(t => t.Request.CustomerRoleId)
            .GreaterThan(0)
            .WithMessage("CustomerRoleId must be > 0").WithErrorCode("13000");

        RuleFor(t => t.Request.CustomerRoleId)
            .GreaterThan(0)
            .MustAsync(async (t, cancellationToken) => (await codebookService.CustomerRoles(cancellationToken)).Any(c => c.Id == t))
            .WithMessage(t => $"CustomerRoleId {t.Request.CustomerRoleId} does not exist.").WithErrorCode("16021");
    }
}