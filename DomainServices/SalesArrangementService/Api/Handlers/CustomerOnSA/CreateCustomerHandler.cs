using DomainServices.SalesArrangementService.Api.Repositories.Entities;
using _SA = DomainServices.SalesArrangementService.Contracts;
using DomainServices.CustomerService.Abstraction;
using CIS.Infrastructure.gRPC.CisTypes;
using _Customer = DomainServices.CustomerService.Contracts;

namespace DomainServices.SalesArrangementService.Api.Handlers.CustomerOnSA;

internal class CreateCustomerHandler
    : IRequestHandler<Dto.CreateCustomerMediatrRequest, _SA.CreateCustomerResponse>
{
    public async Task<_SA.CreateCustomerResponse> Handle(Dto.CreateCustomerMediatrRequest request, CancellationToken cancellation)
    {
        _logger.RequestHandlerStartedWithId(nameof(CreateCustomerHandler), request.Request.SalesArrangementId);
        
        // check existing SalesArrangementId
        await _saRepository.GetSalesArrangement(request.Request.SalesArrangementId, cancellation);

        var entity = new Repositories.Entities.CustomerOnSA
        {
            SalesArrangementId = request.Request.SalesArrangementId,
            CustomerRoleId = (CIS.Foms.Enums.CustomerRoles)request.Request.CustomerRoleId,
            Identities = request.Request.CustomerIdentifiers?.Select(t => new CustomerOnSAIdentity(t)).ToList()
        };

        // pokud se jedna o existujici identitu v KB
        if (request.Request.CustomerIdentifiers is not null && request.Request.CustomerIdentifiers.Any())
        {
            //TODO nepotrebuju asi cely detail, tak by stacila nejaka metoda CustomerExists? Nebo mam propsat udaje z CM nize do CustomerOnSA?
            // pokud klient neexistuje, mela by CustomerService vyhodit vyjimku
            var customerInstance = ServiceCallResult.Resolve<_Customer.CustomerResponse>(await _customerService.GetCustomerDetail(new CustomerService.Contracts.CustomerRequest
            {
                Identity = request.Request.CustomerIdentifiers.First()
            }, cancellation));

            // propsat udaje do customerOnSA
            entity.DateOfBirthNaturalPerson = customerInstance.NaturalPerson?.DateOfBirth;
            entity.FirstNameNaturalPerson = customerInstance.NaturalPerson?.FirstName;
            entity.Name = customerInstance.NaturalPerson?.LastName ?? "";

            // pokud jeste nema modre ID, ale ma cervene
            if (request.Request.CustomerIdentifiers.Any(t => t.IdentityScheme == Identity.Types.IdentitySchemes.Kb) && !request.Request.CustomerIdentifiers.Any(t => t.IdentityScheme == Identity.Types.IdentitySchemes.Mp))
            {
                //TODO jak tady bude vypadat volani pro PO?
                // zavolat EAS
                int? partnerId = resolveCreateEasClient(await _easClient.CreateNewOrGetExisingClient(new ExternalServices.Eas.Dto.ClientDataModel
                {
                    BirthNumber = customerInstance.NaturalPerson!.BirthNumber,
                    FirstName = customerInstance.NaturalPerson.FirstName,
                    LastName = customerInstance.NaturalPerson.LastName,
                    DateOfBirth = customerInstance.NaturalPerson.DateOfBirth
                }));

                // pokud se to povedlo, pridej customerovi modrou identitu
                if (partnerId.HasValue)
                    entity.Identities!.Add(new CustomerOnSAIdentity
                    {
                        IdentityScheme = CIS.Foms.Enums.IdentitySchemes.Mp,
                        CustomerOnSAIdentityId = partnerId.Value
                    });
            }
        }
        else // neexistujici customer
        {
            entity.DateOfBirthNaturalPerson = request.Request.DateOfBirthNaturalPerson;
            entity.FirstNameNaturalPerson = request.Request.FirstNameNaturalPerson;
            entity.Name = request.Request.Name;
        }

        int customerId = await _repository.CreateCustomer(entity, cancellation);
        
        _logger.EntityCreated(nameof(Repositories.Entities.CustomerOnSA), customerId);
        
        return new _SA.CreateCustomerResponse()
        {
            CustomerOnSAId = customerId
        };
    }

    // zalozit noveho klienta v EAS
    private static int? resolveCreateEasClient(IServiceCallResult result) =>
        result switch
        {
            SuccessfulServiceCallResult<ExternalServices.Eas.Dto.CreateNewOrGetExisingClientResponse> r => r.Model.Id,
            ErrorServiceCallResult r => default(int?), //TODO co se ma v tomhle pripade delat?
            _ => throw new NotImplementedException("CreateCustomerWithHouseholdService.resolveCreateClient")
        };

    private readonly Eas.IEasClient _easClient;
    private readonly Repositories.CustomerOnSAServiceRepository _repository;
    private readonly Repositories.SalesArrangementServiceRepository _saRepository;
    private readonly ICustomerServiceAbstraction _customerService;
    private readonly ILogger<CreateCustomerHandler> _logger;
    private readonly CodebookService.Abstraction.ICodebookServiceAbstraction _codebookService;
    
    public CreateCustomerHandler(
        Eas.IEasClient easClient,
        ICustomerServiceAbstraction customerService,
        CodebookService.Abstraction.ICodebookServiceAbstraction codebookService,
        Repositories.CustomerOnSAServiceRepository repository,
        Repositories.SalesArrangementServiceRepository saRepository,
        ILogger<CreateCustomerHandler> logger)
    {
        _easClient = easClient;
        _customerService = customerService;
        _codebookService = codebookService;
        _repository = repository;
        _saRepository = saRepository;
        _logger = logger;
    }
}