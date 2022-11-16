﻿using CIS.InternalServices.DocumentDataAggregator.Configuration.Document;
using CIS.InternalServices.DocumentDataAggregator.DataServices;
using CIS.InternalServices.DocumentDataAggregator.Documents;
using CIS.InternalServices.DocumentDataAggregator.Documents.Mapper;
using CIS.InternalServices.DocumentDataAggregator.EasForms;
using Microsoft.Extensions.DependencyInjection;

namespace CIS.InternalServices.DocumentDataAggregator;

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

    public async Task<ICollection<DocumentFieldData>> GetDocumentData(Document document, string documentVersion, InputParameters input)
    {
        var config = await _configurationManager.LoadDocumentConfiguration((int)document, documentVersion);

        var documentMapper = await LoadDocumentData(document, input, config);

        var dynamicStringFormats = documentMapper.GetDynamicStringFormats(config.DynamicStringFormats);

        return documentMapper.GetDocumentFields(config.SourceFields, dynamicStringFormats).ToList();
    }

    public async Task<IEasForm<TData>> GetEasForm<TData>(int salesArrangementId) where TData : IEasFormData
    {
        var formData = _serviceProvider.GetRequiredService<TData>();

        var config = await _configurationManager.LoadEasFormConfiguration((int)formData.EasFormRequestType);

        await LoadEasFormData(formData, config.InputConfig, salesArrangementId);

        return new EasForm<TData>(formData, config.SourceFields);
    }

    private async Task<DocumentMapper> LoadDocumentData(Document document, InputParameters inputParameters, DocumentConfiguration config)
    {
        var documentData = DocumentDataFactory.Create(document);

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