using CIS.InternalServices.DocumentDataAggregator.Configuration.EasForm;
using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts;

namespace CIS.InternalServices.DocumentDataAggregator.EasForms.FormData;

internal class ServiceFormData : DataServices.AggregatedData, IServiceFormData
{
    private List<GenericCodebookItem> _academicDegreesBefore = null!;

    EasFormRequestType IEasFormData.EasFormRequestType => EasFormRequestType.Service;

    public MockValues MockValues = new();

    public string? DegreeBefore
    {
        get
        {
            var degreeBeforeId = Customer.NaturalPerson.DegreeBeforeId;

            return degreeBeforeId.HasValue ? _academicDegreesBefore.First(x => x.Id == degreeBeforeId).Name : null;
        }
    }

    public bool IsAgent => SalesArrangement.Drawing.Agent is not null;

    public Task LoadAdditionalData() => Task.CompletedTask;

    public override async Task LoadCodebooks(ICodebookServiceClients codebookService)
    {
        _academicDegreesBefore = await codebookService.AcademicDegreesBefore();
    }
}