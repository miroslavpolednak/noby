using CIS.InternalServices.DocumentGeneratorService.Api.AcroForm.AcroFieldFormat;

namespace CIS.InternalServices.DocumentGeneratorService.Api.AcroForm.AcroFormWriter;

[SingletonService, SelfService]
public class PdfAcroFormWriterFactory
{
    private readonly AcroFieldFormatProvider _fieldFormatProvider;

    public PdfAcroFormWriterFactory(AcroFieldFormatProvider fieldFormatProvider)
    {
        _fieldFormatProvider = fieldFormatProvider;
    }

    public IAcroFormWriter Create(IEnumerable<GenerateDocumentPartData> values)
    {
        var valuesByTable = values.ToLookup(v => v.ValueCase == GenerateDocumentPartData.ValueOneofCase.Table);

        var tables = valuesByTable[true];
        var otherValues = valuesByTable[false];

        //True when Table exists in request
        return tables.Any() ? CreateTableWriter(tables, otherValues) : CreateBasic(otherValues);
    }

    private IAcroFormWriter CreateBasic(IEnumerable<GenerateDocumentPartData> values) => new BasicAcroFormWriter(_fieldFormatProvider, values);

    private IAcroFormWriter CreateTableWriter(IEnumerable<GenerateDocumentPartData> tables, IEnumerable<GenerateDocumentPartData> values)
    {
        if (tables.Skip(1).Any())
            throw new InvalidOperationException("Multiple tables are not supported.");

        return new TableAcroFormWriter(_fieldFormatProvider, tables.First(), CreateBasic(values));
    }
}