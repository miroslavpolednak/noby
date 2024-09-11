using CIS.Core.Security;
using CIS.Infrastructure.CisMediatR.Rollback;
using DomainServices.CaseService.Api;
using DomainServices.CaseService.Api.Endpoints.v1.CreateCase;
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
    private readonly Mock<IRollbackBag> _bag;
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
        _bag = new Mock<IRollbackBag>();
        _codebookService = new Mock<ICodebookServiceClient>();
        _userService = new Mock<UserService.Clients.v1.IUserServiceClient>();
        _easClient = new Mock<IEasClient>();
    }
}
