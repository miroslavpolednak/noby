using CIS.InternalServices.DocumentDataAggregator.Configuration.EasForm;
using DomainServices.CodebookService.Clients;

namespace CIS.InternalServices.DocumentDataAggregator.EasForms.FormData;

internal class ServiceFormData : DataServices.AggregatedData, IServiceFormData
{
    EasFormRequestType IEasFormData.EasFormRequestType => EasFormRequestType.Service;

    public Task LoadAdditionalData()
    {
        return Task.CompletedTask;
    }

    public override Task LoadCodebooks(ICodebookServiceClients codebookService)
    {
        return Task.CompletedTask;
    }
}