using CIS.Core.Exceptions;
using CIS.Core.Results;
using DomainServices.OfferService.Abstraction;
using FOMS.Api.Endpoints.Savings.Offer.Dto;

namespace FOMS.Api.Endpoints.Savings.Offer;

internal class CreateCaseHandler
    : IRequestHandler<CreateCaseRequest, int>
{
    public async Task<int> Handle(CreateCaseRequest request, CancellationToken cancellationToken)
    {
        // vytvorit case
        var caseModel = new DomainServices.CaseService.Contracts.CreateCaseRequest()
        {
            PartyId = _userAccessor.User.Id,
            ProductInstanceType = 1,
            DateOfBirthNaturalPerson = request.Request.DateOfBirth,
            FirstNameNaturalPerson = request.Request.FirstName,
            Name = request.Request.LastName
        };
        _logger.LogInformation("Create case with {model}", caseModel);
        long caseId = resolveCaseResult(await _caseService.CreateCase(caseModel));

        // vytvorit zadost
        //_salesArrangementService.CreateSalesArrangement(caseId, _configuration.Savings.SavingsProductInstanceType, )

        return 1;
    }

    private long resolveCaseResult(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult<long> r => r.Model,
            SimulationServiceErrorResult e1 => throw new CIS.Core.Exceptions.CisValidationException(e1.Errors),
            ErrorServiceCallResult e2 => throw new CIS.Core.Exceptions.CisValidationException(e2.Errors),
            _ => throw new NotImplementedException()
        };

    private readonly DomainServices.CaseService.Abstraction.ICaseServiceAbstraction _caseService;
    private readonly DomainServices.CaseService.Abstraction.ISalesArrangementServiceAbstraction _salesArrangementService;
    private readonly Infrastructure.Configuration.AppConfiguration _configuration;
    private readonly ILogger<CreateCaseHandler> _logger;
    private readonly CIS.Core.Security.ICurrentUserAccessor _userAccessor;

    public CreateCaseHandler(
        CIS.Core.Security.ICurrentUserAccessor userAccessor,
        ILogger<CreateCaseHandler> logger,
        Infrastructure.Configuration.AppConfiguration configuration,
        DomainServices.CaseService.Abstraction.ICaseServiceAbstraction caseService, 
        DomainServices.CaseService.Abstraction.ISalesArrangementServiceAbstraction salesArrangementService)
    {
        _userAccessor = userAccessor;
        _logger = logger;
        _configuration = configuration;
        _caseService = caseService;
        _salesArrangementService = salesArrangementService;
    }
}
