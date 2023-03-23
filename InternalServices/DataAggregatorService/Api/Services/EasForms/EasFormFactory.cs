using CIS.InternalServices.DataAggregatorService.Api.Configuration.EasForm;
using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;
using CIS.InternalServices.DataAggregatorService.Api.Services.EasForms.FormData;
using CIS.InternalServices.DataAggregatorService.Api.Services.EasForms.Forms;

namespace CIS.InternalServices.DataAggregatorService.Api.Services.EasForms;

[TransientService, SelfService]
internal class EasFormFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly DataServicesLoader _dataServicesLoader;

    public EasFormFactory(IServiceProvider serviceProvider, DataServicesLoader dataServicesLoader)
    {
        _serviceProvider = serviceProvider;
        _dataServicesLoader = dataServicesLoader;
    }

    public async Task<IEasForm> Create(int salesArrangementId, EasFormConfiguration config, CancellationToken cancellationToken)
    {
        var inputParameters = new InputParameters { SalesArrangementId = salesArrangementId };

        var easForm = config.EasFormKey.RequestType switch
        {
            EasFormRequestType.Service => CreateServiceEasForm(config.EasFormKey),
            EasFormRequestType.Product => new EasProductForm(CreateData<ProductFormData>()),
            _ => throw new ArgumentOutOfRangeException()
        };

        await _dataServicesLoader.LoadData(config.InputConfig, inputParameters, easForm.AggregatedData, cancellationToken);

        return easForm;
    }

    private IEasForm CreateServiceEasForm(EasFormKey easFormKey)
    {
        return easFormKey.EasFormTypes.First() switch
        {
            EasFormType.F3700 => new EasServiceForm<DrawingFormData>(CreateData<DrawingFormData>()),
            _ => throw new NotImplementedException()
        };
    }

    private TFormData CreateData<TFormData>() where TFormData : AggregatedData => _serviceProvider.GetRequiredService<TFormData>();
}