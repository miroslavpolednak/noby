using CIS.Infrastructure.CisMediatR.GrpcValidation;
using FluentValidation;

namespace DomainServices.ProductService.Api.Endpoints.CreateMortgage;

internal sealed class CreateMortgageRequestValidator : AbstractValidator<Contracts.CreateMortgageRequest>
{
    public CreateMortgageRequestValidator()
    {
        RuleFor(t => t.CaseId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.InvalidArgument12008);

        RuleFor(t => t.Mortgage.ProductTypeId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.InvalidArgument12009);

        RuleFor(t => t.Mortgage.PartnerId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.InvalidArgument12010);

        RuleFor(t => t.Mortgage.LoanKindId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.InvalidArgument12007);
    }
}

