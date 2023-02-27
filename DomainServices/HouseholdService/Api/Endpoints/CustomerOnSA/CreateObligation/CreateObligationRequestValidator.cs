using DomainServices.HouseholdService.Contracts;
using FluentValidation;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.CreateObligation;

internal sealed class CreateObligationRequestValidator
    : AbstractValidator<CreateObligationRequest>
{
    public CreateObligationRequestValidator(CodebookService.Clients.ICodebookServiceClients codebookService, Database.HouseholdServiceDbContext dbContext)
    {
        RuleFor(t => t.CustomerOnSAId)
            .GreaterThan(0)
            .WithErrorCode(ValidationMessages.CustomerOnSAIdIsEmpty);

        // customer nenalezen v DB
        RuleFor(t => t.CustomerOnSAId)
            .MustAsync(async (customerOnSAId, cancellationToken) => await dbContext.Customers.AnyAsync(t => t.CustomerOnSAId == customerOnSAId, cancellationToken))
            .WithErrorCode(ValidationMessages.CustomerOnSANotFound)
            .ThrowCisException(GrpcValidationBehaviorExeptionTypes.CisNotFoundException);

        RuleFor(t => t.ObligationTypeId)
            .MustAsync(async (t, cancellationToken) => !t.HasValue || (await codebookService.ObligationTypes(cancellationToken)).Any(x => x.Id == t.Value))
            .WithErrorCode(ValidationMessages.ObligationTypeIsEmpty);

        RuleFor(t => t.CreditCardLimit)
            .Must((r, t) => t is null || t == 0M || r.ObligationTypeId.GetValueOrDefault() != 1 && r.ObligationTypeId.GetValueOrDefault() != 2)
            .WithErrorCode(ValidationMessages.CreditCardLimitNotAllowed);

        RuleFor(t => t.LoanPrincipalAmount)
            .Must((r, t) => t is null || t == 0M || r.ObligationTypeId.GetValueOrDefault() != 3 && r.ObligationTypeId.GetValueOrDefault() != 4)
            .WithErrorCode(ValidationMessages.LoanPrincipalAmountNotAllowed);

        RuleFor(t => t.InstallmentAmount)
            .Must((r, t) => t is null || t == 0M || r.ObligationTypeId.GetValueOrDefault() != 3 && r.ObligationTypeId.GetValueOrDefault() != 4)
            .WithErrorCode(ValidationMessages.InstallmentAmountNotAllowed);

        RuleFor(t => t.Creditor)
            .ChildRules(v =>
            {
                v.RuleFor(t => t.CreditorId)
                    .Must((creditor, t) => string.IsNullOrEmpty(creditor.CreditorId) || string.IsNullOrEmpty(creditor.Name))
                    .WithErrorCode(ValidationMessages.CreditorIdAndNameInSameTime);
            });

        /*RuleFor(t => t.Request.Correction)
            .ChildRules(v =>
            {
                v.RuleFor(t => t.CorrectionTypeId)
                    .MustAsync(async (t, token) => !t.HasValue || (await codebookService.ObligationCorrectionTypes(token)).Any(x => x.Id == t.Value))
                    .WithMessage("ObligationTypeId is not valid").WithErrorCode("0");
            });*/
    }
}
