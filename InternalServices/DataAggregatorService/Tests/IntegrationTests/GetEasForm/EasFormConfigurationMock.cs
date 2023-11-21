using CIS.InternalServices.DataAggregatorService.Api.Configuration;
using CIS.InternalServices.DataAggregatorService.Api.Configuration.Common;
using CIS.InternalServices.DataAggregatorService.Api.Configuration.EasForm;
using CIS.InternalServices.DataAggregatorService.Contracts;

namespace CIS.InternalServices.DataAggregator.Tests.IntegrationTests.GetEasForm;

internal static class EasFormConfigurationMock
{
    public static void MockProductRequest(this IServiceConfigurationManager configuration)
    {
        var easFormConfiguration = new EasFormConfiguration
        {
            EasFormKey = new EasFormKey(EasFormRequestType.Product, new[] { EasFormType.F3601, EasFormType.F3602 }),
            SourceFields = new EasFormSourceField[]
            {
                new() { JsonPropertyName = "cislo_smlouvy", DataService = DataService.SalesArrangementService, FieldPath = "SalesArrangement.ContractNumber", EasFormType = EasFormType.F3601 },
                new() { JsonPropertyName = "cislo_smlouvy", DataService = DataService.SalesArrangementService, FieldPath = "SalesArrangement.ContractNumber", EasFormType = EasFormType.F3602 },
                new() { JsonPropertyName = "business_id_formulare", DataService = DataService.DocumentOnSa, FieldPath = "Custom.DocumentOnSa.FinalDocument.FormId", EasFormType = EasFormType.F3601 },
                new() { JsonPropertyName = "uv_produkt", DataService = DataService.CaseService, FieldPath = "Case.Data.ProductTypeId", EasFormType = EasFormType.F3601 },
                new() { JsonPropertyName = "developer_id", DataService = DataService.OfferService, FieldPath = "ConditionalFormValues.DeveloperId", EasFormType = EasFormType.F3601 },
            },
            DynamicInputParameters = new List<DynamicInputParameter>
            {
                new() { InputParameter = "CaseId", TargetDataService = DataService.CaseService, SourceDataService = DataService.SalesArrangementService, SourceFieldPath = "SalesArrangement.CaseId" },
                new() { InputParameter = "OfferId", TargetDataService = DataService.OfferService, SourceDataService = DataService.SalesArrangementService, SourceFieldPath = "SalesArrangement.OfferId" }
            }
        };

        configuration.LoadEasFormConfiguration(Arg.Is<EasFormKey>(x => x.RequestType == EasFormRequestType.Product), Arg.Any<CancellationToken>()).Returns(easFormConfiguration);
    }

    public static void MockServiceRequest(this IServiceConfigurationManager configuration)
    {
        var easFormConfiguration = new EasFormConfiguration
        {
            EasFormKey = new EasFormKey(EasFormRequestType.Service, new[] { EasFormType.F3700 }),
            SourceFields = new EasFormSourceField[]
            {
                new() { JsonPropertyName = "cislo_smlouvy", DataService = DataService.CaseService, FieldPath = "Case.Data.ContractNumber", EasFormType = EasFormType.F3700 },
                new() { JsonPropertyName = "business_id_formulare", DataService = DataService.DocumentOnSa, FieldPath = "Custom.DocumentOnSa.FinalDocument.FormId", EasFormType = EasFormType.F3700 }
            },
            DynamicInputParameters = new List<DynamicInputParameter>
            {
                new() { InputParameter = "CaseId", TargetDataService = DataService.CaseService, SourceDataService = DataService.SalesArrangementService, SourceFieldPath = "SalesArrangement.CaseId" }
            }
        };

        configuration.LoadEasFormConfiguration(Arg.Is<EasFormKey>(x => x.RequestType == EasFormRequestType.Service), Arg.Any<CancellationToken>()).Returns(easFormConfiguration);
    }
}