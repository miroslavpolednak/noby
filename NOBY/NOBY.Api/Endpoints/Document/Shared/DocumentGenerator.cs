using CIS.Core.Attributes;
using CIS.InternalServices.DataAggregatorService.Clients;
using CIS.InternalServices.DataAggregatorService.Contracts;
using CIS.InternalServices.DocumentGeneratorService.Clients;
using _Document = CIS.InternalServices.DocumentGeneratorService.Contracts;

namespace NOBY.Api.Endpoints.Document.Shared;

[TransientService, SelfService]
internal class DocumentGenerator
{
    private readonly IDataAggregatorServiceClient _dataAggregator;
    private readonly IDocumentGeneratorServiceClient _documentGenerator;

    public DocumentGenerator(IDataAggregatorServiceClient dataAggregator, IDocumentGeneratorServiceClient documentGenerator)
    {
        _dataAggregator = dataAggregator;
        _documentGenerator = documentGenerator;
    }

    public async Task<_Document.GenerateDocumentRequest> CreateRequest(GetDocumentBaseRequest request, CancellationToken cancellationToken)
    {
        var documentData = await GenerateDocumentData(request, cancellationToken);

        return Create(request, documentData);
    }

    public async Task<ReadOnlyMemory<byte>> GenerateDocument(_Document.GenerateDocumentRequest generateDocumentRequest, CancellationToken cancellationToken)
    {
        var document = await _documentGenerator.GenerateDocument(generateDocumentRequest, cancellationToken);

        return document.Data.Memory;
    }

    private async Task<ICollection<DocumentFieldData>> GenerateDocumentData(GetDocumentBaseRequest documentRequest, CancellationToken cancellationToken)
    {
        var documentDataRequest = new GetDocumentDataRequest
        {
            DocumentTypeId = (int)documentRequest.DocumentType,
            DocumentTemplateVariantId = documentRequest.DocumentTemplateVariantId,
            InputParameters = documentRequest.InputParameters
        };

        var result = await _dataAggregator.GetDocumentData(documentDataRequest, cancellationToken);

        documentRequest.DocumentTemplateVersionId = result.DocumentTemplateVersionId;
        documentRequest.InputParameters = result.InputParameters;

        return result.DocumentData;
    }

    private static _Document.GenerateDocumentRequest Create(GetDocumentBaseRequest request, IEnumerable<DocumentFieldData> documentData) =>
        new()
        {
            DocumentTypeId = (int)request.DocumentType,
            DocumentTemplateVersionId = request.DocumentTemplateVersionId,
            DocumentTemplateVariantId = request.DocumentTemplateVariantId,
            ForPreview = request.ForPreview,
            OutputType = _Document.OutputFileType.Pdfa,
            Parts =
            {
                new _Document.GenerateDocumentPart
                {
                    DocumentTypeId = (int)request.DocumentType,
                    DocumentTemplateVersionId = request.DocumentTemplateVersionId,
                    DocumentTemplateVariantId = request.DocumentTemplateVariantId,
                    Data = { documentData.Select(CreateDocumentPartData) }
                }
            },
            DocumentFooter = new _Document.DocumentFooter
            {
                CaseId = request.InputParameters.CaseId,
                OfferId = request.InputParameters.OfferId
            }
        };

    private static _Document.GenerateDocumentPartData CreateDocumentPartData(DocumentFieldData fieldData)
    {
        var partData = new _Document.GenerateDocumentPartData
        {
            Key = fieldData.FieldName,
            StringFormat = fieldData.StringFormat,
            TextAlign = (_Document.TextAlign)(fieldData.TextAlign ?? 0)
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