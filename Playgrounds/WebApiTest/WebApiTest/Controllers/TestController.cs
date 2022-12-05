using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApiTest.Controllers;
[ApiController]
[Route("test")]
public class TestController : ControllerBase
{
    private readonly IMediator _mediator;
    public TestController(IMediator mediator) => _mediator = mediator;

    public IHttpContextAccessor Accessor { get; }

    [HttpGet(Name = "t1")]
    public async Task<string> t1()
        => await _mediator.Send(new req1());
}

public class req1 : IRequest<string> { }
public class han1 : IRequestHandler<req1, string>
{
    private readonly IHttpContextAccessor _accessor;
    public han1(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public async Task<string> Handle(req1 request, CancellationToken cancellation)
    {
        var svc = _accessor.HttpContext.RequestServices.GetRequiredService<DomainServices.UserService.Clients.IUserServiceClient>();
        var svc2 = _accessor.HttpContext.RequestServices.GetRequiredService<DomainServices.HouseholdService.Clients.IHouseholdServiceClient>();

        var user = await svc.GetUserByLogin("990614w");

        return user.FullName;
    }
}