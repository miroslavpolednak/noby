using __SA = DomainServices.SalesArrangementService.Contracts;

namespace NOBY.Api.Endpoints.Cases.CreateSalesArrangement.Services;

internal sealed class CustomerChangeBuilder
    : BaseBuilder, ICreateSalesArrangementParametersBuilder
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CustomerChangeBuilder(ILogger<CreateSalesArrangementParametersFactory> logger, __SA.CreateSalesArrangementRequest request, IHttpContextAccessor httpContextAccessor)
        : base(logger, request)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<__SA.CreateSalesArrangementRequest> UpdateParameters(CancellationToken cancellationToken = default(CancellationToken))
    {
        // Dotažení dat z KonsDB ohledně účtu pro splácení přes getMortgage
        var productService = _httpContextAccessor.HttpContext!.RequestServices.GetRequiredService<DomainServices.ProductService.Clients.IProductServiceClient>();
        var customerService = _httpContextAccessor.HttpContext!.RequestServices.GetRequiredService<DomainServices.CustomerService.Clients.ICustomerServiceClient>();

        try
        {
            var mortgageInstance = await productService.GetMortgage(_request.CaseId, cancellationToken);

            if (!string.IsNullOrEmpty(mortgageInstance.Mortgage.PaymentAccount?.Number) && !string.IsNullOrEmpty(mortgageInstance.Mortgage.PaymentAccount?.BankCode))
            {
                _request.CustomerChange.RepaymentAccount.Prefix = mortgageInstance.Mortgage.PaymentAccount.Prefix;
                _request.CustomerChange.RepaymentAccount.Number = mortgageInstance.Mortgage.PaymentAccount.Number;
                _request.CustomerChange.RepaymentAccount.BankCode = mortgageInstance.Mortgage.PaymentAccount.BankCode;
            }
            else
                _logger.LogInformation("DrawingBuilder: Account is empty");
    
            // applicants
            var customers = (await productService.GetCustomersOnProduct(_request.CaseId, cancellationToken))
                .Customers
                .Where(t => t.RelationshipCustomerProductTypeId == 1 || t.RelationshipCustomerProductTypeId == 2);
            
            foreach (var customer in customers)
            {
                var identity = customer.CustomerIdentifiers.First();
                var customerDetail = await customerService.GetCustomerDetail(identity, cancellationToken);
                
                _request.CustomerChange.Applicants.Add(new __SA.SalesArrangementParametersCustomerChange.Types.ApplicantObject
                {
                    Identity = identity,
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
                });
            }
        }
        catch
        {
            _logger.LogInformation("DrawingBuilder: Account not found in ProductService");
        }

        return _request;
    }
}
