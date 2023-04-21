using CIS.InternalServices.DataAggregatorService.Api.Configuration.EasForm;
using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;
using CIS.InternalServices.DataAggregatorService.Api.Services.EasForms.FormData;
using CIS.InternalServices.DataAggregatorService.Api.Services.EasForms.Forms;
using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts.Endpoints.DocumentTypes;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.EasForms;

[TransientService, SelfService]
internal class EasFormFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly DataServicesLoader _dataServicesLoader;
    private readonly ICodebookServiceClients _codebookService;

    public EasFormFactory(IServiceProvider serviceProvider, DataServicesLoader dataServicesLoader, ICodebookServiceClients codebookService)
    {
        _serviceProvider = serviceProvider;
        _dataServicesLoader = dataServicesLoader;
        _codebookService = codebookService;
    }

    public async Task<IEasForm> Create(int salesArrangementId, int userId, EasFormConfiguration config, CancellationToken cancellationToken)
    {
        var inputParameters = new InputParameters
        {
            SalesArrangementId = salesArrangementId,
            UserId = userId
        };

        var documentTypes = await _codebookService.DocumentTypes(cancellationToken);

        var easForm = config.EasFormKey.RequestType switch
        {
            EasFormRequestType.Service => CreateServiceEasForm(config.EasFormKey, documentTypes),
            EasFormRequestType.Product => new EasProductForm(CreateData<ProductFormData>(), documentTypes),
            _ => throw new ArgumentOutOfRangeException()
        };

        await _dataServicesLoader.LoadData(config.InputConfig, inputParameters, easForm.AggregatedData, cancellationToken);

        return easForm;
    }

    private IEasForm CreateServiceEasForm(EasFormKey easFormKey, List<DocumentTypeItem> documentTypes)
    {
        return easFormKey.EasFormTypes.First() switch
        {
            EasFormType.F3700 => new EasServiceForm<DrawingFormData>(CreateData<DrawingFormData>(), documentTypes),
            _ => throw new NotImplementedException()
        };
    }

    private TFormData CreateData<TFormData>() where TFormData : AggregatedData => _serviceProvider.GetRequiredService<TFormData>();
}