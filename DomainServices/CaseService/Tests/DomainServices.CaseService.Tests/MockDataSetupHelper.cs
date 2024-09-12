using CIS.Core.Exceptions;
using Moq;

namespace DomainServices.CaseService.Tests;

internal class MockDataSetupHelper
{
    public static Mock<UserService.Clients.v1.IUserServiceClient> SetupUserService()
    {
        var userService = new Mock<UserService.Clients.v1.IUserServiceClient>();

        userService
            .Setup(s => s.GetUser(It.Is<int>(x => x == 1), It.IsAny<CancellationToken>()))
            .Returns(() => Task.FromResult(new UserService.Clients.Dto.UserDto
            {
                UserId = 1,
                UserIdentifiers = [new SharedTypes.GrpcTypes.UserIdentity("X12345", SharedTypes.Enums.UserIdentitySchemes.KbUid)],
                UserInfo = new()
                {
                    DisplayName = "Test",
                    Cpm = "90999999",
                    Icp = "909999999"
                },
                UserPermissions = [200]
            }));

        return userService;
    }

    public static Mock<SalesArrangementService.Clients.ISalesArrangementServiceClient> SetupSalesArrangementService()
    {
        var salesArrangementService = new Mock<SalesArrangementService.Clients.ISalesArrangementServiceClient>();

        salesArrangementService
            .Setup(s => s.GetProductSalesArrangements(It.Is<long>(x => x == 1), It.IsAny<CancellationToken>()))
            .Returns(() => Task.FromResult(new List<SalesArrangementService.Contracts.GetProductSalesArrangementsResponse.Types.SalesArrangement>
            {
                new() { SalesArrangementId = SalesArrangementIdCase1 }
            }));

        salesArrangementService
            .Setup(s => s.GetProductSalesArrangements(It.Is<long>(x => x == 3), It.IsAny<CancellationToken>()))
            .Returns(() => Task.FromResult(new List<SalesArrangementService.Contracts.GetProductSalesArrangementsResponse.Types.SalesArrangement>
            {
                new() { SalesArrangementId = SalesArrangementIdCase3 }
            }));

        salesArrangementService
            .Setup(s => s.GetProductSalesArrangements(It.Is<long>(x => x == 4), It.IsAny<CancellationToken>()))
            .Returns(() => Task.FromResult(new List<SalesArrangementService.Contracts.GetProductSalesArrangementsResponse.Types.SalesArrangement>
            {
                new() { SalesArrangementId = SalesArrangementIdCase4 }
            }));

        salesArrangementService
            .Setup(s => s.GetProductSalesArrangements(It.Is<long>(x => x == 5), It.IsAny<CancellationToken>()))
            .Returns(() => Task.FromResult(new List<SalesArrangementService.Contracts.GetProductSalesArrangementsResponse.Types.SalesArrangement>
            {
                new() { SalesArrangementId = SalesArrangementIdCase5 }
            }));

        salesArrangementService
            .Setup(s => s.GetSalesArrangement(It.Is<int>(x => x == SalesArrangementIdCase3), It.IsAny<CancellationToken>()))
            .Returns(() => Task.FromResult(new SalesArrangementService.Contracts.SalesArrangement
            {
                SalesArrangementId = SalesArrangementIdCase3
            }));

        salesArrangementService
            .Setup(s => s.GetSalesArrangement(It.Is<int>(x => x == SalesArrangementIdCase4), It.IsAny<CancellationToken>()))
            .Returns(() => Task.FromResult(new SalesArrangementService.Contracts.SalesArrangement
            {
                SalesArrangementId = SalesArrangementIdCase3
            }));

        salesArrangementService
            .Setup(s => s.GetSalesArrangement(It.Is<int>(x => x == SalesArrangementIdCase5), It.IsAny<CancellationToken>()))
            .Returns(() => Task.FromResult(new SalesArrangementService.Contracts.SalesArrangement
            {
                SalesArrangementId = SalesArrangementIdCase3,
                RiskBusinessCaseId = "123"
            }));

        return salesArrangementService;
    }

    public static Mock<CodebookService.Clients.ICodebookServiceClient> SetupCodebookService()
    {
        var codebookService = new Mock<CodebookService.Clients.ICodebookServiceClient>();

        codebookService
            .Setup(s => s.ProductTypes(It.IsAny<CancellationToken>()))
            .Returns(() => Task.FromResult(new List<CodebookService.Contracts.v1.ProductTypesResponse.Types.ProductTypeItem>
            {
                new() { Id = 20001, MandantId = 2 },
                new() { Id = 20002, MandantId = 1 }
            }));

        codebookService
            .Setup(s => s.CaseStates(It.IsAny<CancellationToken>()))
            .Returns(() => Task.FromResult(new List<CodebookService.Contracts.v1.GenericCodebookResponse.Types.GenericCodebookItem>
            {
                new() { Id = 1, Name = "State 1", IsDefault = true },
                new() { Id = 2, Name = "State 2" }
            }));

        return codebookService;
    }

    public const int SalesArrangementIdCase1 = 88;
    public const int SalesArrangementIdCase3 = 86;
    public const int SalesArrangementIdCase4 = 87;
    public const int SalesArrangementIdCase5 = 85;
}
