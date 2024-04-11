﻿using FluentValidation;
using NOBY.Api.Endpoints.Cases.IdentifyCase;

namespace NOBY.Api.Endpoints.Cases.SearchCases;

internal sealed class IdentifyCaseRequestValidator : AbstractValidator<IdentifyCaseRequest>
{
    public IdentifyCaseRequestValidator()
    {
        When(request => request.Criterion == Criterion.CaseId, () =>
        {
            RuleFor(r => r.CaseId)
                .NotNull();
        });
        
        When(request => request.Criterion == Criterion.ContractNumber, () =>
        {
            RuleFor(r => r.ContractNumber)
                .NotEmpty();
        });
        
        When(request => request.Criterion == Criterion.FormId, () =>
        {
            RuleFor(r => r.FormId)
                .NotEmpty();
        });
        
        When(request => request.Criterion == Criterion.PaymentAccount, () =>
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

        When(request => request.Criterion == Criterion.CustomerIdentity, () =>
        {
            RuleFor(r => r.CustomerIdentity)
                .NotNull()
                .ChildRules(identity =>
                {
                    identity.RuleFor(t => t!.IdentityId)
                        .NotEmpty();
                });
        });
    }
}
