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

    public async Task<EasForm> Create(int salesArrangementId, EasFormConfiguration config, CancellationToken cancellationToken)
    {
        var inputParameters = new InputParameters { SalesArrangementId = salesArrangementId };

        var easForm = CreateEasForm(config.EasFormRequestType);

        await _dataServicesLoader.LoadData(config.InputConfig, inputParameters, easForm.FormData, cancellationToken);

        return easForm;
    }

    private EasForm CreateEasForm(EasFormRequestType requestType)
    {
        return requestType switch
        {
            EasFormRequestType.Service => new EasServiceForm(CreateData<ServiceFormData>()),
            EasFormRequestType.Product => new EasProductForm(CreateData<ProductFormData>()),
            _ => throw new ArgumentOutOfRangeException()
        };

        TFormData CreateData<TFormData>() where TFormData : AggregatedData => _serviceProvider.GetRequiredService<TFormData>();
    }
}