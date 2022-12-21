﻿using CIS.Infrastructure.gRPC.CisTypes;
using CIS.InternalServices.DataAggregator.Documents.Table;
using CIS.InternalServices.DocumentGeneratorService.Clients;
using CIS.InternalServices.DocumentGeneratorService.Contracts;

namespace CIS.InternalServices.DataAggregator.Documents;

public static class DocumentGeneratorExtensions
{
    public static GenerateDocumentPart FillDocumentPart(this GenerateDocumentPart documentPart, IEnumerable<DocumentFieldData> values)
    {
        foreach (var documentValue in values)
        {
            var partData = new GenerateDocumentPartData
            {
                Key = documentValue.FieldName,
                StringFormat = documentValue.StringFormat
            }.SetDocumentPartDataValue(documentValue.Value);

            documentPart.Data.Add(partData);
        }

        return documentPart;
    }

    public static GenerateDocumentPartData SetDocumentPartDataValue(this GenerateDocumentPartData partData, object value)
    {
        if (partData.TrySetCommonData(value))
            return partData;

        if (value is DocumentTable table) 
            partData.Table = CreateTable(table);

        return partData;
    }

    private static GenericTable CreateTable(DocumentTable documentTable)
    {
        return new GenericTable
        {
            Columns =
            {
                documentTable.Columns.Select(c => new GenericTableColumn
                {
                    Header = c.Header,
                    StringFormat = c.StringFormat,
                    WidthPercentage = (GrpcDecimal)c.WidthPercentage
                })
            },
            Rows =
            {
                documentTable.RowsValues.Select(row => new GenericTableRow
                {
                    Values =
                    {
                        row.Select(obj => SetGenericTableRowValue(new GenericTableRowValue(), obj))
                    }
                })
            },
            ConcludingParagraph = documentTable.ConcludingParagraph
        };
    }

    private static GenericTableRowValue SetGenericTableRowValue(GenericTableRowValue rowValue, object? obj)
    {
        if (rowValue.TrySetCommonData(obj))
            return rowValue;

        throw new NotImplementedException();
    }
}