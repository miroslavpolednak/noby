using DomainServices.CustomerService.Api.Dto;
using FluentValidation;

namespace DomainServices.CustomerService.Api.Validators;

internal class CreateCustomerMediatrRequestValidator : AbstractValidator<CreateCustomerMediatrRequest>
{
    public CreateCustomerMediatrRequestValidator()
    {
    }
}