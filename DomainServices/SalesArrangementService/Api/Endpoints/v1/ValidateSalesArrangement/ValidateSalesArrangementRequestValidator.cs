using CIS.Infrastructure.CisMediatR.GrpcValidation;
using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Endpoints.ValidateSalesArrangement;

internal sealed class ValidateSalesArrangementRequestValidator
    : AbstractValidator<Contracts.ValidateSalesArrangementRequest>
{
    public ValidateSalesArrangementRequestValidator()
    {
        RuleFor(t => t.SalesArrangementId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.SalesArrangementIdIsEmpty);
    }
}
