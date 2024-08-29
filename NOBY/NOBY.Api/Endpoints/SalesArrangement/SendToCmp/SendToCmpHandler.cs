using SharedTypes.GrpcTypes;
using DomainServices.CaseService.Clients.v1;
using DomainServices.CustomerService.Clients;
using DomainServices.CustomerService.Contracts;
using DomainServices.DocumentOnSAService.Clients;
using DomainServices.HouseholdService.Clients.v1;
using DomainServices.HouseholdService.Contracts;
using DomainServices.ProductService.Clients;
using DomainServices.ProductService.Contracts;
using DomainServices.SalesArrangementService.Clients;
using NOBY.Api.Endpoints.SalesArrangement.GetFlowSwitches;
using NOBY.Api.Endpoints.SalesArrangement.SendToCmp.Dto;
using _SA = DomainServices.SalesArrangementService.Contracts;
using CreateCustomerRequest = DomainServices.CustomerService.Contracts.CreateCustomerRequest;
using Mandants = SharedTypes.GrpcTypes.Mandants;

namespace NOBY.Api.Endpoints.SalesArrangement.SendToCmp;

internal sealed class SendToCmpHandler(
    DomainServices.CodebookService.Clients.ICodebookServiceClient _codebookService,
    ICaseServiceClient _caseService,
    ISalesArrangementServiceClient _salesArrangementService,
    ICustomerOnSAServiceClient _customerOnSaService,
    IProductServiceClient _productService,
    ICustomerServiceClient _customerService,
    IDocumentOnSAServiceClient _documentOnSAService,
    IMediator _mediator)
        : IRequestHandler<SendToCmpRequest>
{
    public async Task Handle(SendToCmpRequest request, CancellationToken cancellationToken)
    {
        // instance SA
        var saInstance = await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken);

        if (saInstance.SalesArrangementTypeId is ((int)SalesArrangementTypes.MortgageRetention or (int)SalesArrangementTypes.MortgageRefixation or (int)SalesArrangementTypes.MortgageExtraPayment))
        {
            throw new NobyValidationException(90032);
        }

        // pokud je to produktovy SA, tak dal, jinak rovnou odeslat
        var saCategory = (await _codebookService.SalesArrangementTypes(cancellationToken)).First(t => t.Id == saInstance.SalesArrangementTypeId);

        // check flow switches
        if (saCategory.SalesArrangementCategory == 1)
            await validateFlowSwitches(saInstance.SalesArrangementId, saCategory.SalesArrangementCategory, cancellationToken);

        await ValidateSalesArrangement(saInstance.SalesArrangementId, request.IgnoreWarnings, cancellationToken);

        if (saCategory.SalesArrangementCategory == 1)
        {
            var customersData = await LoadCustomersData(saInstance.SalesArrangementId, saInstance.CaseId, cancellationToken);

            foreach (var customerOnSa in customersData.CustomersOnSa)
            {
                await CreateCustomerMpIfNotExists(customerOnSa, cancellationToken);

                await CreateContractRelationshipIfNotExists(customersData.RedundantCustomersOnProduct, customerOnSa, saInstance.CaseId, cancellationToken);
            }

            await DeleteRedundantContractRelationship(saInstance.CaseId, customersData.RedundantCustomersOnProduct, cancellationToken);

			await ArchiveElectronicDocumets(saInstance.SalesArrangementId, cancellationToken);

			// odeslat do SB
			await _salesArrangementService.SendToCmp(saInstance.SalesArrangementId, false, cancellationToken);

            // update case state
            await _caseService.UpdateCaseState(saInstance.CaseId, (int)EnumCaseStates.InApproval, cancellationToken);
        }
        else
        {
			await ArchiveElectronicDocumets(saInstance.SalesArrangementId, cancellationToken);

			// odeslat do SB
			await _salesArrangementService.SendToCmp(saInstance.SalesArrangementId, false, cancellationToken);
        }
    }

	private async Task ArchiveElectronicDocumets(int salesArrangementId, CancellationToken cancellationToken)
	{
		var digitallySignedDocuments = (await _documentOnSAService.GetDocumentsOnSAList(salesArrangementId, cancellationToken))
									   .DocumentsOnSA.Where(d => d.IsSigned && d.SignatureTypeId == (int)SignatureTypes.Electronic && !d.IsArchived);

		await Task.WhenAll(digitallySignedDocuments.Select(doc => _documentOnSAService.SetDocumentOnSAArchived(doc.DocumentOnSAId!.Value, cancellationToken)));
	}

	private async Task validateFlowSwitches(int salesArrangementId, int salesArrangementCategory, CancellationToken cancellationToken)
    {
        var flowSwitches = await _salesArrangementService.GetFlowSwitches(salesArrangementId, cancellationToken);
        var sections = await _mediator.Send(new GetFlowSwitchesRequest(salesArrangementId), cancellationToken);

        // HFICH-3630
        if (salesArrangementCategory == 1 && !flowSwitches.Any(t => t.FlowSwitchId == (int)FlowSwitches.IsOfferGuaranteed && t.Value))
        {
            throw new NobyValidationException(90016);
        }

        // HFICH-8011
        if (!sections.SendButton.IsActive)
        {
            throw new NobyValidationException(90001, "SendButton.IsActive is false");
        }
    }

    private async Task ValidateSalesArrangement(int salesArrangementId, bool ignoreWarnings, CancellationToken cancellationToken)
    {
        var validationResult = await _salesArrangementService.ValidateSalesArrangement(salesArrangementId, cancellationToken);

        var hasWarnings = false;
        var hasErrors = validationResult.ValidationMessages.Any(v =>
        {
            if (!hasWarnings)
                hasWarnings = v.NobyMessageDetail.Severity == _SA.ValidationMessageNoby.Types.NobySeverity.Warning;

            return v.NobyMessageDetail.Severity == _SA.ValidationMessageNoby.Types.NobySeverity.Error;
        });

        if (hasErrors || (hasWarnings && !ignoreWarnings))
            throw new CisValidationException("SA neni validni, nelze odeslat do SB. Provolej Validate endpoint.");
    }

    private async Task<CustomersData> LoadCustomersData(int salesArrangementId, long caseId, CancellationToken cancellationToken)
    {
        var customersOnSa = await _customerOnSaService.GetCustomerList(salesArrangementId, cancellationToken);
        var redundantCustomersOnProduct = (await _productService.GetCustomersOnProduct(caseId, cancellationToken)).Customers;

        return new CustomersData
        {
            CustomersOnSa = customersOnSa.Select(ParseCustomerOnSaExtended).ToList(),
            RedundantCustomersOnProduct = redundantCustomersOnProduct
        };

        static CustomerOnSaExtended ParseCustomerOnSaExtended(CustomerOnSA customerOnSa)
        {
            ValidateCustomerIdentifiers(customerOnSa.CustomerIdentifiers, customerOnSa.CustomerOnSAId);

            var identityMp = customerOnSa.CustomerIdentifiers.GetMpIdentity();
            var identityKb = customerOnSa.CustomerIdentifiers.GetKbIdentity();

            return new CustomerOnSaExtended
            {
                IdentityMp = identityMp,
                IdentityKb = identityKb,
                Customer = customerOnSa
            };
        }
    }

    private static void ValidateCustomerIdentifiers(IEnumerable<Identity> identities, int customerOnSaId)
    {
        var requiredSchemes = new[] { Identity.Types.IdentitySchemes.Mp, Identity.Types.IdentitySchemes.Kb };

        if (identities.Select(c => c.IdentityScheme).Join(requiredSchemes, x => x, y => y, (x, _) => x).Count() < 2)
        {
            throw new CisValidationException($"CustomerOnSa (Id: {customerOnSaId}) does not have KB ID or MP ID");
        }
    }

    private async Task CreateCustomerMpIfNotExists(CustomerOnSaExtended customerOnSa, CancellationToken cancellationToken)
    {
        try
        {
            var customerMp = await _customerService.GetCustomerDetail(customerOnSa.IdentityMp, cancellationToken);

            if (!customerMp.Identities.HasKbIdentity())
            {
                var updateTask = _customerService.UpdateCustomerIdentifiers(new UpdateCustomerIdentifiersRequest
                {
                    Mandant = Mandants.Mp,
                    CustomerIdentities = { customerOnSa.IdentityMp, customerOnSa.IdentityKb }
                }, cancellationToken);

                await RunTaskAndIgnoreMpHomeErrors(updateTask);
            }
        }
        catch (CisNotFoundException)
        {
            await CreateCustomerMp(customerOnSa.IdentityMp, customerOnSa.IdentityKb, cancellationToken);
        }
    }

    private async Task CreateCustomerMp(Identity identityMp, Identity identityKb, CancellationToken cancellationToken)
    {
        var customerDetail = await _customerService.GetCustomerDetail(identityKb, cancellationToken);

        var request = new CreateCustomerRequest
        {
            Mandant = Mandants.Mp,
            HardCreate = true,
            Identities = { identityMp, identityKb },
            NaturalPerson = customerDetail.NaturalPerson,
            Addresses = { customerDetail.Addresses },
            IdentificationDocument = customerDetail.IdentificationDocument,
            Contacts = { customerDetail.Contacts }
        };

        await RunTaskAndIgnoreMpHomeErrors(_customerService.CreateCustomer(request, cancellationToken));
    }

    private async Task CreateContractRelationshipIfNotExists(ICollection<GetCustomersOnProductResponseItem> redundantCustomersOnProduct,
                                                             CustomerOnSaExtended customerOnSa,
                                                             long caseId,
                                                             CancellationToken cancellationToken)
    {
        var customerOnProduct = redundantCustomersOnProduct.FirstOrDefault(c => c.CustomerIdentifiers.Any(IdentityPredicate));

        if (customerOnProduct is not null)
        {
            redundantCustomersOnProduct.Remove(customerOnProduct);

            return;
        }

        var relationshipTypeId = (SharedTypes.Enums.EnumCustomerRoles)customerOnSa.Customer.CustomerRoleId switch
        {
            SharedTypes.Enums.EnumCustomerRoles.Debtor => 1,
            SharedTypes.Enums.EnumCustomerRoles.Codebtor => 2,
            _ => 0
        };
        await RunTaskAndIgnoreMpHomeErrors(_productService.CreateContractRelationship(customerOnSa.IdentityMp.IdentityId, caseId, relationshipTypeId, cancellationToken));

        bool IdentityPredicate(Identity identity) => identity.IdentityId == customerOnSa.IdentityMp.IdentityId && identity.IdentityScheme == Identity.Types.IdentitySchemes.Mp;
    }

    private async Task DeleteRedundantContractRelationship(long caseId, IEnumerable<GetCustomersOnProductResponseItem> redundantCustomersOnProduct, CancellationToken cancellationToken)
    {
        foreach (var redundantCustomerOnProduct in redundantCustomersOnProduct)
        {
            await _productService.DeleteContractRelationship(redundantCustomerOnProduct.CustomerIdentifiers.GetMpIdentity().IdentityId, caseId, cancellationToken);
        }
    }

    //Mock - should return warning in the future.
    private static async Task RunTaskAndIgnoreMpHomeErrors(Task task)
    {
        try
        {
            await task;
        }
        catch (Exception ex) when (ex is CisArgumentException or CisValidationException)
        {
            throw;
        }
        catch
        {
            //Ignore.
        }
    }
}