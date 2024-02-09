using CIS.Infrastructure.CisMediatR.GrpcValidation;
using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Endpoints.SendToCmp;

internal sealed class SendToCmpRequestValidator
    : AbstractValidator<Contracts.SendToCmpRequest>
{
    public SendToCmpRequestValidator()
    {
        RuleFor(t => t.SalesArrangementId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.SalesArrangementIdIsEmpty);
    }
}
