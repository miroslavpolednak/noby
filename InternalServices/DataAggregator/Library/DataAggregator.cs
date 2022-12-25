using CIS.Foms.Enums;
using CIS.InternalServices.DataAggregator.Configuration.Document;
using CIS.InternalServices.DataAggregator.DataServices;
using CIS.InternalServices.DataAggregator.Documents;
using CIS.InternalServices.DataAggregator.Documents.Mapper;
using CIS.InternalServices.DataAggregator.EasForms;
using CIS.InternalServices.DataAggregator.EasForms.FormData;
using Microsoft.Extensions.DependencyInjection;

namespace CIS.InternalServices.DataAggregator;

internal class DataAggregator : IDataAggregator
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ConfigurationManager _configurationManager;
    private readonly DataServicesLoader _dataServicesLoader;

    public DataAggregator(IServiceProvider serviceProvider, ConfigurationManager configurationManager, DataServicesLoader dataServicesLoader)
    {
        _serviceProvider = serviceProvider;
        _configurationManager = configurationManager;
        _dataServicesLoader = dataServicesLoader;
    }

    public async Task<ICollection<DocumentFieldData>> GetDocumentData(DocumentTemplateType documentType, string documentVersion, InputParameters input)
    {
        var config = await _configurationManager.LoadDocumentConfiguration((int)documentType, documentVersion);

        var documentMapper = await LoadDocumentData(documentType, input, config);

        var dynamicStringFormats = documentMapper.GetDynamicStringFormats(config.DynamicStringFormats);

        return documentMapper.GetDocumentFields(config.SourceFields, dynamicStringFormats)
                             .Union(documentMapper.GetDocumentTables(config.Tables))
                             .ToList();
    }

    public async Task<IEasForm<TData>> GetEasForm<TData>(int salesArrangementId) where TData : IEasFormData
    {
        var formData = _serviceProvider.GetRequiredService<TData>();

        var config = await _configurationManager.LoadEasFormConfiguration((int)formData.EasFormRequestType);

        await LoadEasFormData(formData, config.InputConfig, salesArrangementId);

        return formData switch
            {
                ProductFormData productData => (IEasForm<TData>)new ProductEasForm(productData, config.SourceFields),
                _ => new EasForm<TData>(formData, config.SourceFields)
            };
    }

    private async Task<DocumentMapper> LoadDocumentData(DocumentTemplateType documentType, InputParameters inputParameters, DocumentConfiguration config)
    {
        var documentData = DocumentDataFactory.Create(documentType);

        await _dataServicesLoader.LoadData(config.InputConfig, inputParameters, documentData);

        return DocumentMapper.Create(documentData);
    }

    private async Task LoadEasFormData(IEasFormData easFormData, InputConfig inputConfig, int salesArrangementId)
    {
        if (easFormData is AggregatedData aggregatedData)
            await _dataServicesLoader.LoadData(inputConfig, new InputParameters { SalesArrangementId = salesArrangementId }, aggregatedData);

        await easFormData.LoadAdditionalData();
    }
}