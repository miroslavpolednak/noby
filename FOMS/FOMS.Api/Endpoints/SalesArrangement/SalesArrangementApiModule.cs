using CIS.Infrastructure.WebApi;
using Microsoft.AspNetCore.Mvc;

namespace FOMS.Api.Endpoints.SalesArrangement;

internal class SalesArrangementApiModule : IApiEndpointModule
{
    const string _prefix = "/api/sales-arrangement";

    public void Register(IEndpointRouteBuilder builder)
    {
        var mediatr = builder.ServiceProvider.GetRequiredService<IMediator>();

        // vraci seznam SA pro dany case
        builder
            .MapGet(_prefix + "/list/{caseId:long}", async (long caseId) => await mediatr.Send(new Dto.GetListRequest(caseId)))
            .WithTags("Sales Arrangement Module")
            .Produces<List<Dto.SalesArrangementListItem>>(StatusCodes.Status200OK);
        
        // vraci seznam customers pro dany SA
        builder
            .MapGet(_prefix + "/{salesArrangementId:int}/customers", async (int salesArrangementId) => await mediatr.Send(new Dto.GetCustomersRequest(salesArrangementId)))
            .WithTags("Sales Arrangement Module")
            .Produces<Dto.CustomerListItem>(StatusCodes.Status200OK);
    }
}
