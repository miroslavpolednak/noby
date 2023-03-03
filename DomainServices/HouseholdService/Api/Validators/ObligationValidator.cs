using DomainServices.HouseholdService.Contracts;
using FluentValidation;

namespace DomainServices.HouseholdService.Api.Validators;

internal sealed class ObligationValidator
    : AbstractValidator<IObligation>
{
    public ObligationValidator(CodebookService.Clients.ICodebookServiceClients codebookService)
    {
        RuleFor(t => t.ObligationTypeId)
            .MustAsync(async (t, cancellationToken) => !t.HasValue || (await codebookService.ObligationTypes(cancellationToken)).Any(x => x.Id == t.Value))
            .WithErrorCode(ErrorCodeMapper.ObligationTypeIsEmpty);

        RuleFor(t => t.CreditCardLimit)
            .Must((r, t) => t is null || t == 0M || r.ObligationTypeId.GetValueOrDefault() != 1 && r.ObligationTypeId.GetValueOrDefault() != 2)
            .WithErrorCode(ErrorCodeMapper.CreditCardLimitNotAllowed);

        RuleFor(t => t.LoanPrincipalAmount)
            .Must((r, t) => t is null || t == 0M || r.ObligationTypeId.GetValueOrDefault() != 3 && r.ObligationTypeId.GetValueOrDefault() != 4)
            .WithErrorCode(ErrorCodeMapper.LoanPrincipalAmountNotAllowed);

        RuleFor(t => t.InstallmentAmount)
            .Must((r, t) => t is null || t == 0M || r.ObligationTypeId.GetValueOrDefault() != 3 && r.ObligationTypeId.GetValueOrDefault() != 4)
            .WithErrorCode(ErrorCodeMapper.InstallmentAmountNotAllowed);

        RuleFor(t => t.Creditor)
            .ChildRules(v =>
            {
                v.RuleFor(t => t.CreditorId)
                    .Must((creditor, t) => string.IsNullOrEmpty(creditor.CreditorId) || string.IsNullOrEmpty(creditor.Name))
                    .WithErrorCode(ErrorCodeMapper.CreditorIdAndNameInSameTime);
            });
    }
}