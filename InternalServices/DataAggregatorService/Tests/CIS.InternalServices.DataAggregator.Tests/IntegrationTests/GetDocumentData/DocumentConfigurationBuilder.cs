using CIS.InternalServices.DataAggregatorService.Api.Configuration.Common;
using CIS.InternalServices.DataAggregatorService.Api.Configuration.Data;
using CIS.InternalServices.DataAggregatorService.Api.Configuration.Data.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace CIS.InternalServices.DataAggregator.Tests.IntegrationTests.GetDocumentData;

public class DocumentConfigurationBuilder
{
    private readonly ConfigurationContext _dbContext;

    public DocumentConfigurationBuilder(IServiceProvider serviceProvider)
    {
        _dbContext = serviceProvider.GetRequiredService<ConfigurationContext>();
    }



    public DocumentConfigurationBuilder DataFields()
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

        _dbContext.DataFields
                  .AddRange(new DataField { DataFieldId = 1, DataServiceId = 1, FieldPath = "SalesArrangement.ContractNumber" },
                            new DataField { DataFieldId = 2, DataServiceId = 3, FieldPath = "Offer.SimulationInputs.LoanAmount", DefaultStringFormat = "{0:C0}" },
                            new DataField { DataFieldId = 3, DataServiceId = 8, FieldPath = "HouseholdMain.Household.Data.ChildrenUpToTenYearsCount" },
                            new DataField { DataFieldId = 4, DataServiceId = 9, FieldPath = "HouseholdMain.Debtor.CustomerAdditionalData.HasRelationshipWithKB" },
                            new DataField { DataFieldId = 5, DataServiceId = 9, FieldPath = "HouseholdMain.Codebtor.CustomerAdditionalData.HasRelationshipWithKB" },
                            new DataField { DataFieldId = 6, DataServiceId = 1, FieldPath = "SalesArrangement.OfferId" },
                            new DataField { DataFieldId = 7, DataServiceId = 3, FieldPath = "Offer.AdditionalSimulationResults.PaymentScheduleSimple[].PaymentNumber" },
                            new DataField { DataFieldId = 8, DataServiceId = 7, FieldPath = "OfferPaymentSchedule.PaymentScheduleFull[]" });

        _dbContext.InputParameters.Add(new InputParameter { InputParameterId = 3, InputParameterName = "OfferId" });

        return this;

        static DataService CreateDataService(DataSource dataSource) => new() { DataServiceId = (int)dataSource, DataServiceName = dataSource.ToString() };
    }

    public DocumentConfigurationBuilder DocumentFields()
    {
        _dbContext.DocumentDataFields
                  .AddRange(new DocumentDataField { DocumentDataFieldId = 1, DocumentId = DocumentConstants.DocumentTypeId, DocumentVersion = DocumentConstants.DocumentTemplateVersion, DataFieldId = 1, AcroFieldName = "RegCislo" },
                            new DocumentDataField { DocumentDataFieldId = 2, DocumentId = DocumentConstants.DocumentTypeId, DocumentVersion = DocumentConstants.DocumentTemplateVersion, DataFieldId = 2, AcroFieldName = "VyseUveru" },
                            new DocumentDataField { DocumentDataFieldId = 3, DocumentId = DocumentConstants.DocumentTypeId, DocumentVersion = DocumentConstants.DocumentTemplateVersion, DataFieldId = 3, AcroFieldName = "PocetDetiDo10" },
                            new DocumentDataField { DocumentDataFieldId = 4, DocumentId = DocumentConstants.DocumentTypeId, DocumentVersion = DocumentConstants.DocumentTemplateVersion, DataFieldId = 4, AcroFieldName = "JsemNejsem1Odrazka" },
                            new DocumentDataField { DocumentDataFieldId = 5, DocumentId = DocumentConstants.DocumentTypeId, DocumentVersion = DocumentConstants.DocumentTemplateVersion, DataFieldId = 5, AcroFieldName = "SpoluzadatelJsemNejsem1Odrazka" },
                            new DocumentDataField { DocumentDataFieldId = 6, DocumentId = DocumentConstants.DocumentTypeId, DocumentVersion = DocumentConstants.DocumentTemplateVersion, DataFieldId = 7, AcroFieldName = "CisloSplatky" });

        _dbContext.DocumentDataFieldVariants
                  .AddRange(new DocumentDataFieldVariant { DocumentDataFieldId = 1, DocumentVariant = DocumentConstants.DocumentTemplateVariant },
                            new DocumentDataFieldVariant { DocumentDataFieldId = 2, DocumentVariant = DocumentConstants.DocumentTemplateVariant },
                            new DocumentDataFieldVariant { DocumentDataFieldId = 3, DocumentVariant = DocumentConstants.DocumentTemplateVariant },
                            new DocumentDataFieldVariant { DocumentDataFieldId = 4, DocumentVariant = DocumentConstants.DocumentTemplateVariant },
                            new DocumentDataFieldVariant { DocumentDataFieldId = 5, DocumentVariant = DocumentConstants.DocumentTemplateVariant },
                            new DocumentDataFieldVariant { DocumentDataFieldId = 6, DocumentVariant = DocumentConstants.DocumentTemplateVariant });

        _dbContext.DocumentDynamicInputParameters.Add(new DocumentDynamicInputParameter { DocumentId = DocumentConstants.DocumentTypeId, DocumentVersion = DocumentConstants.DocumentTemplateVersion, InputParameterId = 3, TargetDataServiceId = 3, SourceDataFieldId = 6 });

        return this;
    }

    public DocumentConfigurationBuilder Table()
    {
        _dbContext.DocumentTables.Add(new DocumentTable
        {
            DocumentTableId = 1,
            DocumentId = DocumentConstants.DocumentTypeTableId,
            DocumentVersion = DocumentConstants.DocumentTemplateVersion,
            DataFieldId = 8,
            AcroFieldPlaceholderName = "SplatkovyKalendar",
            ConcludingParagraph = "Text"
        });

        _dbContext.DocumentTableColumns.Add(new DocumentTableColumn { DocumentTableId = 1, FieldPath = "Amount", Order = 3, Header = "Header 1" });
        _dbContext.DocumentTableColumns.Add(new DocumentTableColumn { DocumentTableId = 1, FieldPath = "PaymentNumber", Order = 3, Header = "Header 2" });

        _dbContext.DocumentDynamicInputParameters.Add(new DocumentDynamicInputParameter { DocumentId = DocumentConstants.DocumentTypeTableId, DocumentVersion = DocumentConstants.DocumentTemplateVersion, InputParameterId = 3, TargetDataServiceId = 7, SourceDataFieldId = 6 });

        return this;
    }

    public async Task Build()
    {
        await _dbContext.SaveChangesAsync();
    }
}