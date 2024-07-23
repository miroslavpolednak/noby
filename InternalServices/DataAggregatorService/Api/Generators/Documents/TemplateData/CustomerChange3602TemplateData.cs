using CIS.InternalServices.DataAggregatorService.Api.Generators.Documents.TemplateData.LoanApplication;
using CIS.InternalServices.DataAggregatorService.Api.Generators.Documents.TemplateData.Shared;
using CIS.InternalServices.DataAggregatorService.Api.Services;
using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices.CustomModels;
using DomainServices.CustomerService.Clients;
using DomainServices.ProductService.Clients;

namespace CIS.InternalServices.DataAggregatorService.Api.Generators.Documents.TemplateData;

[TransientService, SelfService]
internal class CustomerChange3602TemplateData : LoanApplicationBaseTemplateData
{
    private readonly IProductServiceClient _productService;
    private readonly ICustomerServiceClient _customerService;
    protected override HouseholdInfo CurrentHousehold => HouseholdCodebtor ?? throw CreateNullHouseholdException(nameof(HouseholdCodebtor));

    public CustomerChange3602TemplateData(CustomerWithChangesService customerWithChangesService, IProductServiceClient productService, ICustomerServiceClient customerService) : base(customerWithChangesService)
    {
        _productService = productService;
        _customerService = customerService;
    }

    public string LoanDurationText => "Předpokládané datum splatnosti";
    
    public string LoanType => Mortgage.LoanKindId == 2001 ? GetLoanKindName(Mortgage.LoanKindId ?? 0) : GetProductTypeName(Mortgage.ProductTypeId);

    public string LoanPurposes => GetLoanPurposes(Mortgage.LoanKindId ?? 0, Mortgage.LoanPurposes.Select(l => l.LoanPurposeId));

    public string? AgentName { get; private set; }

    public override async Task LoadAdditionalData(InputParameters parameters, CancellationToken cancellationToken)
    {
        await base.LoadAdditionalData(parameters, cancellationToken);

        if (SalesArrangement.SalesArrangementTypeId != (int)SalesArrangementTypes.CustomerChange3602B)
            return;

        var customersOnProduct = await _productService.GetCustomersOnProduct(SalesArrangement.CaseId, cancellationToken);

        var customerIdentity = customersOnProduct.Customers.Where(c => c.Agent == true).Select(c => c.CustomerIdentifiers.FirstOrDefault()).FirstOrDefault();

        if (customerIdentity is null)
            return;

        var customerDetail = await _customerService.GetCustomerDetail(customerIdentity, cancellationToken);

        AgentName = CustomerHelper.FullName(customerDetail);
    }
}