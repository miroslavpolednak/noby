namespace DomainServices.SalesArrangementService.Api.Endpoints.SendToCmp;

public class DocumentDataDto
{
    public string FieldName { get; set; } = null!;

    public string? StringFormat { get; set; }

    public string Text { get; set; } = null!;

    public DateOnlyDto? Date { get; set; }

    public int Number { get; set; }

    public DecimalNumberDto? DecimalNumber { get; set; }

    public bool LogicalValue { get; set; }

    /// <summary>
    /// This is not supported yet
    /// </summary>
    public object Table { get; set; } = null!;

    public int ValueCase { get; set; }
}

public class DateOnlyDto
{
    public int Year { get; set; }

    public int Month { get; set; }

    public int Day { get; set; }
}

public class DecimalNumberDto
{
    public long Units { get; set; }

    public int Nanos { get; set; }
}
