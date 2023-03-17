using CIS.Infrastructure.BackgroundServiceJob;

namespace DomainServices.SalesArrangementService.Api.BackgroundServices.OfferGuaranteeDateToCheck;

public class OfferGuaranteeDateToCheckConfiguration
     : IPeriodicJobConfiguration<OfferGuaranteeDateToCheckService>
{
    public string SectionName => "OfferGuaranteeDateToCheckConfiguration";

    public bool ServiceDisabled { get; set; }

    public TimeSpan TickInterval { get; set; } = TimeSpan.FromHours(16);
}
