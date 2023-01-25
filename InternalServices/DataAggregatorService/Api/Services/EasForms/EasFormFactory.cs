using CIS.InternalServices.DataAggregatorService.Api.Configuration.EasForm;
using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;
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

        await _dataServicesLoader.LoadData(config.InputConfig, inputParameters, easForm.FormData.AggregatedData);
        await easForm.FormData.LoadFormSpecificData(cancellationToken);

        return easForm;
    }

    private EasForm CreateEasForm(EasFormRequestType requestType) =>
        requestType switch
        {
            EasFormRequestType.Service => CreateForm<EasServiceForm>(),
            EasFormRequestType.Product => CreateForm<EasProductForm>(),
            _ => throw new ArgumentOutOfRangeException()
        };

    private EasForm CreateForm<TForm>() where TForm : EasForm => _serviceProvider.GetRequiredService<TForm>();
}