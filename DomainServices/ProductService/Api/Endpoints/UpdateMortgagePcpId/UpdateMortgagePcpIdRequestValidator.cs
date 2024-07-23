using FluentValidation;

namespace DomainServices.ProductService.Api.Endpoints.UpdateMortgagePcpId;

internal sealed class UpdateMortgagePcpIdRequestValidator
    : AbstractValidator<UpdateMortgagePcpIdRequest>
{
    public UpdateMortgagePcpIdRequestValidator()
    {
        RuleFor(t => t.ProductId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.InvalidArgument12014);
    }
}
