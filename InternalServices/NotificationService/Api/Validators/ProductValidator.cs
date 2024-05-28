using FluentValidation;

namespace CIS.InternalServices.NotificationService.Api.Validators;

public class ProductValidator
    : AbstractValidator<Contracts.v2.Product>
{
    public ProductValidator()
    {
        RuleFor(request => request.ProductId)
            .NotEmpty()
            .WithErrorCode(ErrorCodeMapper.ProductIdRequired);

        RuleFor(request => request.ProductType)
            .Must(x => x != Contracts.v2.Product.Types.ProductTypes.Unknown && Enum.IsDefined(x))
            .WithErrorCode(ErrorCodeMapper.ProductTypeRequired);
    }
}