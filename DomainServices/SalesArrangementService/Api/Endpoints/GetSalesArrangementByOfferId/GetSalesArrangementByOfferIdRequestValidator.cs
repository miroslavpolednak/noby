using CIS.Infrastructure.CisMediatR.GrpcValidation;
using FluentValidation;

namespace DomainServices.SalesArrangementService.Api.Endpoints.GetSalesArrangementByOfferId;

internal sealed class GetSalesArrangementByOfferIdRequestValidator
    : AbstractValidator<Contracts.GetSalesArrangementByOfferIdRequest>
{
    public GetSalesArrangementByOfferIdRequestValidator()
    {
        RuleFor(t => t.OfferId)
            .GreaterThan(0)
            .WithErrorCode(ErrorCodeMapper.OfferIdIsEmpty);
    }
}