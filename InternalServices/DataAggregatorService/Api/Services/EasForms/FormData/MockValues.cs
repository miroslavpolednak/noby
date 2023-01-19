namespace CIS.InternalServices.DataAggregatorService.Api.Services.EasForms.FormData;

public class MockValues
{
    public string UserCPM => "99806569";

    public string UserICP => "114306569";

    public int DefaultOneValue => 1;

    public int DefaultZeroValue => 0;

    public bool DefaultFalseValue => false;

    public object[] ListIdForm => new object[] { new { Id = 0 } };

    public string MockDocumentId => "9876543210";
}