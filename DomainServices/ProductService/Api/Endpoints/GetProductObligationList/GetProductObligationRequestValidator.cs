using FluentValidation;

namespace DomainServices.ProductService.Api.Endpoints.GetProductObligationList;

internal sealed class GetProductObligationRequestValidator : AbstractValidator<GetProductObligationListRequest>
{
    public GetProductObligationRequestValidator()
    {
        RuleFor(t => t.ProductId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.InvalidArgument12014);
    }
}