using CIS.Infrastructure.CisMediatR.GrpcValidation;
using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Endpoints.GetSalesArrangement;

internal sealed class GetSalesArrangementRequestValidator
    : AbstractValidator<Contracts.GetSalesArrangementRequest>
{
    public GetSalesArrangementRequestValidator()
    {
        RuleFor(t => t.SalesArrangementId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.SalesArrangementIdIsEmpty);
    }
}
