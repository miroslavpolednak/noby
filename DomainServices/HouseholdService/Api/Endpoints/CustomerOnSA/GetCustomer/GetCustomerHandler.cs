using SharedTypes.GrpcTypes;
using DomainServices.HouseholdService.Api.Database;
using DomainServices.HouseholdService.Contracts;
using SharedComponents.DocumentDataStorage;
using DomainServices.HouseholdService.Api.Database.DocumentDataEntities.Mappers;

namespace DomainServices.HouseholdService.Api.Endpoints.CustomerOnSA.GetCustomer;

internal sealed class GetCustomerHandler
    : IRequestHandler<GetCustomerRequest, Contracts.CustomerOnSA>
{
    public async Task<Contracts.CustomerOnSA> Handle(GetCustomerRequest request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Customers
            .Include(t => t.Identities)
            .AsNoTracking()
            .Where(t => t.CustomerOnSAId == request.CustomerOnSAId)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new CisNotFoundException(ErrorCodeMapper.CustomerOnSANotFound, ErrorCodeMapper.GetMessage(ErrorCodeMapper.CustomerOnSANotFound, request.CustomerOnSAId));

        var customerInstance = new Contracts.CustomerOnSA
        {
            CustomerOnSAId = entity.CustomerOnSAId,
            SalesArrangementId = entity.SalesArrangementId,
            CaseId = entity.CaseId,
            Name = entity.Name,
            FirstNameNaturalPerson = entity.FirstNameNaturalPerson,
            DateOfBirthNaturalPerson = entity.DateOfBirthNaturalPerson,
            CustomerRoleId = (int)entity.CustomerRoleId,
            LockedIncomeDateTime = entity.LockedIncomeDateTime,
            MaritalStatusId = entity.MaritalStatusId,
            CustomerChangeData = entity.ChangeData
        };

        // document data
        var additionalData = await _documentDataStorage.FirstOrDefaultByEntityId<Database.DocumentDataEntities.CustomerOnSAData>(request.CustomerOnSAId, cancellationToken);
        var (customerAdditionalData, customerChangeMetadata) = _customerMapper.MapFromDataToSingle(additionalData?.Data);

        customerInstance.CustomerAdditionalData = customerAdditionalData;
        customerInstance.CustomerChangeMetadata = customerChangeMetadata;

        // identity
        if (entity.Identities is not null)
        {
            customerInstance.CustomerIdentifiers.AddRange(entity.Identities.Select(t => new Identity(t.IdentityId, t.IdentityScheme)));
        }

        // obligations
        var obligations = await _documentDataStorage.GetList<Database.DocumentDataEntities.Obligation>(request.CustomerOnSAId, cancellationToken);
        customerInstance.Obligations.AddRange(obligations.Select(t => _obligationMapper.MapFromDataToList(t)));

        // incomes
        var incomes = await _documentDataStorage.GetList<Database.DocumentDataEntities.Income>(request.CustomerOnSAId, cancellationToken);
        customerInstance.Incomes.AddRange(incomes.Select(t => _incomeMapper.MapFromDataToList(t)));

        return customerInstance;
    }

    private readonly CustomerOnSADataMapper _customerMapper;
    private readonly IncomeMapper _incomeMapper;
    private readonly ObligationMapper _obligationMapper;
    private readonly IDocumentDataStorage _documentDataStorage;
    private readonly HouseholdServiceDbContext _dbContext;

    public GetCustomerHandler(
        CustomerOnSADataMapper customerMapper,
        HouseholdServiceDbContext dbContext, 
        IDocumentDataStorage documentDataStorage,
        IncomeMapper incomeMapper,
        ObligationMapper obligationMapper)
    {
        _customerMapper = customerMapper;
        _documentDataStorage = documentDataStorage;
        _dbContext = dbContext;
        _incomeMapper = incomeMapper;
        _obligationMapper = obligationMapper;
    }
}