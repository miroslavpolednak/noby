﻿using CIS.Infrastructure.gRPC.CisTypes;
using DomainServices.CustomerService.Contracts;
using FOMS.Api.Endpoints.Customer.Search;
using FOMS.Api.Endpoints.Customer.Search.Dto;
using contracts = DomainServices.CustomerService.Contracts;

namespace FOMS.Api.Endpoints.Customer.Identify;

internal sealed class IdentifyHandler
    : IRequestHandler<IdentifyRequest, CustomerInList>
{
    public async Task<CustomerInList?> Handle(IdentifyRequest request, CancellationToken cancellationToken)
    {
        var dsRequest = new contracts.SearchCustomersRequest
        {
            NaturalPerson = new contracts.NaturalPersonSearch
            {
                FirstName = request.FirstName ?? "",
                LastName = request.LastName ?? "",
                BirthNumber = request.BirthNumber ?? "",
                DateOfBirth = request.DateOfBirth
            },
            IdentificationDocument = new IdentificationDocumentSearch
            {
                IdentificationDocumentTypeId = request.IdentificationDocumentTypeId,
                IssuingCountryId = request.IssuingCountryId,
                Number = request.IdentificationDocumentNumber ?? ""
            },
            Mandant = Mandants.Kb
        };

        // ID klienta
        if (request.Identity is not null && request.Identity.Id > 0)
        {
            dsRequest.Identity = new Identity(request.Identity.Id, request.Identity.Scheme);
        }

        var result = ServiceCallResult.ResolveAndThrowIfError<contracts.SearchCustomersResponse>(await _customerService.SearchCustomers(dsRequest, cancellationToken));

        if (!result.Customers.Any())
            return null;
        else if (result.Customers.Count > 1)
        {
            _logger.LogInformation("More than 1 client found");
            throw new CisConflictException($"More than 1 client found: {string.Join(", ", result.Customers.Select(t => t.Identity?.IdentityId.ToString()))}");
        }

        var customer = result.Customers.First();
        return (new CustomerInList())
            .FillBaseData(customer)
            .FillIdentification(customer.Identity);
    }

    private readonly ILogger<IdentifyHandler> _logger;
    private readonly DomainServices.CustomerService.Abstraction.ICustomerServiceAbstraction _customerService;

    public IdentifyHandler(DomainServices.CustomerService.Abstraction.ICustomerServiceAbstraction customerService, ILogger<IdentifyHandler> logger)
    {
        _customerService = customerService;
        _logger = logger;
    }
}
