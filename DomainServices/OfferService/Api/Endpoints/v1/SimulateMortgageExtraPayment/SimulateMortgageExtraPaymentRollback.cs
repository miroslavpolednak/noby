using CIS.Infrastructure.CisMediatR.Rollback;
using DomainServices.OfferService.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.OfferService.Api.Endpoints.v1.SimulateMortgageExtraPayment;

internal sealed class SimulateMortgageExtraPaymentRollback(
    IRollbackBag _bag,
    Database.OfferServiceDbContext _dbContext,
    ILogger<SimulateMortgageExtraPaymentRollback> _logger)
    : IRollbackAction<SimulateMortgageExtraPaymentRequest>
{
    public async Task ExecuteRollback(Exception exception, SimulateMortgageExtraPaymentRequest request, CancellationToken cancellationToken = default)
    {
        _logger.RollbackHandlerStarted(nameof(SimulateMortgageExtraPaymentRollback));

        if (_bag.ContainsKey(BagKeyOfferId))
        {
            var offerId = (int)_bag[BagKeyOfferId]!;

            await _dbContext.Offers.Where(t => t.OfferId == offerId).ExecuteDeleteAsync(cancellationToken);

            _logger.RollbackHandlerStepDone(BagKeyOfferId, _bag[BagKeyOfferId]!);
        }
    }

    public bool OverrideThrownException { get => false; }

    public const string BagKeyOfferId = "BagKeyOfferId";
}
