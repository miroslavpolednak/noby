using DomainServices.OfferService.Contracts;
using DomainServices.SalesArrangementService.Clients;
using DomainServices.SalesArrangementService.Contracts;
using NOBY.Infrastructure.ErrorHandling;

namespace NOBY.Services.OfferLink;

public class MortgageOfferLinkValidator
{
    public delegate Task<bool> AdditionalValidator(SalesArrangement salesArrangement, GetOfferResponse offer, CancellationToken cancellationToken);

    public required SalesArrangementTypes SalesArrangementType { get; init; }

    public required OfferTypes OfferType { get; init; }

    public AdditionalValidator AdditionalValidation { get; init; } = EmptyAdditionalValidation;

    public List<EnumSalesArrangementStates> ValidStates { get; init; } = [EnumSalesArrangementStates.InProgress, EnumSalesArrangementStates.NewArrangement];

    public async Task Validate(SalesArrangement salesArrangement, GetOfferResponse offer, CancellationToken cancellationToken)
    {
        if (salesArrangement.SalesArrangementTypeId != (int)SalesArrangementType || offer.Data.OfferType != OfferType)
            throw new NobyValidationException(90032);

        if (!salesArrangement.IsInState(ValidStates))
            throw new NobyValidationException(90032);

        if (await AdditionalValidation(salesArrangement, offer, cancellationToken))
            return;

        throw new NobyValidationException(90032);
    }

    private static Task<bool> EmptyAdditionalValidation(SalesArrangement salesArrangement, GetOfferResponse offer, CancellationToken cancellationToken) => Task.FromResult(true);
}