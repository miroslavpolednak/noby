using FluentValidation;

namespace DomainServices.ProductService.Api.Endpoints.GetProductList;

internal class GetProductListRequestValidator : AbstractValidator<Contracts.GetProductListRequest>
{
    public GetProductListRequestValidator()
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0)
            .WithMessage("CaseId is not specified").WithErrorCode("12008");
    }
}

