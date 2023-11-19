using FluentValidation;

namespace DomainServices.ProductService.Api.Endpoints.GetProductList;

internal sealed class GetProductListRequestValidator : AbstractValidator<Contracts.GetProductListRequest>
{
    public GetProductListRequestValidator()
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.InvalidArgument12008);
    }
}

