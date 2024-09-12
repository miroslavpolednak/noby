using CIS.Core.Security;
using DomainServices.CaseService.Api;
using DomainServices.CaseService.Api.Endpoints.v1.CreateCase;
using DomainServices.CaseService.Contracts;
using DomainServices.CodebookService.Clients;
using ExternalServices.Eas.V1;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace DomainServices.CaseService.Tests.EndpointsTests;

public class CreateCaseHandlerTests
{
    private readonly Mock<IMediator> _mediator;
    private readonly Mock<UserService.Clients.v1.IUserServiceClient> _userService;
    private readonly Mock<IEasClient> _easClient;
    private readonly Mock<ICodebookServiceClient> _codebookService;
    private readonly Mock<ICurrentUserAccessor> _currentUser;
    private readonly ILogger<CreateCaseHandler> _logger;

    public CreateCaseHandlerTests()
    {
        ErrorCodeMapper.Init();

        _logger = new NullLogger<CreateCaseHandler>();
        _currentUser = new Mock<ICurrentUserAccessor>();
        _mediator = new Mock<IMediator>();
        _codebookService = MockDataSetupHelper.SetupCodebookService();
        _userService = MockDataSetupHelper.SetupUserService();
        _easClient = new Mock<IEasClient>();
    }

    [Fact]
    public async Task Handle_Should_Throw_AlreadyExists()
    {
        var request = createRequest();
        setupExternalData();

        using var dbContext = DatabaseHelpers.CreateDbContext(_currentUser.Object);
        dbContext.Cases.Add(DatabaseHelpers.CreateEntity(_newCaseId, EnumCaseStates.InProgress));
        dbContext.SaveChanges();
        dbContext.ChangeTracker.Clear();

        var handler = createHandler(dbContext);

        // vraci se ruzne chyby pro MS SQL a InMemory
        await Assert.ThrowsAnyAsync<Exception>(() => handler.Handle(request, default));
    }

    [Fact]
    public async Task Handle_Should_Return_CaseId()
    {
        var request = createRequest();
        setupExternalData();

        using var dbContext = DatabaseHelpers.CreateDbContext(_currentUser.Object);
        var handler = createHandler(dbContext);
        var result = await handler.Handle(request, default);

        result.CaseId.Should().Be(_newCaseId);

        Assert.True(dbContext.Cases.Any(c => c.CaseId == _newCaseId));

        _easClient.Verify(v =>
            v.GetCaseId(It.Is<IdentitySchemes>(x => x == IdentitySchemes.Kb), It.Is<int>(x => x == request.Data.ProductTypeId), It.IsAny<CancellationToken>()),
            Times.Once);

        _mediator.Verify(v =>
            v.Send(It.Is<NotifyStarbuildRequest>(x => x.CaseId == _newCaseId && x.SkipRiskBusinessCaseId), It.IsAny<CancellationToken>()),
            Times.Once);

        // kontrola entity
        var newEntity = dbContext.Cases.First(c => c.CaseId == _newCaseId);

        newEntity.ProductTypeId.Should().Be(request.Data.ProductTypeId);
        newEntity.TargetAmount.Should().Be(request.Data.TargetAmount);
        newEntity.ContractNumber.Should().Be(request.Data.ContractNumber);
        newEntity.OwnerUserId.Should().Be(request.CaseOwnerUserId);

        newEntity.EmailForOffer.Should().Be(request.OfferContacts.EmailForOffer);
        newEntity.PhoneNumberForOffer.Should().Be(request.OfferContacts.PhoneNumberForOffer.PhoneNumber);
        newEntity.PhoneIDCForOffer.Should().Be(request.OfferContacts.PhoneNumberForOffer.PhoneIDC);

        newEntity.DateOfBirthNaturalPerson.Should().Be(request.Customer.DateOfBirthNaturalPerson);
        newEntity.FirstNameNaturalPerson.Should().Be(request.Customer.FirstNameNaturalPerson);
        newEntity.Name.Should().Be(request.Customer.Name);
        newEntity.Cin.Should().Be(request.Customer.Cin);

        newEntity.CustomerChurnRisk.Should().Be(request.Customer.CustomerChurnRisk);
        newEntity.CustomerPriceSensitivity.Should().Be(request.Customer.CustomerPriceSensitivity);
        newEntity.CustomerIdentityScheme.Should().Be((IdentitySchemes)request.Customer.Identity.IdentityScheme);
        newEntity.CustomerIdentityId.Should().Be(request.Customer.Identity.IdentityId);
    }

    private void setupExternalData()
    {
        _easClient
            .Setup(s => s.GetCaseId(It.IsAny<IdentitySchemes>(), It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_newCaseId);
    }

    private static CreateCaseRequest createRequest()
    {
        return new()
        {
            CaseOwnerUserId = 1,
            Data = new()
            {
                ProductTypeId = 20001,
                TargetAmount = 1000000,
                ContractNumber = "888999111"
            },
            OfferContacts = new()
            {
                EmailForOffer = "aaa@bbb.cz",
                PhoneNumberForOffer = new()
                {
                    PhoneIDC = "420",
                    PhoneNumber = "123456789"
                }
            },
            Customer = new CustomerData()
            {
                DateOfBirthNaturalPerson = DateTime.Now.AddYears(-30),
                FirstNameNaturalPerson = "Test",
                Name = "Test",
                Cin = "1234567890",
                CustomerChurnRisk = 555,
                CustomerPriceSensitivity = 666,
                Identity = new()
                {
                    IdentityScheme = SharedTypes.GrpcTypes.Identity.Types.IdentitySchemes.Kb,
                    IdentityId = 22243455
                }
            }
        };
    }

    private CreateCaseHandler createHandler(Api.Database.CaseServiceDbContext dbContext)
    {
        return new CreateCaseHandler(_mediator.Object, _userService.Object, _codebookService.Object, _easClient.Object, dbContext, _logger, TimeProvider.System);
    }

    private const long _newCaseId = 1099;
}
