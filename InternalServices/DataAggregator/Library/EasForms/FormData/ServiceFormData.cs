using CIS.Infrastructure.gRPC.CisTypes;
using CIS.InternalServices.DataAggregator.Configuration.EasForm;
using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts;

namespace CIS.InternalServices.DataAggregator.EasForms.FormData;

internal class ServiceFormData : DataServices.AggregatedData, IServiceFormData
{
    private List<GenericCodebookItem> _academicDegreesBefore = null!;

    EasFormRequestType IEasFormData.EasFormRequestType => EasFormRequestType.Service;

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

    public Task LoadAdditionalData() => Task.CompletedTask;

    public override async Task LoadCodebooks(ICodebookServiceClients codebookService)
    {
        _academicDegreesBefore = await codebookService.AcademicDegreesBefore();
    }
}