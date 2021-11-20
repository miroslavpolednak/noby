using DomainServices.ProductService.Api.Dto;
using FluentValidation;

namespace DomainServices.OfferService.Api.Validators;

internal class CreateProductInstanceMediatrRequestValidator : AbstractValidator<CreateProductInstanceMediatrRequest>
{
    public CreateProductInstanceMediatrRequestValidator()
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0)
            .WithMessage(t => "CaseId must be greater than 0").WithErrorCode("10001");

        RuleFor(t => t.ProductInstanceType)
            .GreaterThan(0)
            .WithMessage(t => "ProductInstanceType must be greater than 0").WithErrorCode("10001");

    }
}
