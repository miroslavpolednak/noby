using CIS.InternalServices.DataAggregatorService.Api.Configuration;
using CIS.InternalServices.DataAggregatorService.Api.Configuration.Common;
using CIS.InternalServices.DataAggregatorService.Api.Configuration.Document;

namespace CIS.InternalServices.DataAggregator.Tests.IntegrationTests.GetDocumentData;

internal static class DocumentConfigurationMock
{
    public static void MockDocumentFields(this IConfigurationManager configurationManager)
    {
        var documentConfiguration = new DocumentConfiguration
        {
            DocumentTemplateVersionId = DocumentConstants.DocumentTemplateVersionId,
            DocumentTemplateVariantId = DocumentConstants.DocumentTemplateVariantId,
            SourceFields = new DocumentSourceField[]
            {
                new() { AcroFieldName = "RegCislo", DataService = DataService.SalesArrangementService, FieldPath = "SalesArrangement.ContractNumber" },
                new() { AcroFieldName = "VyseUveru", DataService = DataService.OfferService, FieldPath = "Offer.SimulationInputs.LoanAmount", StringFormat = "{0:C0}" },
                new() { AcroFieldName = "PocetDetiDo10", DataService = DataService.HouseholdService, FieldPath = "HouseholdMain.Household.Data.ChildrenUpToTenYearsCount" },
                new() { AcroFieldName = "JsemNejsem1Odrazka", DataService = DataService.HouseholdMainService, FieldPath = "HouseholdMain.Debtor.CustomerAdditionalData.HasRelationshipWithKB" },
                new() { AcroFieldName = "SpoluzadatelJsemNejsem1Odrazka", DataService = DataService.HouseholdMainService, FieldPath = "HouseholdMain.Codebtor.CustomerAdditionalData.HasRelationshipWithKB" },
                new() { AcroFieldName = "CisloSplatky", DataService = DataService.OfferService, FieldPath = "Offer.AdditionalSimulationResults.PaymentScheduleSimple[].PaymentNumber" },
            },
            DynamicInputParameters = new List<DynamicInputParameter>
            {
                new() { InputParameter = "OfferId", TargetDataService = DataService.OfferService, SourceDataService = DataService.SalesArrangementService, SourceFieldPath = "SalesArrangement.OfferId" }
            },
            DynamicStringFormats = Enumerable.Empty<DocumentDynamicStringFormat>().ToLookup(_ => ""),
            Tables = Array.Empty<DocumentTable>()
        };

        configurationManager.LoadDocumentConfiguration(Arg.Is<DocumentKey>(x => x.TypeId == DocumentConstants.DocumentTypeId && x.VersionData.VersionId == DocumentConstants.DocumentTemplateVersionId), 
                                                       Arg.Any<CancellationToken>())
                            .Returns(documentConfiguration);
    }

    public static void MockTable(this IConfigurationManager configurationManager)
    {
        var documentConfiguration = new DocumentConfiguration()
        {
            DocumentTemplateVersionId = DocumentConstants.DocumentTemplateVersionId,
            DocumentTemplateVariantId = DocumentConstants.DocumentTemplateVariantId,
            SourceFields = Array.Empty<DocumentSourceField>(),
            DynamicInputParameters = new List<DynamicInputParameter>
            {
                new() { InputParameter = "OfferId", TargetDataService = DataService.OfferPaymentScheduleService, SourceDataService = DataService.SalesArrangementService, SourceFieldPath = "SalesArrangement.OfferId" }
            },
            DynamicStringFormats = Enumerable.Empty<DocumentDynamicStringFormat>().ToLookup(_ => ""),
            Tables = new DocumentTable[]
            {
                new()
                {
                    DocumentTableId = 1,
                    AcroFieldPlaceholder = "SplatkovyKalendar", 
                    DataService = DataService.OfferPaymentScheduleService,
                    TableSourcePath = "OfferPaymentSchedule.PaymentScheduleFull[]",
                    ConcludingParagraph = "Text",
                    Columns =
                    {
                        new DocumentTable.Column { CollectionFieldPath = "Amount", Header = "Header 1", WidthPercentage = 10 },
                        new DocumentTable.Column { CollectionFieldPath = "PaymentNumber", Header = "Header 2", WidthPercentage = 10 },
                    }
                }

            }
        };

        configurationManager.LoadDocumentConfiguration(Arg.Is<DocumentKey>(x => x.TypeId == DocumentConstants.DocumentTypeTableId),
                                                       Arg.Any<CancellationToken>())
                            .Returns(documentConfiguration);
    }
}