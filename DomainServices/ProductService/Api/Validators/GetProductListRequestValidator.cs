using FluentValidation;

namespace DomainServices.ProductService.Api.Validators;

internal class GetProductListRequestValidator : AbstractValidator<Dto.GetProductListMediatrRequest>
{
    public GetProductListRequestValidator()
    {
        RuleFor(t => t.Request.CaseId)
            .GreaterThan(0)
            .WithMessage("CaseId is not specified").WithErrorCode("12008");
    }
}

