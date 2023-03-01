using DomainServices.HouseholdService.Contracts;
using FluentValidation;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.UpdateObligation;

internal sealed class UpdateObligationRequestValidator
    : AbstractValidator<Obligation>
{
    public UpdateObligationRequestValidator(CodebookService.Clients.ICodebookServiceClients codebookService)
    {
        RuleFor(t => t.CustomerOnSAId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.CustomerOnSAIdIsEmpty);

        RuleFor(t => t.ObligationTypeId)
            .MustAsync(async (t, token) => !t.HasValue || (await codebookService.ObligationTypes(token)).Any(x => x.Id == t.Value))
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

        /*RuleFor(t => t.Request.Correction)
            .ChildRules(v =>
            {
                v.RuleFor(t => t.CorrectionTypeId)
                    .MustAsync(async (t, token) => !t.HasValue || (await codebookService.ObligationCorrectionTypes(token)).Any(x => x.Id == t.Value))
                    .WithMessage("ObligationTypeId is not valid").WithErrorCode("0");
            });*/
    }
}
