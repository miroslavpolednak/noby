using DomainServices.SalesArrangementService.Api.Repositories.Entities;
using DomainServices.SalesArrangementService.Contracts;
using DomainServices.CustomerService.Abstraction;

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
        
        // zkontrolovat zda customer existuje pokud ma danou identitu
        if (request.Request.CustomerIdentifiers is not null && request.Request.CustomerIdentifiers.Any())
        {
            //TODO nepotrebuju asi cely detail, tak by stacila nejaka metoda CustomerExists? Nebo mam propsat udaje z CM nize do CustomerOnSA?
            // pokud klient neexistuje, mela by CustomerService vyhodit vyjimku
            await _customerService.GetCustomerDetail(new CustomerService.Contracts.CustomerRequest
            {
                Identity = request.Request.CustomerIdentifiers.First()
            }, cancellation);
        }

        // ulozit customera do databaze
        var entity = new Repositories.Entities.CustomerOnSA
        {
            SalesArrangementId = request.Request.SalesArrangementId,
            CustomerRoleId = (CIS.Foms.Enums.CustomerRoles)request.Request.CustomerRoleId,
            DateOfBirthNaturalPerson = request.Request.DateOfBirthNaturalPerson,
            FirstNameNaturalPerson = request.Request.FirstNameNaturalPerson,
            Name = request.Request.Name,
            Identities = request.Request.CustomerIdentifiers?.Select(t => new CustomerOnSAIdentity(t)).ToList()
        };
        int customerId = await _repository.CreateCustomer(entity, cancellation);
        
        _logger.EntityCreated(nameof(Repositories.Entities.CustomerOnSA), customerId);
        
        return new CreateCustomerResponse()
        {
            CustomerOnSAId = customerId
        };
    }
    
    private readonly Repositories.CustomerOnSAServiceRepository _repository;
    private readonly Repositories.SalesArrangementServiceRepository _saRepository;
    private readonly ICustomerServiceAbstraction _customerService;
    private readonly ILogger<CreateCustomerHandler> _logger;
    private readonly CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;
    
    public CreateCustomerHandler(
        ICustomerServiceAbstraction customerService,
        CodebookService.Abstraction.ICodebookServiceAbstraction codebookService,
        Repositories.CustomerOnSAServiceRepository repository,
        Repositories.SalesArrangementServiceRepository saRepository,
        ILogger<CreateCustomerHandler> logger)
    {
        _customerService = customerService;
        _codebookService = codebookService;
        _repository = repository;
        _saRepository = saRepository;
        _logger = logger;
    }
}