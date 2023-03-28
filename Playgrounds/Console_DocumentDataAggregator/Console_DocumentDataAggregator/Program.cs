using CIS.Core;
using CIS.Core.Configuration;
using CIS.Core.Security;
using CIS.Foms.Enums;
using CIS.InternalServices;
using CIS.InternalServices.DataAggregatorService.Clients;
using CIS.InternalServices.DataAggregatorService.Contracts;
using CIS.InternalServices.DocumentGeneratorService.Clients;
using CIS.InternalServices.DocumentGeneratorService.Contracts;
using Console_CustomerService;
using Console_DocumentDataAggregator;
using Microsoft.Extensions.DependencyInjection;
using _Document = CIS.InternalServices.DocumentGeneratorService.Contracts;
using _DataAggregator = CIS.InternalServices.DataAggregatorService.Contracts;

Console.WriteLine("Hello, World!");

var services = new ServiceCollection();

services.AddTransient<ICurrentUserAccessor, MockCurrentUserAccessor>();
services.AddSingleton<ICisEnvironmentConfiguration>(new CisEnvironmentConfiguration());
services.AddDocumentGeneratorService("https://localhost:5009");
services.AddCisServiceDiscovery("https://localhost:5005");
services.AddDataAggregatorService("https://localhost:5099");

var foundServices = services
                    .Where(t => t is { Lifetime: ServiceLifetime.Singleton, ImplementationInstance: IIsServiceDiscoverable })
                    .Select(t => t.ImplementationInstance as IIsServiceDiscoverable)
                    .Where(t => t is not null && t.UseServiceDiscovery)
                    .ToList();

foundServices.ForEach(instance =>
{
    // nastavit URL ze ServiceDiscovery
    instance!.ServiceUrl = new Uri("https://localhost");
});

var serviceProvider = services.BuildServiceProvider();

var dataAggregatorService = serviceProvider.GetRequiredService<IDataAggregatorServiceClient>();

await GenerateDocument(dataAggregatorService, serviceProvider.GetRequiredService<IDocumentGeneratorServiceClient>());
//await BuildForms(dataAggregatorService);

Console.ReadKey();

static async Task GenerateDocument(IDataAggregatorServiceClient dataAggregatorService, IDocumentGeneratorServiceClient documentGeneratorService)
{
    var inputParameters = new InputParameters { SalesArrangementId = 20002, UserId = 3048 };

    var documentType = DocumentTemplateType.ZADOSTHU;

    var dataRequest = new GetDocumentDataRequest
    {
        DocumentTypeId = (int)DocumentTemplateType.ZADOSTHU,
        DocumentTemplateVariantId = 4,
        InputParameters = inputParameters
    };

    var data = await dataAggregatorService.GetDocumentData(dataRequest);

    var request = new GenerateDocumentRequest
    {
        DocumentTypeId = (int)documentType,
        DocumentTemplateVersion = data.DocumentTemplateVersion,
        DocumentTemplateVariant = data.DocumentTemplateVariant,
        OutputType = OutputFileType.OpenForm,
        Parts =
        {
            new GenerateDocumentPart
            {
                DocumentTypeId = (int)documentType,
                DocumentTemplateVersion = data.DocumentTemplateVersion,
                DocumentTemplateVariant = data.DocumentTemplateVariant,
                Data = { data.DocumentData.Select(CreateDocumentPartData) }
            }

        },
        DocumentFooter = new DocumentFooter
        {
            CaseId = inputParameters.CaseId,
            OfferId = inputParameters.OfferId,
            //ArchiveId = 123456789
        }
    };

    var result = await documentGeneratorService.GenerateDocument(request);

    await using var fileStream = File.Open($"D:\\MPSS\\TestPdf\\Results\\{documentType}.pdf", FileMode.Create);

    result.Data.WriteTo(fileStream);
}

static async Task BuildForms(IDataAggregatorServiceClient dataAggregator)
{
    var request = new GetEasFormRequest
    {
        SalesArrangementId = 1248,
        EasFormRequestType = EasFormRequestType.Product,
        DynamicFormValues =
        {
            new DynamicFormValues
            {
                DocumentTypeId = 4,
                HouseholdId = 123
            }
        }
    };

    var response = await dataAggregator.GetEasForm(request);
}

static GenerateDocumentPartData CreateDocumentPartData(DocumentFieldData fieldData)
{
    var partData = new GenerateDocumentPartData
    {
        Key = fieldData.FieldName,
        StringFormat = fieldData.StringFormat
    };

    switch (fieldData.ValueCase)
    {
        case DocumentFieldData.ValueOneofCase.None:
            break;

        case DocumentFieldData.ValueOneofCase.Text:
            partData.Text = fieldData.Text;
            break;

        case DocumentFieldData.ValueOneofCase.Date:
            partData.Date = fieldData.Date;
            break;

        case DocumentFieldData.ValueOneofCase.Number:
            partData.Number = fieldData.Number;
            break;

        case DocumentFieldData.ValueOneofCase.DecimalNumber:
            partData.DecimalNumber = fieldData.DecimalNumber;
            break;

        case DocumentFieldData.ValueOneofCase.LogicalValue:
            partData.LogicalValue = fieldData.LogicalValue;
            break;

        case DocumentFieldData.ValueOneofCase.Table:
            partData.Table = new _Document.GenericTable
            {
                Rows =
                    {
                        fieldData.Table.Rows.Select(row => new _Document.GenericTableRow
                        {
                            Values = { row.Values.Select(CreateTableRowValue) }
                        })
                    },
                Columns =
                    {
                        fieldData.Table.Columns.Select(column => new _Document.GenericTableColumn
                        {
                            Header = column.Header,
                            StringFormat = column.StringFormat,
                            WidthPercentage = column.WidthPercentage
                        })
                    },
                ConcludingParagraph = fieldData.Table.ConcludingParagraph
            };
            break;

        default:
            throw new NotImplementedException();
    }

    return partData;
}

static _Document.GenericTableRowValue CreateTableRowValue(_DataAggregator.GenericTableRowValue value)
{
    var tableRowValue = new _Document.GenericTableRowValue();

    switch (value.ValueCase)
    {
        case _DataAggregator.GenericTableRowValue.ValueOneofCase.None:
            break;

        case _DataAggregator.GenericTableRowValue.ValueOneofCase.Text:
            tableRowValue.Text = value.Text;
            break;

        case _DataAggregator.GenericTableRowValue.ValueOneofCase.Date:
            tableRowValue.Date = value.Date;
            break;

        case _DataAggregator.GenericTableRowValue.ValueOneofCase.Number:
            tableRowValue.Number = value.Number;
            break;

        case _DataAggregator.GenericTableRowValue.ValueOneofCase.DecimalNumber:
            tableRowValue.DecimalNumber = value.DecimalNumber;
            break;

        default:
            throw new NotImplementedException();
    }

    return tableRowValue;
}