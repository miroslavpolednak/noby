using CIS.Infrastructure.gRPC.CisTypes;
using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;
using CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData.Shared;
using DomainServices.ProductService.Clients;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.EasForms.FormData;

[TransientService, SelfService]
internal class DrawingFormData : AggregatedData
{
    private readonly IProductServiceClient _productService;

    private ICollection<Identity> _applicantIdentities = null!;

    public DrawingFormData(IProductServiceClient productService)
    {
        _productService = productService;
    }

    public MockValues MockValues { get; } = new();

    public string? DegreeBefore
    {
        get
        {
            var degreeBeforeId = Customer.NaturalPerson.DegreeBeforeId;

            return degreeBeforeId.HasValue ? _codebookManager.DegreesBefore.First(x => x.Id == degreeBeforeId).Name : null;
        }
    }

    public long IdentityKb => _applicantIdentities.First(c => c.IdentityScheme == Identity.Types.IdentitySchemes.Kb).IdentityId;

    public long IdentityMp => _applicantIdentities.First(i => i.IdentityScheme == Identity.Types.IdentitySchemes.Mp).IdentityId;

    public bool IsAgent => SalesArrangement.Drawing.Agent?.IsActive ?? false;

    public override async Task LoadAdditionalData(CancellationToken cancellationToken)
    {
        var customers = await _productService.GetCustomersOnProduct(SalesArrangement.CaseId, cancellationToken);

        _applicantIdentities = customers.Customers.First(c => c.CustomerIdentifiers.Contains(SalesArrangement.Drawing.Applicant)).CustomerIdentifiers;
    }

    protected override void ConfigureCodebooks(ICodebookManagerConfigurator configurator)
    {
        configurator.DegreesBefore();
    }
}