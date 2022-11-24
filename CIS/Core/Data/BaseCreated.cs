namespace CIS.Core.Data;

/// <summary>
/// Implementace <see cref="ICreated"/>
/// </summary>
public class BaseCreated 
    : ICreated
{
    public string? CreatedUserName { get; set; }
    
    public int? CreatedUserId { get; set; }
    
    public DateTime CreatedTime { get; set; }
}