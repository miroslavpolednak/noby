﻿using System.ComponentModel;
using CIS.InternalServices.DataAggregatorService.Api.Configuration.EasForm;
using CIS.InternalServices.DataAggregatorService.Api.Generators.EasForms.FormData;
using CIS.InternalServices.DataAggregatorService.Api.Generators.EasForms.Forms;
using CIS.InternalServices.DataAggregatorService.Api.Services.DataServices;
using DomainServices.CodebookService.Clients;
using DomainServices.CodebookService.Contracts.v1;

namespace CIS.InternalServices.DataAggregatorService.Api.Generators.EasForms;

[TransientService, SelfService]
internal class EasFormFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly DataServicesLoader _dataServicesLoader;
    private readonly ICodebookServiceClient _codebookService;

    public EasFormFactory(IServiceProvider serviceProvider, DataServicesLoader dataServicesLoader, ICodebookServiceClient codebookService)
    {
        _serviceProvider = serviceProvider;
        _dataServicesLoader = dataServicesLoader;
        _codebookService = codebookService;
    }

    public async Task<IEasForm> Create(InputParameters inputParameters, EasFormConfiguration config, ICollection<DynamicFormValues> dynamicFormValues, CancellationToken cancellationToken)
    {
        var documentTypes = await _codebookService.DocumentTypes(cancellationToken);

        var easForm = config.EasFormKey.RequestType switch
        {
            EasFormRequestType.Service => CreateServiceEasForm(config.EasFormKey, documentTypes),
            EasFormRequestType.Product => CreateProductEasForm(dynamicFormValues, documentTypes, config.IsCancelled),
            _ => throw new InvalidEnumArgumentException(nameof(config.EasFormKey.RequestType), config.EasFormKey.RequestTypeId, typeof(EasFormRequestType))
        };

        await _dataServicesLoader.LoadData(config.InputConfig, inputParameters, easForm.AggregatedData, cancellationToken);

        return easForm;
    }

    private IEasForm CreateProductEasForm(IEnumerable<DynamicFormValues> dynamicFormValues, List<DocumentTypesResponse.Types.DocumentTypeItem> documentTypes, bool isCancelled)
    {
        var productData = CreateData<ProductFormData>();

        productData.MainDynamicFormValues = dynamicFormValues.First(d => d.DocumentTypeId == (int)DocumentTypes.ZADOSTHU);
        productData.IsCancelled = isCancelled;

        return new EasProductForm(productData, documentTypes);
    }

    private IEasForm CreateServiceEasForm(EasFormKey easFormKey, List<DocumentTypesResponse.Types.DocumentTypeItem> documentTypes)
    {
        return easFormKey.EasFormTypes.First() switch
        {
            EasFormType.F3700 => new EasServiceForm<DrawingFormData>(CreateData<DrawingFormData>(), documentTypes),
            EasFormType.F3602 => new EasServiceForm<CustomerChange3602FormData>(CreateData<CustomerChange3602FormData>(), documentTypes),
            _ => throw new NotImplementedException()
        };
    }

    private TFormData CreateData<TFormData>() where TFormData : AggregatedData => _serviceProvider.GetRequiredService<TFormData>();
}