﻿using SharedTypes.GrpcTypes;
using NOBY.Api.Endpoints.Cases.CreateSalesArrangement.Services.Internals;
using __SA = DomainServices.SalesArrangementService.Contracts;

namespace NOBY.Api.Endpoints.Cases.CreateSalesArrangement.Services;

internal sealed class CustomerChangeBuilder
    : BaseBuilder
{
    public override async Task<__SA.CreateSalesArrangementRequest> UpdateParameters(CancellationToken cancellationToken = default(CancellationToken))
    {
        // Dotažení dat z KonsDB ohledně účtu pro splácení přes getMortgage
        var productService = _httpContextAccessor.HttpContext!.RequestServices.GetRequiredService<DomainServices.ProductService.Clients.IProductServiceClient>();
        var customerService = _httpContextAccessor.HttpContext!.RequestServices.GetRequiredService<DomainServices.CustomerService.Clients.ICustomerServiceClient>();

        _request.CustomerChange = new();
        
        try
        {
            var mortgageInstance = await productService.GetMortgage(_request.CaseId, cancellationToken);

            if (!string.IsNullOrEmpty(mortgageInstance.Mortgage.RepaymentAccount?.Number) && !string.IsNullOrEmpty(mortgageInstance.Mortgage.RepaymentAccount?.BankCode))
            {
                _request.CustomerChange.RepaymentAccount = new()
                {
                    AgreedPrefix = mortgageInstance.Mortgage.RepaymentAccount.Prefix,
                    AgreedNumber = mortgageInstance.Mortgage.RepaymentAccount.Number,
                    AgreedBankCode = mortgageInstance.Mortgage.RepaymentAccount.BankCode
                };
            }
            else
                _logger.LogInformation("DrawingBuilder: Account is empty");

            // applicants
            var customers = (await productService.GetCustomersOnProduct(_request.CaseId, cancellationToken)).Customers;
            var applicants = customers.Where(t => _allowedCustomerRoles.Contains(t.RelationshipCustomerProductTypeId));
            var loadedCustomers = new List<DomainServices.CustomerService.Contracts.CustomerDetailResponse>();

            foreach (var customer in applicants)
            {
                var identity = customer.CustomerIdentifiers.First();
                var customerDetail = await customerService.GetCustomerDetail(identity, cancellationToken);
                loadedCustomers.Add(customerDetail);

                var applicant = new __SA.SalesArrangementParametersCustomerChange.Types.ApplicantObject
                {
                    NaturalPerson = new __SA.SalesArrangementParametersCustomerChange.Types.NaturalPersonObject
                    {
                        FirstName = customerDetail.NaturalPerson?.FirstName ?? "",
                        LastName = customerDetail.NaturalPerson?.LastName ?? "",
                        DateOfBirth = ((DateTime?)customerDetail.NaturalPerson?.DateOfBirth) ?? DateTime.MinValue
                    },
                    IdentificationDocument = new()
                    {
                        IdentificationDocumentTypeId = customerDetail.IdentificationDocument?.IdentificationDocumentTypeId ?? 0,
                        Number = customerDetail.IdentificationDocument?.Number ?? ""
                    }
                };
                applicant.Identity.Add(identity);

                _request.CustomerChange.Applicants.Add(applicant);
            }

            // agent
            var agent = customers.FirstOrDefault(t => t.Agent ?? false)?.CustomerIdentifiers?.FirstOrDefault(t => t.IdentityScheme == Identity.Types.IdentitySchemes.Kb);
            if (agent is not null)
            {
                var agentCustomer = loadedCustomers.FirstOrDefault(t => t.Identities.Any(t => t.IdentityScheme == agent.IdentityScheme && t.IdentityId == agent.IdentityId));
                // customer jeste nebyl dotazen
                if (agentCustomer is null)
                {
                    agentCustomer = await customerService.GetCustomerDetail(agent, cancellationToken);
                }

                _request.CustomerChange.Agent = new()
                {
                    ActualAgent = $"{agentCustomer.NaturalPerson.LastName} {agentCustomer.NaturalPerson.FirstName}"
                };
            }
        }
        catch
        {
            _logger.LogInformation("DrawingBuilder: Account not found in ProductService");
        }

        return _request;
    }

    private static int[] _allowedCustomerRoles = new[] { 1, 2 };

    public CustomerChangeBuilder(ILogger<CreateSalesArrangementParametersFactory> logger, __SA.CreateSalesArrangementRequest request, IHttpContextAccessor httpContextAccessor)
        : base(logger, request, httpContextAccessor)
    {
    }
}
