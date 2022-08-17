using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Validators;

internal class UpdateObligationMediatrRequestValidator
    : AbstractValidator<Dto.UpdateObligationMediatrRequest>
{
    public UpdateObligationMediatrRequestValidator(CodebookService.Abstraction.ICodebookServiceAbstraction codebookService)
    {
        RuleFor(t => t.Request.CustomerOnSAId)
            .GreaterThan(0)
            .WithMessage("CustomerOnSAId must be > 0").WithErrorCode("16024");

        RuleFor(t => t.Request.ObligationTypeId)
            .MustAsync(async (t, token) => !t.HasValue || (await codebookService.ObligationTypes(token)).Any(x => x.Id == t.Value))
            .WithMessage("ObligationTypeId is not valid").WithErrorCode("16048");

        RuleFor(t => t.Request.CreditCardLimit)
            .Must((r, t) => t == 0M || (r.Request.ObligationTypeId.GetValueOrDefault() != 1 && r.Request.ObligationTypeId.GetValueOrDefault() != 2))
            .WithMessage("CreditCardLimit not allowed for current ObligationTypeId").WithErrorCode("16049");

        RuleFor(t => t.Request.LoanPrincipalAmount)
            .Must((r, t) => t == 0M || (r.Request.ObligationTypeId.GetValueOrDefault() != 3 && r.Request.ObligationTypeId.GetValueOrDefault() != 4))
            .WithMessage("LoanPrincipalAmount not allowed for current ObligationTypeId").WithErrorCode("16050");

        RuleFor(t => t.Request.InstallmentAmount)
            .Must((r, t) => t == 0M || (r.Request.ObligationTypeId.GetValueOrDefault() != 3 && r.Request.ObligationTypeId.GetValueOrDefault() != 4))
            .WithMessage("InstallmentAmount not allowed for current ObligationTypeId").WithErrorCode("16051");

        RuleFor(t => t.Request.Creditor)
            .ChildRules(v =>
            {
                v.RuleFor(t => t.CreditorId)
                    .Must((creditor, t) => creditor.CreditorId.GetValueOrDefault() == 0 || string.IsNullOrEmpty(creditor.Name))
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
