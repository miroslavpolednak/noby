using CIS.Infrastructure.CisMediatR.GrpcValidation;
using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Endpoints.LinkModelationToSalesArrangement;

internal sealed class LinkModelationToSalesArrangementRequestValidator
    : AbstractValidator<Contracts.LinkModelationToSalesArrangementRequest>
{
    public LinkModelationToSalesArrangementRequestValidator()
    {
        RuleFor(t => t.SalesArrangementId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.SalesArrangementIdIsEmpty);

        RuleFor(t => t.OfferId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.OfferIdIsEmpty);
    }
}
