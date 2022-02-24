using DomainServices.SalesArrangementService.Api.Repositories.Entities;
using DomainServices.SalesArrangementService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Handlers;

internal class CreateCustomerHandler
    : IRequestHandler<Dto.CreateCustomerMediatrRequest, CreateCustomerResponse>
{
    public async Task<CreateCustomerResponse> Handle(Dto.CreateCustomerMediatrRequest request, CancellationToken cancellation)
    {
        _logger.RequestHandlerStartedWithId(nameof(CreateCustomerHandler), request.Request.SalesArrangementId);
        
        // check existing SalesArrangementId
        await _saRepository.GetSalesArrangement(request.Request.SalesArrangementId, cancellation);
        
        // check customer role
        if (!(await _codebookService.CustomerRoles(cancellation)).Any(t => t.Id == request.Request.CustomerRoleId))
#pragma warning disable CA2208
            throw new CisArgumentException(16021, $"CustomerRoleId {request.Request.CustomerRoleId} does not exist.", "CustomerRoleId");
#pragma warning restore CA2208
        
        // ulozit customera do databaze
        var entity = new Repositories.Entities.CustomerOnSA
        {
            SalesArrangementId = request.Request.SalesArrangementId,
            CustomerRoleId = request.Request.CustomerRoleId,
            HasPartner = request.Request.HasPartner,
            DateOfBirthNaturalPerson = request.Request.DateOfBirthNaturalPerson,
            FirstNameNaturalPerson = request.Request.FirstNameNaturalPerson,
            Name = request.Request.Name
        };
        // identity
        if (request.Request.CustomerIdentifiers is not null)
            entity.Identities.AddRange(request.Request.CustomerIdentifiers.Select(t => new CustomerOnSAIdentity() { Id = t.IdentityId, IdentityScheme = (CIS.Foms.Enums.IdentitySchemes)(int)t.IdentityScheme }));
        
        int customerId = await _repository.CreateCustomer(entity, cancellation);

        _logger.EntityCreated(nameof(Repositories.Entities.CustomerOnSA), customerId);
        
        return new CreateCustomerResponse()
        {
            CustomerOnSAId = customerId
        };
    }
    
    private readonly Repositories.CustomerOnSAServiceRepository _repository;
    private readonly Repositories.SalesArrangementServiceRepository _saRepository;
    private readonly ILogger<CreateCustomerHandler> _logger;
    private readonly CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;
    
    public CreateCustomerHandler(
        CodebookService.Abstraction.ICodebookServiceAbstraction codebookService,
        Repositories.CustomerOnSAServiceRepository repository,
        Repositories.SalesArrangementServiceRepository saRepository,
        ILogger<CreateCustomerHandler> logger)
    {
        _codebookService = codebookService;
        _repository = repository;
        _saRepository = saRepository;
        _logger = logger;
    }
}