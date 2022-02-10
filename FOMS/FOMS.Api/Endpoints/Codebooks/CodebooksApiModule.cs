﻿using CIS.Infrastructure.WebApi;
using DomainServices.CodebookService.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace FOMS.Api.Endpoints.Codebooks;

internal class CodebooksApiModule : IApiEndpointModule
{
    const string _prefix = "/api/codebooks";

    public void Register(IEndpointRouteBuilder builder)
    {
        var mediatr = builder.ServiceProvider.GetRequiredService<IMediator>();

        // vraci vsechny vyzadane ciselniky
        // q = kody ciselniku oddelene ","
        builder.MapGet(_prefix + "/get-all", async ([FromQuery(Name = "q")] string codebookTypes) => await mediatr.Send(new Dto.GetAllRequest(codebookTypes)))
            .WithTags("Codebooks Module")
            .Produces<Dto.GetAllResponseItem>(StatusCodes.Status200OK);

        // fixace uveru
        builder.MapGet(_prefix + "/fixation-period-length", 
            async ([FromQuery] int productTypeId, [FromServices] ICodebookServiceAbstraction svc) => 
                (await svc.FixedLengthPeriods())
                    .Where(t => t.ProductTypeId == productTypeId)
                    .Select(t => t.FixedLengthPeriod)
                    .OrderBy(t => t)
            )
            .WithTags("Codebooks Module")
            .Produces<List<int>>(StatusCodes.Status200OK);

        // druhy uveru
        builder.MapGet(_prefix + "/product-loan-kinds",
            async ([FromQuery] int productTypeId, [FromServices] ICodebookServiceAbstraction svc) =>
                (await svc.ProductLoanKinds())
                    .Where(t => t.IsValid && t.ProductTypeId == productTypeId)
                    .OrderBy(t => t.Name)
            )
            .WithTags("Codebooks Module")
            .Produces<List<DomainServices.CodebookService.Contracts.Endpoints.ProductLoanKinds.ProductLoanKindsItem>>(StatusCodes.Status200OK);
    }
}
