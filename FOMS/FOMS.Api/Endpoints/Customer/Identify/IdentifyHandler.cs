﻿using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.CustomerService.Contracts;
using FOMS.Api.Endpoints.Customer.Search;
using FOMS.Api.Endpoints.Customer.Search.Dto;
using contracts = DomainServices.CustomerService.Contracts;

namespace FOMS.Api.Endpoints.Customer.Identify;

internal sealed class IdentifyHandler
    : IRequestHandler<IdentifyRequest, Search.Dto.CustomerInList>
{
    public async Task<Search.Dto.CustomerInList> Handle(IdentifyRequest request, CancellationToken cancellationToken)
    {
        var dsRequest = new contracts.SearchCustomersRequest
        {
            NaturalPerson = new contracts.SearchNaturalPerson()
            {
                FirstName = request.FirstName ?? "",
                LastName = request.LastName ?? "",
                BirthNumber = request.BirthNumber ?? "",
                DateOfBirth = request.DateOfBirth
            },
            IdentificationDocument = new SearchIdentificationDocument
            {
                IdentificationDocumentTypeId = request.IdentificationDocumentTypeId,
                IssuingCountryId = request.IssuingCountryId,
                Number = request.IdentificationDocumentNumber ?? ""
            },
            Mandant = CIS.Infrastructure.gRPC.CisTypes.Mandants.Kb
        };

        // ID klienta
        if (request.IdentityId.HasValue)
        {
            dsRequest.Identity = new Identity(request.IdentityId, CIS.Foms.Enums.IdentitySchemes.Kb);
        }

        var result = ServiceCallResult.ResolveAndThrowIfError<contracts.SearchCustomersResponse>(await _customerService.SearchCustomers(dsRequest, cancellationToken));

        if (!result.Customers.Any())
            throw new CisValidationException("Client not found");
        else if (result.Customers.Count > 1)
        {
            _logger.LogInformation("More than 1 client found");
            throw new CisValidationException("More than 1 client found");
        }

        var customer = result.Customers.First();
        return (new CustomerInList())
            .FillBaseData(customer)
            .FillIdentification(customer.Identities);
    }

    private readonly ILogger<IdentifyHandler> _logger;
    private readonly DomainServices.CustomerService.Abstraction.ICustomerServiceAbstraction _customerService;

    public IdentifyHandler(DomainServices.CustomerService.Abstraction.ICustomerServiceAbstraction customerService, ILogger<IdentifyHandler> logger)
    {
        _customerService = customerService;
        _logger = logger;
    }
}
