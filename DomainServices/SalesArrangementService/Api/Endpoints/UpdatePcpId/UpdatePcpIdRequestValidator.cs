using CIS.Infrastructure.CisMediatR.GrpcValidation;
using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Endpoints.UpdatePcpId;

internal sealed class UpdatePcpIdRequestValidator
    : AbstractValidator<Contracts.UpdatePcpIdRequest>
{
    public UpdatePcpIdRequestValidator()
    {
        RuleFor(t => t.SalesArrangementId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.SalesArrangementIdIsEmpty);

        RuleFor(t => t.PcpId)
            .NotEmpty()
            .WithErrorCode(ErrorCodeMapper.PcpIdIsEmpty);
    }
}
