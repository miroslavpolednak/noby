using DomainServices.CaseService.Contracts;
using FluentValidation;

namespace DomainServices.CaseService.Api.Endpoints.UpdateCustomerData;

internal sealed class UpdateCustomerDataRequestValidator 
    : AbstractValidator<UpdateCustomerDataRequest>
{
    public UpdateCustomerDataRequestValidator()
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.CaseIdIsEmpty);

        RuleFor(t => t.Customer.Name)
            .NotEmpty()
            .WithErrorCode(ErrorCodeMapper.CustomerNameIsEmpty);
    }
}
