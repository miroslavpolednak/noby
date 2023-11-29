using CIS.Core.Security;
using DomainServices.ProductService.Contracts;
using NOBY.Infrastructure.Security;

namespace NOBY.Services.CreateCaseFromExternalSources;

[TransientService, SelfService]
public sealed class CreateCaseFromExternalSourcesService
{
    public async Task CreateCase(long caseId, CancellationToken cancellationToken)
    {
        var productTypes = (await _codebookService.ProductTypes(cancellationToken)).Select(t => t.Id);

        var mortgageInstance = (await _productService.GetMortgage(caseId, cancellationToken)).Mortgage;
        if (!productTypes.Any(t => productTypes.Contains(mortgageInstance.ProductTypeId)))
        {
            throw new CisNotFoundException(0, "Product is not KB type");
        }

        // stav Case
        int caseState = getState(mortgageInstance);
        
        // kontrola na uzivatele a stav
        SecurityHelpers.CheckCaseOwnerAndState(_currentUser, Convert.ToInt32(mortgageInstance.CaseOwnerUserCurrentId.GetValueOrDefault()), caseState);

        // instance uzivatele
        var customerIdentity =  new SharedTypes.GrpcTypes.Identity(mortgageInstance.PartnerId, IdentitySchemes.Mp);
        var customer = await _customerService.GetCustomerDetail(customerIdentity, cancellationToken);

        // prioritne chceme pouzit customera z CM
        var kbIdentity = customer.Identities.FirstOrDefault(t => t.IdentityScheme == SharedTypes.GrpcTypes.Identity.Types.IdentitySchemes.Kb);
        if (kbIdentity is not null)
        {
            customer = await _customerService.GetCustomerDetail(kbIdentity, cancellationToken);
        }
        
        // vytvorit case
        var createCaseRequest = new DomainServices.CaseService.Contracts.CreateExistingCaseRequest
        {
            CaseId = caseId,
            State = caseState,
            CaseOwnerUserId = Convert.ToInt32(mortgageInstance.CaseOwnerUserOrigId ?? mortgageInstance.CaseOwnerUserCurrentId, CultureInfo.InvariantCulture),
            Data = new DomainServices.CaseService.Contracts.CaseData
            {
                TargetAmount = (decimal)mortgageInstance.LoanAmount!,
                ContractNumber = mortgageInstance.ContractNumber,
                ProductTypeId = mortgageInstance.ProductTypeId
            },
            Customer = new DomainServices.CaseService.Contracts.CustomerData
            {
                DateOfBirthNaturalPerson = customer.NaturalPerson?.DateOfBirth,
                FirstNameNaturalPerson = customer.NaturalPerson?.FirstName,
                Name = customer.NaturalPerson?.LastName,
                Identity = customerIdentity,
                Cin = customer.NaturalPerson?.BirthNumber
            }
        };
        await _caseService.CreateExistingCase(createCaseRequest, cancellationToken);

        // zalozit SA
        var saRequest = new DomainServices.SalesArrangementService.Contracts.CreateSalesArrangementRequest
        {
            CaseId = caseId,
            ContractNumber = mortgageInstance.ContractNumber,
            SalesArrangementTypeId = 1,
            State = (int)SalesArrangementStates.InApproval
        };
        await _salesArrangementService.CreateSalesArrangement(saRequest, cancellationToken);
    }

    private static int getState(MortgageData mortgageInstance)
    {
        if (mortgageInstance.IsCancelled)
        {
            return 7;
        }
        else
        {
            return mortgageInstance.MortgageState switch
            {
                0 or 1 or 3 or 4 => 8,
                2 => 7,
                5 or 6 or 7 or 8 => 6,
                9 => 4,
                10 => 3,
                11 => ((DateTime?)mortgageInstance.DrawingFinishedDate).HasValue ? 5 : 4,
                _ => 8
            };
        }
    }

    private readonly ICurrentUserAccessor _currentUser;
    private readonly DomainServices.CustomerService.Clients.ICustomerServiceClient _customerService;
    private readonly DomainServices.CodebookService.Clients.ICodebookServiceClient _codebookService;
    private readonly DomainServices.ProductService.Clients.IProductServiceClient _productService;
    private readonly DomainServices.CaseService.Clients.ICaseServiceClient _caseService;
    private readonly DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClient _salesArrangementService;

    public CreateCaseFromExternalSourcesService(
        ICurrentUserAccessor currentUser,
        DomainServices.CustomerService.Clients.ICustomerServiceClient customerService,
        DomainServices.CodebookService.Clients.ICodebookServiceClient codebookService,
        DomainServices.ProductService.Clients.IProductServiceClient productService,
        DomainServices.CaseService.Clients.ICaseServiceClient caseService,
        DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClient salesArrangementService)
    {
        _customerService = customerService;
        _codebookService = codebookService;
        _currentUser = currentUser;
        _productService = productService;
        _caseService = caseService;
        _salesArrangementService = salesArrangementService;
    }
}
