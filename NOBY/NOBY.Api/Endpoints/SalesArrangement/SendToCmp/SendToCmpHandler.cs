﻿using CIS.Foms.Enums;
using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.CaseService.Clients;
using DomainServices.CustomerService.Clients;
using DomainServices.CustomerService.Contracts;
using DomainServices.DocumentOnSAService.Clients;
using DomainServices.HouseholdService.Clients;
using DomainServices.HouseholdService.Contracts;
using DomainServices.ProductService.Clients;
using DomainServices.ProductService.Contracts;
using DomainServices.SalesArrangementService.Clients;
using NOBY.Api.Endpoints.SalesArrangement.GetFlowSwitches;
using NOBY.Api.Endpoints.SalesArrangement.SendToCmp.Dto;
using System.Threading;
using _SA = DomainServices.SalesArrangementService.Contracts;
using CreateCustomerRequest = DomainServices.CustomerService.Contracts.CreateCustomerRequest;
using Mandants = CIS.Infrastructure.gRPC.CisTypes.Mandants;

namespace NOBY.Api.Endpoints.SalesArrangement.SendToCmp;

internal sealed class SendToCmpHandler
    : IRequestHandler<SendToCmpRequest>
{

    private readonly DomainServices.CodebookService.Clients.ICodebookServiceClient _codebookService;
    private readonly ICaseServiceClient _caseService;
    private readonly ISalesArrangementServiceClient _salesArrangementService;
    private readonly ICustomerOnSAServiceClient _customerOnSaService;
    private readonly IProductServiceClient _productService;
    private readonly ICustomerServiceClient _customerService;
    private readonly IDocumentOnSAServiceClient _documentOnSAService;
    private readonly IMediator _mediator;

    public SendToCmpHandler(
        DomainServices.CodebookService.Clients.ICodebookServiceClient codebookService,
        ICaseServiceClient caseService,
        ISalesArrangementServiceClient salesArrangementService,
        ICustomerOnSAServiceClient customerOnSaService,
        IProductServiceClient productService,
        ICustomerServiceClient customerService,
        IDocumentOnSAServiceClient documentOnSAService,
        IMediator mediator)
    {
        _codebookService = codebookService;
        _caseService = caseService;
        _salesArrangementService = salesArrangementService;
        _customerOnSaService = customerOnSaService;
        _productService = productService;
        _customerService = customerService;
        _documentOnSAService = documentOnSAService;
        _mediator = mediator;
    }

    public async Task Handle(SendToCmpRequest request, CancellationToken cancellationToken)
    {
        // instance SA
        var saInstance = await _salesArrangementService.GetSalesArrangement(request.SalesArrangementId, cancellationToken);

        // pokud je to produktovy SA, tak dal, jinak rovnou odeslat
        var saCategory = (await _codebookService.SalesArrangementTypes(cancellationToken)).First(t => t.Id == saInstance.SalesArrangementTypeId);

        // check flow switches
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
            await _caseService.UpdateCaseState(saInstance.CaseId, (int)CaseStates.InApproval, cancellationToken);
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
                                       .DocumentsOnSA.Where(d => d.IsSigned && d.SignatureTypeId == (int)SignatureTypes.Electronic);
        
        await Task.WhenAll(digitallySignedDocuments.Select(doc => _documentOnSAService.SetDocumentOnSAArchived(doc.DocumentOnSAId!.Value, cancellationToken)));
    }

    private async Task validateFlowSwitches(int salesArrangementId, int salesArrangementCategory, CancellationToken cancellationToken)
    {
        var flowSwitches = await _salesArrangementService.GetFlowSwitches(salesArrangementId, cancellationToken);

        // HFICH-3630
        if (salesArrangementCategory == 1 && !isSet(FlowSwitches.IsOfferGuaranteed))
        {
            throw new NobyValidationException(90016);
        }

        // HFICH-5191
        if (salesArrangementCategory == 1 && isSet(FlowSwitches.IsOfferWithDiscount) && isSet(FlowSwitches.IsWflTaskForIPApproved, false))
        {
            throw new NobyValidationException(90018);
        }

        var sections = await _mediator.Send(new GetFlowSwitchesRequest(salesArrangementId), cancellationToken);

        if (!EverySectionIsCompleted(sections.ModelationSection, sections.IndividualPriceSection, sections.HouseholdSection,
                                     sections.ParametersSection, sections.SigningSection, sections.ScoringSection, sections.EvaluationSection))
        {
            throw new NobyValidationException(90001, "Some sections are not completed");
        }

        bool isSet(FlowSwitches flowSwitch, bool value = true)
            => flowSwitches.Any(t => t.FlowSwitchId == (int)flowSwitch && t.Value == value);

        bool EverySectionIsCompleted(params GetFlowSwitchesResponseItem[] switches) =>
            switches.Where(x => x.IsActive)
                    .All(x => x.IsCompleted || ReferenceEquals(x, sections.EvaluationSection) && flowSwitches.Any(f => f.FlowSwitchId == (int)FlowSwitches.IsRealEstateValuationAllowed && !f.Value));
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

            var identityMp = customerOnSa.CustomerIdentifiers.First(c => c.IdentityScheme == Identity.Types.IdentitySchemes.Mp);
            var identityKb = customerOnSa.CustomerIdentifiers.First(c => c.IdentityScheme == Identity.Types.IdentitySchemes.Kb);

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

            if (customerMp.Identities.All(c => c.IdentityScheme != Identity.Types.IdentitySchemes.Kb))
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

        var relationshipTypeId = (CustomerRoles)customerOnSa.Customer.CustomerRoleId switch
        {
            CustomerRoles.Debtor => 1,
            CustomerRoles.Codebtor => 2,
            _ => 0
        };
        await RunTaskAndIgnoreMpHomeErrors(_productService.CreateContractRelationship(customerOnSa.IdentityMp.IdentityId, caseId, relationshipTypeId, cancellationToken));

        bool IdentityPredicate(Identity identity) => identity.IdentityId == customerOnSa.IdentityMp.IdentityId && identity.IdentityScheme == Identity.Types.IdentitySchemes.Mp;
    }

    private async Task DeleteRedundantContractRelationship(long caseId, IEnumerable<GetCustomersOnProductResponseItem> redundantCustomersOnProduct, CancellationToken cancellationToken)
    {
        foreach (var redundantCustomerOnProduct in redundantCustomersOnProduct)
        {
            await _productService.DeleteContractRelationship(redundantCustomerOnProduct.CustomerIdentifiers.First(c => c.IdentityScheme == Identity.Types.IdentitySchemes.Mp).IdentityId, caseId, cancellationToken);
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