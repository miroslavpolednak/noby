using CIS.Core.Security;
using DomainServices.ProductService.Contracts;
using NOBY.Infrastructure.ErrorHandling;
using NOBY.Infrastructure.Security;

namespace NOBY.Services.CreateCaseFromExternalSources;

// CreateCaseInNobyFromKonsDB
[TransientService, SelfService]
public sealed class CreateCaseFromExternalSourcesService(
    ICurrentUserAccessor _currentUser,
    DomainServices.CustomerService.Clients.v1.ICustomerServiceClient _customerService,
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
        var caseState = getState(mortgageInstance);
        
        // kontrola na uzivatele a stav
        SecurityHelpers.CheckCaseOwnerAndState(_currentUser, Convert.ToInt32(mortgageInstance.CaseOwnerUserCurrentId.GetValueOrDefault()), caseState, null);

        // instance uzivatele
        var mpIdentity =  new SharedTypes.GrpcTypes.Identity(mortgageInstance.PartnerId, IdentitySchemes.Mp);
        var customer = await _customerService.GetCustomerDetail(mpIdentity, cancellationToken);

		// prioritne chceme pouzit customera z CM
        var kbIdentity = customer.Identities.GetKbIdentityOrDefault();
        if (productType.MandantId == (int)Mandants.Kb && kbIdentity is not null)
        {
            customer = await _customerService.GetCustomerDetail(kbIdentity, cancellationToken);	
		}

		// update PCPID pokud neexistuje
		string? pcpId = mortgageInstance.PcpId;
		if (string.IsNullOrEmpty(mortgageInstance.PcpId) && kbIdentity is not null)
		{
			pcpId = await _productService.UpdateMortgagePcpId(new UpdateMortgagePcpIdRequest
			{
				Identity = kbIdentity,
				ProductId = caseId,
				ProductTypeId = mortgageInstance.ProductTypeId
			}, cancellationToken);
		}

		// vytvorit case
		var createCaseRequest = new DomainServices.CaseService.Contracts.CreateExistingCaseRequest
        {
            CaseId = caseId,
            State = (int)caseState,
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
                Identity = kbIdentity ?? mpIdentity,
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
            State = (int)EnumSalesArrangementStates.InApproval
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
            && mortgageData.ProductTypeId > 0;
    }

    private static EnumCaseStates getState(MortgageData mortgageInstance)
    {
        return mortgageInstance.MortgageState switch
        {
            0 or 1 or 3 or 4 => EnumCaseStates.InApprovalConfirmed,
            2 => EnumCaseStates.Cancelled,
            5 or 6 or 7 or 8 => EnumCaseStates.Finished,
            9 => EnumCaseStates.InDisbursement,
            10 => EnumCaseStates.InSigning,
            11 => ((DateTime?)mortgageInstance.DrawingFinishedDate).HasValue ? EnumCaseStates.InAdministration : EnumCaseStates.InDisbursement,
            _ => EnumCaseStates.InApprovalConfirmed
        };
    }
}
