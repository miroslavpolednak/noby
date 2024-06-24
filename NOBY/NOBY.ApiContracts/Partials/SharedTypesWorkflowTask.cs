namespace NOBY.ApiContracts;

public partial class SharedTypesWorkflowTask
{
    [JsonIgnore]
    public bool Cancelled { get; set; }
}
