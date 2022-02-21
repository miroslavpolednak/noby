using CIS.Core.Validation;
using CIS.Infrastructure.WebApi.Validation;
using DomainServices.CustomerService.Dto;
using FluentValidation;

namespace DomainServices.CustomerService.Api.Validators
{
    internal class GetDetailRequestValidator : AbstractValidator<GetCustomerDetailMediatrRequest>, IValidatableRequest
    {
        public GetDetailRequestValidator()
        {
            RuleFor(t => t.Request.Identity.IdentityId).GreaterThan(0).WithMessage("Identity");
        }
    }
}
