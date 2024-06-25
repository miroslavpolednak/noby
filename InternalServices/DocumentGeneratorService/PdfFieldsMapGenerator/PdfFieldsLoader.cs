using ceTe.DynamicPDF;
using ceTe.DynamicPDF.Forms;
using ceTe.DynamicPDF.Merger;
using ceTe.DynamicPDF.Merger.Forms;
using CIS.InternalServices.DocumentGeneratorService.Api.Model.PdfMap;

namespace CIS.InternalServices.DocumentGeneratorService.PdfFieldsMapGenerator;

public class PdfFieldsLoader
{
    private PdfFieldsLoader()
    {
    }

    public static PdfFieldsLoader Instance { get; } = new();

    public IEnumerable<DocumentMapItem> LoadDocumentMap(PdfDocument pdfDocument, MergeDocument mergeDocument)
    {
        foreach (FormField formField in mergeDocument.Form.Fields)
        {
            var acroField = pdfDocument.Form.Fields[formField.FullName];

            if (acroField.HasChildFields)
            {
                yield return CreateDocumentMapItemWithChildren(mergeDocument, formField, acroField);
            }
            else
            {
                var page = mergeDocument.Pages[acroField.GetOriginalPageNumber() - 1];

                yield return new DocumentMapItem
                {
                    Field = CreateAcroFieldInfo(acroField, page)
                };
            }
        }
    }

    private static DocumentMapItem CreateDocumentMapItemWithChildren(MergeDocument mergeDocument, FormField formField, PdfFormField acroField)
    {
        var childFields = formField.ChildFields.OfType<FormField>().Select((_, i) =>
        {
            var childAcroField = acroField.ChildFields[i];
            var page = mergeDocument.Pages[childAcroField.GetOriginalPageNumber() - 1];

            return CreateAcroFieldInfo(childAcroField, page);
        }).DistinctBy(c => c.Coordinates).ToList();

        return new DocumentMapItem
        {
            Field = childFields.Count == 1 ? childFields.First() with { PageNumber = -1 } : childFields.First(),
            ChildFields = childFields.Count == 1 ? null : childFields
        };
    }

    private static FieldInfo CreateAcroFieldInfo(PdfFormField acroField, Page page) =>
        new()
        {
            Name = acroField.FullName,
            PageNumber = acroField.GetOriginalPageNumber(),
            FontName = acroField.Font.Name,
            FontSize = acroField.FontSize,
            Coordinates = new FieldCoordinates
            {
                X = acroField.GetX(page),
                Y = acroField.GetY(page),
                Width = acroField.Width,
                Height = acroField.Height,
            }
        };

}