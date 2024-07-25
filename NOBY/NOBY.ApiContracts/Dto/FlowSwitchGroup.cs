namespace NOBY.ApiContracts.Dto;

public sealed class FlowSwitchGroup
{
    public bool IsActive { get; set; }

    public bool IsVisible { get; set; }

    //public bool IsCompleted { get; set; }
    public int State { get; set; }
}
