using FluentValidation;

namespace NOBY.Api.Endpoints.Cases.SearchCases;

internal sealed class IdentifyCaseRequestValidator 
    : AbstractValidator<CasesIdentifyCaseRequest>
{
    public IdentifyCaseRequestValidator()
    {
        When(request => request.Criterion == CasesIdentifyCaseRequestCriterion.CaseId, () =>
        {
            RuleFor(r => r.CaseId)
                .NotNull();
        });
        
        When(request => request.Criterion == CasesIdentifyCaseRequestCriterion.ContractNumber, () =>
        {
            RuleFor(r => r.ContractNumber)
                .NotEmpty();
        });
        
        When(request => request.Criterion == CasesIdentifyCaseRequestCriterion.FormId, () =>
        {
            RuleFor(r => r.FormId)
                .NotEmpty();
        });
        
        When(request => request.Criterion == CasesIdentifyCaseRequestCriterion.PaymentAccount, () =>
        {
            RuleFor(r => r.Account)
                .NotNull()
                .ChildRules(account =>
                {
                    account.RuleFor(a => a!.Prefix)
                        .MaximumLength(6);
                    
                    account.RuleFor(a => a!.Number)
                        .NotEmpty()
                        .MinimumLength(3)
                        .MaximumLength(10);
                });
        });

        When(request => request.Criterion == CasesIdentifyCaseRequestCriterion.CustomerIdentity, () =>
        {
            RuleFor(r => r.CustomerIdentity)
                .NotNull()
                .ChildRules(identity =>
                {
                    identity.RuleFor(t => t!.Id)
                        .NotEmpty();
                });
        });
    }
}
