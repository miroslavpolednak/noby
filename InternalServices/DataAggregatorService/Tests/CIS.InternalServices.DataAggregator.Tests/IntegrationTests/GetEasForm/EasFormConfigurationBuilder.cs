﻿using CIS.InternalServices.DataAggregatorService.Api.Configuration.Common;
using CIS.InternalServices.DataAggregatorService.Api.Configuration.Data;
using CIS.InternalServices.DataAggregatorService.Api.Configuration.Data.Entities;
using CIS.InternalServices.DataAggregatorService.Contracts;
using FastEnumUtility;
using Microsoft.Extensions.DependencyInjection;
using EasFormType = CIS.InternalServices.DataAggregatorService.Contracts.EasFormType;

namespace CIS.InternalServices.DataAggregator.Tests.IntegrationTests.GetEasForm;

public class EasFormConfigurationBuilder
{
    private readonly ConfigurationContext _dbContext;

    public EasFormConfigurationBuilder(IServiceProvider serviceProvider)
    {
        _dbContext = serviceProvider.GetRequiredService<ConfigurationContext>();
    }

    public EasFormConfigurationBuilder MapFields()
    {
        _dbContext.DataServices
                  .AddRange(CreateDataService(DataSource.General),
                            CreateDataService(DataSource.SalesArrangementService),
                            CreateDataService(DataSource.CaseService),
                            CreateDataService(DataSource.OfferService),
                            CreateDataService(DataSource.UserService),
                            CreateDataService(DataSource.CustomerService),
                            CreateDataService(DataSource.ProductService),
                            CreateDataService(DataSource.OfferPaymentScheduleService),
                            CreateDataService(DataSource.HouseholdService),
                            CreateDataService(DataSource.HouseholdMainService),
                            CreateDataService(DataSource.HouseholdCodebtorService),
                            CreateDataService(DataSource.DocumentOnSa));

        _dbContext.DataFields.AddRange(new DataField[]
        {
            new() { DataFieldId = 1, DataServiceId = 1, FieldPath = "SalesArrangement.ContractNumber" },
            new() { DataFieldId = 2, DataServiceId = 1, FieldPath = "SalesArrangement.OfferId" },
            new() { DataFieldId = 3, DataServiceId = 11, FieldPath = "Custom.DocumentOnSa.FinalDocument.FormId" }
        });

        _dbContext.EasRequestTypes.Add(new EasRequestType { EasRequestTypeId = 1, EasRequestTypeName = "Service" });
        _dbContext.EasRequestTypes.Add(new EasRequestType { EasRequestTypeId = 2, EasRequestTypeName = "Product" });

        _dbContext.EasFormTypes.AddRange(new() { EasFormTypeId = 1, EasFormTypeName = "F3700", Version = 1, ValidFrom = DateTime.Now.AddYears(-1), ValidTo = DateTime.Now.AddYears(1) },
                                         new() { EasFormTypeId = 2, EasFormTypeName = "F3601", Version = 1, ValidFrom = DateTime.Now.AddYears(-1), ValidTo = DateTime.Now.AddYears(1) },
                                         new() { EasFormTypeId = 3, EasFormTypeName = "F3602", Version = 1, ValidFrom = DateTime.Now.AddYears(-1), ValidTo = DateTime.Now.AddYears(1) });

        _dbContext.EasFormDataFields
                  .AddRange(new EasFormDataField { EasFormDataFieldId = 1, EasRequestTypeId = EasFormRequestType.Product.ToInt32(), EasFormTypeId = EasFormType.F3601.ToInt32(), DataFieldId = 1, JsonPropertyName = "cislo_smlouvy" },
                            new EasFormDataField { EasFormDataFieldId = 2, EasRequestTypeId = EasFormRequestType.Product.ToInt32(), EasFormTypeId = EasFormType.F3602.ToInt32(), DataFieldId = 1, JsonPropertyName = "cislo_smlouvy" },
                            new EasFormDataField { EasFormDataFieldId = 3, EasRequestTypeId = EasFormRequestType.Product.ToInt32(), EasFormTypeId = EasFormType.F3601.ToInt32(), DataFieldId = 3, JsonPropertyName = "business_id_formulare" });

        _dbContext.EasFormSpecialDataFields.Add(new EasFormSpecialDataField { EasRequestTypeId = 2, JsonPropertyName = "developer_id", DataServiceId = 3, EasFormTypeId = 2, FieldPath = "ConditionalFormValues.DeveloperId" });

        _dbContext.InputParameters.Add(new InputParameter { InputParameterId = 3, InputParameterName = "OfferId" });

        _dbContext.EasFormDynamicInputParameters.Add(new EasFormDynamicInputParameter { EasRequestTypeId = 2, InputParameterId = 3, TargetDataServiceId = 3, SourceDataFieldId = 2, EasFormTypeId = 2 });

        return this;

        static DataService CreateDataService(DataSource dataSource) => new() { DataServiceId = (int)dataSource, DataServiceName = dataSource.ToString() };
    }

    public async Task Build()
    {
        await _dbContext.SaveChangesAsync();
    }
}