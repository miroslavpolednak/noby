using DomainServices.CodebookService.Contracts.Endpoints.RepaymentScheduleTypes;

namespace DomainServices.CodebookService.Endpoints.RepaymentScheduleTypes;

public class RepaymentScheduleTypesHandler
    : IRequestHandler<RepaymentScheduleTypesRequest, List<RepaymentScheduleTypeItem>>
{
    public Task<List<RepaymentScheduleTypeItem>> Handle(RepaymentScheduleTypesRequest request, CancellationToken cancellationToken)
    {
        // TODO: Redirect to real data source!     
        return Task.FromResult(new List<RepaymentScheduleTypeItem>
        {
            new RepaymentScheduleTypeItem() { Id = 1, Name = "Anuitní", Code = "A" },
            new RepaymentScheduleTypeItem() { Id = 2, Name = "Postupné", Code = "P" },
            new RepaymentScheduleTypeItem() { Id = 3, Name = "Jednorázové - jedna splátka", Code = "OS" },
            new RepaymentScheduleTypeItem() { Id = 4, Name = "Jednorázové - více splátek", Code = "OM" },
            new RepaymentScheduleTypeItem() { Id = 5, Name = "Anuitní s mimořádnými splátkami", Code = "AX" },
            new RepaymentScheduleTypeItem() { Id = 6, Name = "Postupné s mimořádnými splátkami", Code = "PX" },
        });
    }

    private readonly CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> _connectionProvider;
    private readonly ILogger<RepaymentScheduleTypesHandler> _logger;

    public RepaymentScheduleTypesHandler(
        CIS.Core.Data.IConnectionProvider<IXxdDapperConnectionProvider> connectionProvider, 
        ILogger<RepaymentScheduleTypesHandler> logger)
    {
        _logger = logger;
        _connectionProvider = connectionProvider;
    }
}