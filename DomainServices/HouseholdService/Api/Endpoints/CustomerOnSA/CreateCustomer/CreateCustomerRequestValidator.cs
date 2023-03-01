﻿using DomainServices.HouseholdService.Contracts;
using FluentValidation;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.CreateCustomer;

internal sealed class CreateCustomerRequestValidator
    : AbstractValidator<CreateCustomerRequest>
{
    static DateTime _dateOfBirthMin = new DateTime(1900, 1, 1);

    public CreateCustomerRequestValidator(CodebookService.Clients.ICodebookServiceClients codebookService)
    {
        RuleFor(t => t.SalesArrangementId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.SalesArrangementIdIsEmpty);

        RuleFor(t => t.CustomerRoleId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.CustomerRoleIdIsEmpty)
            .MustAsync(async (t, cancellationToken) => (await codebookService.CustomerRoles(cancellationToken)).Any(c => c.Id == t))
            .WithErrorCode(ErrorCodeMapper.CustomerRoleNotFound);

        RuleFor(t => t.Customer.DateOfBirthNaturalPerson)
            .Must(d => d > _dateOfBirthMin && d < DateTime.Now)
            .WithErrorCode(ErrorCodeMapper.InvalidDateOfBirth)
            .When(t => t.Customer.DateOfBirthNaturalPerson is not null);
    }
}