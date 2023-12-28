using CIS.Core.Attributes;
using CIS.InternalServices.DataAggregatorService.Clients;
using CIS.InternalServices.DataAggregatorService.Contracts;
using CIS.InternalServices.DocumentGeneratorService.Clients;
using CIS.InternalServices.DocumentGeneratorService.Contracts;
using _Document = CIS.InternalServices.DocumentGeneratorService.Contracts;
using GenericTableRowValue = CIS.InternalServices.DataAggregatorService.Contracts.GenericTableRowValue;

namespace NOBY.Api.Endpoints.Document.SharedDto;

[TransientService, SelfService]
internal sealed class DocumentGenerator
{
    private readonly IDataAggregatorServiceClient _dataAggregator;
    private readonly IDocumentGeneratorServiceClient _documentGenerator;

    public DocumentGenerator(IDataAggregatorServiceClient dataAggregator, IDocumentGeneratorServiceClient documentGenerator)
    {
        _dataAggregator = dataAggregator;
        _documentGenerator = documentGenerator;
    }

    public async Task<GenerateDocumentRequest> CreateRequest(GetDocumentBaseRequest request, CancellationToken cancellationToken)
    {
        var documentData = await GenerateDocumentData(request, cancellationToken);

        return Create(request, documentData);
    }

    public async Task<ReadOnlyMemory<byte>> GenerateDocument(GenerateDocumentRequest generateDocumentRequest, CancellationToken cancellationToken)
    {
        var document = await _documentGenerator.GenerateDocument(generateDocumentRequest, cancellationToken);

        return document.Data.Memory;
    }

    private async Task<ICollection<DocumentFieldData>> GenerateDocumentData(GetDocumentBaseRequest documentRequest, CancellationToken cancellationToken)
    {
        var documentDataRequest = new GetDocumentDataRequest
        {
            DocumentTypeId = (int)documentRequest.DocumentType,
            InputParameters = documentRequest.InputParameters
        };

        var result = await _dataAggregator.GetDocumentData(documentDataRequest, cancellationToken);

        documentRequest.DocumentTemplateVersionId = result.DocumentTemplateVersionId;
        documentRequest.DocumentTemplateVariantId = result.DocumentTemplateVariantId;
        documentRequest.InputParameters = result.InputParameters;

        return result.DocumentData;
    }

    private static GenerateDocumentRequest Create(GetDocumentBaseRequest request, IEnumerable<DocumentFieldData> documentData) =>
        new()
        {
            DocumentTypeId = (int)request.DocumentType,
            DocumentTemplateVersionId = request.DocumentTemplateVersionId,
            DocumentTemplateVariantId = request.DocumentTemplateVariantId,
            ForPreview = request.ForPreview,
            OutputType = OutputFileType.Pdfa,
            Parts =
            {
                new GenerateDocumentPart
                {
                    DocumentTypeId = (int)request.DocumentType,
                    DocumentTemplateVersionId = request.DocumentTemplateVersionId,
                    DocumentTemplateVariantId = request.DocumentTemplateVariantId,
                    Data = { documentData.Select(CreateDocumentPartData) }
                }
            },
            DocumentFooter = new DocumentFooter
            {
                CaseId = request.InputParameters.CaseId,
                OfferId = request.InputParameters.OfferId,
                SalesArrangementId = request.InputParameters.SalesArrangementId
            }
        };

    private static GenerateDocumentPartData CreateDocumentPartData(DocumentFieldData fieldData)
    {
        var fieldDataWrapper = new DocumentFieldDataWrapper(fieldData);

        var documentData = fieldDataWrapper.CreateDocumentData();

        var valueWasSet = documentData.TrySetValue(fieldDataWrapper);

        if (valueWasSet)
            return documentData;

        if (fieldData.ValueCase != DocumentFieldData.ValueOneofCase.Table)
            throw new NotSupportedException($"ValueCase {fieldData.ValueCase} is not supported");

        documentData.Table = new _Document.GenericTable
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

        return documentData;
    }

    private static _Document.GenericTableRowValue CreateTableRowValue(GenericTableRowValue value)
    {
        var tableRowValue = new _Document.GenericTableRowValue();

        switch (value.ValueCase)
        {
            case GenericTableRowValue.ValueOneofCase.None:
                break;

            case GenericTableRowValue.ValueOneofCase.Text:
                tableRowValue.Text = value.Text;
                break;

            case GenericTableRowValue.ValueOneofCase.Date:
                tableRowValue.Date = value.Date;
                break;

            case GenericTableRowValue.ValueOneofCase.Number:
                tableRowValue.Number = value.Number;
                break;

            case GenericTableRowValue.ValueOneofCase.DecimalNumber:
                tableRowValue.DecimalNumber = value.DecimalNumber;
                break;

            default:
                throw new NotImplementedException();
        }

        return tableRowValue;
    }
}