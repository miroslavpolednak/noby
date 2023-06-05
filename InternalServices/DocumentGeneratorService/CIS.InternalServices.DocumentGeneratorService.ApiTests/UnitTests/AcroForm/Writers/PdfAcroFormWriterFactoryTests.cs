using AutoFixture;
using CIS.InternalServices.DocumentGeneratorService.Api.AcroForm.AcroFormWriter;
using CIS.InternalServices.DocumentGeneratorService.Contracts;
using FluentAssertions;
using Xunit;

namespace CIS.InternalServices.DocumentGeneratorService.Tests.UnitTests.AcroForm.Writers;

public class PdfAcroFormWriterFactoryTests
{
    [Fact]
    public void Create_ValuesWithoutTable_ShouldReturnBasicWriter()
    {
        var sut = new Fixture().Create<PdfAcroFormWriterFactory>();
        var values = Enumerable.Range(0, 3).Select(_ => new GenerateDocumentPartData());

        var result = sut.Create(values);

        result.Should().BeOfType<BasicAcroFormWriter>();
    }

    [Fact]
    public void Create_MultipleTable_ShouldThrowInvalidOperation()
    {
        var sut = new Fixture().Create<PdfAcroFormWriterFactory>();
        var values = Enumerable.Range(0, 3).Select(_ => new GenerateDocumentPartData { Table = new GenericTable() });

        var act = () => sut.Create(values);

        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Create_ValuesWithTable_ShouldReturnTableWriter()
    {
        var sut = new Fixture().Create<PdfAcroFormWriterFactory>();
        var values = new[] { new GenerateDocumentPartData { Table = new GenericTable() } };

        var result = sut.Create(values);

        result.Should().BeOfType<TableAcroFormWriter>();
    }
}