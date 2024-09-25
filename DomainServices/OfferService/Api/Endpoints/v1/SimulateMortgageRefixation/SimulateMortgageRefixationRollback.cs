using CIS.Infrastructure.CisMediatR.Rollback;
using DomainServices.OfferService.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DomainServices.OfferService.Api.Endpoints.v1.SimulateMortgageRefixation;

internal sealed class SimulateMortgageRefixationRollback(
    IRollbackBag _bag,
    Database.OfferServiceDbContext _dbContext,
    ILogger<SimulateMortgageRefixationRollback> _logger)
    : IRollbackAction<SimulateMortgageRefixationRequest>
{
    public async Task ExecuteRollback(Exception exception, SimulateMortgageRefixationRequest request, CancellationToken cancellationToken = default)
    {
        _logger.RollbackHandlerStarted(nameof(SimulateMortgageRefixationRollback));

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