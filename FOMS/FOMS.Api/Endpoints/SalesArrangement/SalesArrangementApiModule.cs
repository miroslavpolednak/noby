using CIS.Infrastructure.WebApi;
using Microsoft.AspNetCore.Mvc;

namespace FOMS.Api.Endpoints.Forms;

internal class SalesArrangementApiModule : IApiEndpointModule
{
    const string _prefix = "/api/sales-arrangements";

    public void Register(IEndpointRouteBuilder builder)
    {
        var mediatr = builder.ServiceProvider.GetRequiredService<IMediator>();

        // GET /forms/{caseid}/{formid}/structure - vrati strukturu formu do wizarda
        // GET /forms/{said}/{formid}/{partid} - vrati template dane casti formu
        // PUT /forms/{said}/{formid}/{partid} - ulozi cast formu
    }
}
