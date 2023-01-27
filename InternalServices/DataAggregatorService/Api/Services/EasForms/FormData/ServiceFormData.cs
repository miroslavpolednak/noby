using CIS.Infrastructure.gRPC.CisTypes;
using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;
using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.EasForms.FormData;

[TransientService, SelfService]
internal class ServiceFormData : AggregatedData
{
    private List<GenericCodebookItem> _academicDegreesBefore = null!;

    public AggregatedData AggregatedData => this;

    public MockValues MockValues { get; } = new();

    public string? DegreeBefore
    {
        get
        {
            var degreeBeforeId = Customer.NaturalPerson.DegreeBeforeId;

            return degreeBeforeId.HasValue ? _academicDegreesBefore.First(x => x.Id == degreeBeforeId).Name : null;
        }
    }

    public long IdentityKb => Customer.Identities.Single(c => c.IdentityScheme == Identity.Types.IdentitySchemes.Kb).IdentityId;

    public bool IsAgent => SalesArrangement.Drawing.Agent is not null;

    public override async Task LoadCodebooks(ICodebookServiceClients codebookService, CancellationToken cancellationToken)
    {
        _academicDegreesBefore = await codebookService.AcademicDegreesBefore(cancellationToken);
    }
}