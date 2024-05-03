using CIS.Core.Security;
using DomainServices.ProductService.Contracts;
using NOBY.Infrastructure.ErrorHandling;
using NOBY.Infrastructure.Security;

namespace NOBY.Services.CreateCaseFromExternalSources;

// CreateCaseInNobyFromKonsDB
[TransientService, SelfService]
public sealed class CreateCaseFromExternalSourcesService(
    ICurrentUserAccessor _currentUser,
    DomainServices.CustomerService.Clients.ICustomerServiceClient _customerService,
    DomainServices.CodebookService.Clients.ICodebookServiceClient _codebookService,
    DomainServices.ProductService.Clients.IProductServiceClient _productService,
    DomainServices.CaseService.Clients.v1.ICaseServiceClient _caseService,
    DomainServices.SalesArrangementService.Clients.ISalesArrangementServiceClient _salesArrangementService)
{
    public async Task CreateCase(long caseId, CancellationToken cancellationToken)
    {
        var mortgageInstance = (await _productService.GetMortgage(caseId, cancellationToken)).Mortgage;
        var productType = (await _codebookService.ProductTypes(cancellationToken))
            .FirstOrDefault(t => t.Id == mortgageInstance.ProductTypeId);
        
        if (productType?.MandantId != (int)Mandants.Kb || !validateMortgageData(mortgageInstance))
        {
            throw new NobyValidationException(90045);
        }

        // stav Case
        int caseState = getState(mortgageInstance);
        
        // kontrola na uzivatele a stav
        SecurityHelpers.CheckCaseOwnerAndState(_currentUser, Convert.ToInt32(mortgageInstance.CaseOwnerUserCurrentId.GetValueOrDefault()), caseState);

        // update PCPID pokud neexistuje
        string? pcpId = mortgageInstance.PcpId;
        if (string.IsNullOrEmpty(mortgageInstance.PcpId))
        {
            pcpId = await _productService.UpdateMortgagePcpId(caseId, cancellationToken);
        }

        // instance uzivatele
        var customerIdentity =  new SharedTypes.GrpcTypes.Identity(mortgageInstance.PartnerId, IdentitySchemes.Mp);
        var customer = await _customerService.GetCustomerDetail(customerIdentity, cancellationToken);

        // prioritne chceme pouzit customera z CM
        var kbIdentity = customer.Identities.FirstOrDefault(t => t.IdentityScheme == SharedTypes.GrpcTypes.Identity.Types.IdentitySchemes.Kb);
        if (productType.MandantId == (int)Mandants.Kb && kbIdentity is not null)
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
            PcpId = pcpId,
            State = (int)SalesArrangementStates.InApproval
        };
        await _salesArrangementService.CreateSalesArrangement(saRequest, cancellationToken);

        // update active task - pozor, spravne se pouze vola getTaskList, tim se uvnitr Case service updatuji tasky
        await _caseService.GetTaskList(caseId, cancellationToken);
    }

    private static bool validateMortgageData(MortgageData mortgageData)
    {
        return mortgageData.MortgageState.HasValue
            && mortgageData.CaseOwnerUserCurrentId.HasValue
            && mortgageData.PartnerId > 0
            && !string.IsNullOrEmpty(mortgageData.ContractNumber)
            && mortgageData.ProductTypeId > 0
            && !string.IsNullOrEmpty(mortgageData.PcpId);
    }

    private static int getState(MortgageData mortgageInstance)
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
