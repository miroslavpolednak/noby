namespace CIS.InternalServices.NotificationService.Api.Services.Repositories.Entities.Abstraction;

public interface ITrackingData
{
    public long Id { get; set; }
    
    public Guid ResultId { get; set; }
    
    public string? CustomId { get; set; }

    public string? ClientId { get; set; }
    
    public string? DocumentId { get; set; }
    
    public Result Result { get; set; }
}