using DomainServices.HouseholdService.Contracts;
using FluentValidation;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.CreateObligation;

internal sealed class CreateObligationRequestValidator
    : AbstractValidator<CreateObligationRequest>
{
    public CreateObligationRequestValidator(CodebookService.Clients.ICodebookServiceClients codebookService)
    {
        RuleFor(t => t.CustomerOnSAId)
            .GreaterThan(0)
            .WithMessage("CustomerOnSAId must be > 0").WithErrorCode("16024");

        RuleFor(t => t.ObligationTypeId)
            .MustAsync(async (t, token) => !t.HasValue || (await codebookService.ObligationTypes(token)).Any(x => x.Id == t.Value))
            .WithMessage("ObligationTypeId is not valid").WithErrorCode("16048");

        RuleFor(t => t.CreditCardLimit)
            .Must((r, t) => t is null || t == 0M || r.ObligationTypeId.GetValueOrDefault() != 1 && r.ObligationTypeId.GetValueOrDefault() != 2)
            .WithMessage("CreditCardLimit not allowed for current ObligationTypeId").WithErrorCode("16049");

        RuleFor(t => t.LoanPrincipalAmount)
            .Must((r, t) => t is null || t == 0M || r.ObligationTypeId.GetValueOrDefault() != 3 && r.ObligationTypeId.GetValueOrDefault() != 4)
            .WithMessage("LoanPrincipalAmount not allowed for current ObligationTypeId").WithErrorCode("16050");

        RuleFor(t => t.InstallmentAmount)
            .Must((r, t) => t is null || t == 0M || r.ObligationTypeId.GetValueOrDefault() != 3 && r.ObligationTypeId.GetValueOrDefault() != 4)
            .WithMessage("InstallmentAmount not allowed for current ObligationTypeId").WithErrorCode("16051");

        RuleFor(t => t.Creditor)
            .ChildRules(v =>
            {
                v.RuleFor(t => t.CreditorId)
                    .Must((creditor, t) => string.IsNullOrEmpty(creditor.CreditorId) || string.IsNullOrEmpty(creditor.Name))
                    .WithMessage("Creditor.CreditorId and Creditor.Name can't be set in the same time").WithErrorCode("16052");
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
