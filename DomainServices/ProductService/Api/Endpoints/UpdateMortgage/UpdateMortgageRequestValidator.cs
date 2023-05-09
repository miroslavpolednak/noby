using CIS.Infrastructure.CisMediatR.GrpcValidation;
using FluentValidation;

namespace DomainServices.ProductService.Api.Endpoints.UpdateMortgage;

internal class UpdateMortgageRequestValidator : AbstractValidator<Contracts.UpdateMortgageRequest>
{
    public UpdateMortgageRequestValidator()
    {
        RuleFor(t => t.ProductId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.InvalidArgument12014);

        RuleFor(t => t.Mortgage.ProductTypeId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.InvalidArgument12009);

        RuleFor(t => t.Mortgage.PartnerId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.InvalidArgument12010);
    }
}

