﻿using DomainServices.CaseService.Contracts;
using FluentValidation;

namespace DomainServices.CaseService.Api.Endpoints.UpdateCaseCustomer;

internal sealed class UpdateCaseCustomerRequestValidator 
    : AbstractValidator<UpdateCaseCustomerRequest>
{
    public UpdateCaseCustomerRequestValidator()
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0)
            .WithMessage("CaseId must be > 0").WithErrorCode("13016");

        RuleFor(t => t.Customer.Name)
            .NotEmpty()
            .WithMessage("Customer Name must not be empty").WithErrorCode("13012");
    }
}