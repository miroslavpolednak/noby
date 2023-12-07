using SharedTypes.Enums;
using SharedTypes.GrpcTypes;
using DomainServices.CodebookService.Clients;
using DomainServices.CustomerService.Clients;
using DomainServices.HouseholdService.Clients;
using DomainServices.HouseholdService.Contracts;
using DomainServices.ProductService.Clients;
using DomainServices.SalesArrangementService.Clients;
using NOBY.Api.Endpoints.Customer.CreateCustomer.Dto;
using Mandants = SharedTypes.GrpcTypes.Mandants;
using CIS.Infrastructure.CisMediatR.Rollback;
using NOBY.Api.Endpoints.Customer.Shared;

namespace NOBY.Api.Endpoints.Customer.CreateCustomer;

internal sealed class CreateCustomerHandler
    : IRequestHandler<CreateCustomerRequest, CreateCustomerResponse>
{
    public async Task<CreateCustomerResponse> Handle(CreateCustomerRequest request, CancellationToken cancellationToken)
    {
        await _customerService.ValidateMobilePhone(request.Contacts?.MobilePhone, cancellationToken);
        await _customerService.ValidateEmail(request.Contacts?.EmailAddress, cancellationToken);

        var customerOnSA = await _customerOnSAService.GetCustomer(request.CustomerOnSAId, cancellationToken);
        var customerOnSaList = await _customerOnSAService.GetCustomerList(customerOnSA.SalesArrangementId, cancellationToken);

        // SA
        var saInstance = await _salesArrangementService.GetSalesArrangement(customerOnSA.SalesArrangementId, cancellationToken);
        var saCategory = (await _codebookService.SalesArrangementTypes(cancellationToken)).First(t => t.Id == saInstance.SalesArrangementTypeId);

        if (saCategory.SalesArrangementCategory == (int)SalesArrangementCategories.ProductRequest && customerOnSA.CustomerRoleId != (int)CustomerRoles.Debtor)
        {
            if (!customerOnSaList.Any(c => c.CustomerRoleId == (int)CustomerRoles.Debtor && c.CustomerIdentifiers.Any(i => i.IdentityScheme == Identity.Types.IdentitySchemes.Kb)))
                throw new NobyValidationException(90001, "Main customer is not identified");
        }

        // vytvorit customera v CM
        long kbId;
        var isVerified = false;
        var resultCode = ResultCode.Created;
        try
        {
            var createResult = await _customerService.CreateCustomer(request.ToDomainService(Mandants.Kb), cancellationToken);
            kbId = createResult.CreatedCustomerIdentity.IdentityId;

            isVerified = !request.HardCreate && createResult.IsVerified;
        }
        // V případě, že existoval jeden klient
        catch (CisValidationException ex) when (ex.Errors[0].ExceptionCode == "11023")
        {
            _logger.LogInformation("CreateCustomer: client found {KBID}", ex.Message);

            kbId = long.Parse(ex.Message, System.Globalization.CultureInfo.InvariantCulture);
            resultCode = ResultCode.Identified;

            if (customerOnSaList.Any(c => c.CustomerIdentifiers.Any(i => i.IdentityScheme == Identity.Types.IdentitySchemes.Kb && i.IdentityId == kbId)))
                throw new NobyValidationException(90001, $"Customer with Kb ID {kbId} already exists on SA");
        }
        // Více klientů
        catch (CisValidationException ex) when (ex.Errors[0].ExceptionCode == "11024")
        {
            _logger.LogInformation("CreateCustomer: more clients found", ex);
            throw new NobyValidationException(90006, 409);
        }
        // Registry nefungují
        catch (CisValidationException ex) when (ex.Errors[0].ExceptionCode == "11025")
        {
            _logger.LogInformation("CreateCustomer: registry failed", ex);
            throw new NobyValidationException(90007);
        }
        catch (CisValidationException ex) when (ex.Errors[0].ExceptionCode == "11026")
        {
            _logger.LogInformation("CreateCustomer: registry failed", ex);
            throw new NobyValidationException(90008, 500);
        }
        catch
        {
            // rethrow? nevim co tady...
            throw;
        }

        // KB customer
        var customerKb = await _customerService.GetCustomerDetail(new Identity
        {
            IdentityId = kbId,
            IdentityScheme = Identity.Types.IdentitySchemes.Kb
        }, cancellationToken);

        // update customerOnSA. Dostanu nove PartnerId
        var updateResponse = await _customerOnSAService.UpdateCustomer(customerOnSA.ToUpdateRequest(customerKb), cancellationToken);
        _bag.Add(CreateCustomerRollback.BagKeyCustomerOnSA, customerOnSA);

        // vytvorit response z API
        var model = customerKb.ToResponseDto(isVerified, resultCode);

        if (customerOnSA.CustomerRoleId == (int)CustomerRoles.Debtor)
        {
            await _createProductTrain.Run(saInstance.CaseId, customerOnSA.SalesArrangementId, request.CustomerOnSAId, updateResponse.CustomerIdentifiers, cancellationToken);
        }
        else
        {
            // pokud je vse OK, zalozit customera v konsDb
            try
            {
                await _createOrUpdateCustomerKonsDb.CreateOrUpdate(updateResponse.CustomerIdentifiers, cancellationToken);

                var relationshipTypeId = customerOnSA.CustomerRoleId == (int)CustomerRoles.Codebtor ? 2 : 0;
                var partnerId = updateResponse.CustomerIdentifiers.First(c => c.IdentityScheme == Identity.Types.IdentitySchemes.Mp).IdentityId;
                await _productService.CreateContractRelationship(partnerId, saInstance.CaseId, relationshipTypeId, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Can not create customer in KonsDB", ex);
            }
        }

        if (saCategory.SalesArrangementCategory == (int)SalesArrangementCategories.ProductRequest)
        {
            var households = await _householdService.GetHouseholdList(saInstance.SalesArrangementId, cancellationToken);
            var household = households.First(t => t.CustomerOnSAId1 == customerOnSA.CustomerOnSAId || t.CustomerOnSAId2 == customerOnSA.CustomerOnSAId);

            await UpdateFlowSwitches(household, customerOnSaList, customerOnSA.CustomerOnSAId, cancellationToken);
        }

        return model;
    }

    private async Task UpdateFlowSwitches(DomainServices.HouseholdService.Contracts.Household household, ICollection<CustomerOnSA> customerDetails, int customerOnSAId, CancellationToken cancellationToken)
    {
        // druhy klient v domacnosti
        var secondCustomerOnHouseholdId = household.CustomerOnSAId1 == customerOnSAId ? household.CustomerOnSAId2 : household.CustomerOnSAId1;
        if (!secondCustomerOnHouseholdId.HasValue || isIdentified())
        {
            var flowSwitchId = household.HouseholdTypeId switch
            {
                (int)HouseholdTypes.Main => FlowSwitches.CustomerIdentifiedOnMainHousehold,
                (int)HouseholdTypes.Codebtor => FlowSwitches.CustomerIdentifiedOnCodebtorHousehold,
                _ => throw new NobyValidationException("Unsupported HouseholdType")
            };

            await _salesArrangementService.SetFlowSwitch(household.SalesArrangementId, flowSwitchId, true, cancellationToken);
        }

        bool isIdentified()
        {
            return customerDetails
                   .First(t => t.CustomerOnSAId == secondCustomerOnHouseholdId)
                   .CustomerIdentifiers?
                   .Any(t => t.IdentityScheme == SharedTypes.GrpcTypes.Identity.Types.IdentitySchemes.Kb && t.IdentityId > 0)
                   ?? false;
        }
    }

    private readonly IRollbackBag _bag;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly Services.CreateProductTrain.ICreateProductTrainService _createProductTrain;
    private readonly ILogger<CreateCustomerHandler> _logger;
    private readonly ICustomerOnSAServiceClient _customerOnSAService;
    private readonly ICustomerServiceClient _customerService;
    private readonly IProductServiceClient _productService;
    private readonly IHouseholdServiceClient _householdService;
    private readonly ICodebookServiceClient _codebookService;
    private readonly Services.CreateOrUpdateCustomerKonsDb.CreateOrUpdateCustomerKonsDbService _createOrUpdateCustomerKonsDb;

    public CreateCustomerHandler(
        IRollbackBag rollbackBag,
        ISalesArrangementServiceClient salesArrangementService,
        Services.CreateProductTrain.ICreateProductTrainService createProductTrain,
        ICustomerOnSAServiceClient customerOnSAService,
        ICustomerServiceClient customerService,
        IProductServiceClient productService,
        IHouseholdServiceClient householdService,
        ICodebookServiceClient codebookService,
        Services.CreateOrUpdateCustomerKonsDb.CreateOrUpdateCustomerKonsDbService createOrUpdateCustomerKonsDb,
        ILogger<CreateCustomerHandler> logger)
    {
        _bag = rollbackBag;
        _salesArrangementService = salesArrangementService;
        _createProductTrain = createProductTrain;
        _customerOnSAService = customerOnSAService;
        _customerService = customerService;
        _productService = productService;
        _householdService = householdService;
        _codebookService = codebookService;
        _createOrUpdateCustomerKonsDb = createOrUpdateCustomerKonsDb;
        _logger = logger;
    }
}
