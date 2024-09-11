using Moq;

namespace DomainServices.CaseService.Tests;

internal class MockDataSetupHelper
{
    public static void SetupSalesArrangementService(Mock<SalesArrangementService.Clients.ISalesArrangementServiceClient> _salesArrangementService)
    {
        _salesArrangementService
            .Setup(s => s.GetProductSalesArrangements(It.Is<long>(x => x == 1), It.IsAny<CancellationToken>()))
            .Returns(() => Task.FromResult(new List<SalesArrangementService.Contracts.GetProductSalesArrangementsResponse.Types.SalesArrangement>
            {
                new() { SalesArrangementId = SalesArrangementIdCase1 }
            }));

        _salesArrangementService
            .Setup(s => s.GetProductSalesArrangements(It.Is<long>(x => x == 3), It.IsAny<CancellationToken>()))
            .Returns(() => Task.FromResult(new List<SalesArrangementService.Contracts.GetProductSalesArrangementsResponse.Types.SalesArrangement>
            {
                new() { SalesArrangementId = SalesArrangementIdCase3 }
            }));

        _salesArrangementService
            .Setup(s => s.GetProductSalesArrangements(It.Is<long>(x => x == 4), It.IsAny<CancellationToken>()))
            .Returns(() => Task.FromResult(new List<SalesArrangementService.Contracts.GetProductSalesArrangementsResponse.Types.SalesArrangement>
            {
                new() { SalesArrangementId = SalesArrangementIdCase4 }
            }));

        _salesArrangementService
            .Setup(s => s.GetProductSalesArrangements(It.Is<long>(x => x == 5), It.IsAny<CancellationToken>()))
            .Returns(() => Task.FromResult(new List<SalesArrangementService.Contracts.GetProductSalesArrangementsResponse.Types.SalesArrangement>
            {
                new() { SalesArrangementId = SalesArrangementIdCase5 }
            }));

        _salesArrangementService
            .Setup(s => s.GetSalesArrangement(It.Is<int>(x => x == SalesArrangementIdCase3), It.IsAny<CancellationToken>()))
            .Returns(() => Task.FromResult(new SalesArrangementService.Contracts.SalesArrangement
            {
                SalesArrangementId = SalesArrangementIdCase3
            }));

        _salesArrangementService
            .Setup(s => s.GetSalesArrangement(It.Is<int>(x => x == SalesArrangementIdCase4), It.IsAny<CancellationToken>()))
            .Returns(() => Task.FromResult(new SalesArrangementService.Contracts.SalesArrangement
            {
                SalesArrangementId = SalesArrangementIdCase3
            }));

        _salesArrangementService
            .Setup(s => s.GetSalesArrangement(It.Is<int>(x => x == SalesArrangementIdCase5), It.IsAny<CancellationToken>()))
            .Returns(() => Task.FromResult(new SalesArrangementService.Contracts.SalesArrangement
            {
                SalesArrangementId = SalesArrangementIdCase3,
                RiskBusinessCaseId = "123"
            }));
    }

    public const int SalesArrangementIdCase1 = 88;
    public const int SalesArrangementIdCase3 = 86;
    public const int SalesArrangementIdCase4 = 87;
    public const int SalesArrangementIdCase5 = 85;
}
