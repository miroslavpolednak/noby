using CIS.Infrastructure.gRPC.CisTypes;
using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;
using CIS.InternalServices.DataAggregatorService.Api.Services.Documents.TemplateData.Shared;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.EasForms.FormData;

[TransientService, SelfService]
internal class DrawingFormData : AggregatedData
{
    public MockValues MockValues { get; } = new();

    public DynamicFormValues? DynamicFormValues { get; set; }

    public string? DegreeBefore
    {
        get
        {
            var degreeBeforeId = Customer.NaturalPerson.DegreeBeforeId;

            return degreeBeforeId.HasValue ? _codebookManager.DegreesBefore.First(x => x.Id == degreeBeforeId).Name : null;
        }
    }

    public long IdentityKb => Customer.Identities.Single(c => c.IdentityScheme == Identity.Types.IdentitySchemes.Kb).IdentityId;

    public bool IsAgent => SalesArrangement.Drawing.Agent?.IsActive ?? false;

    protected override void ConfigureCodebooks(ICodebookManagerConfigurator configurator)
    {
        configurator.DegreesBefore();
    }
}