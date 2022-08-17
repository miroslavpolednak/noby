﻿using Swashbuckle.AspNetCore.Annotations;

namespace FOMS.Api.Endpoints.Customer;

[ApiController]
[Route("api")]
public class CustomerController : ControllerBase
{
    /// <summary>
    /// Vyhledavani klientu
    /// </summary>
    /// <remarks>
    /// Endpoint umoznuje:
    /// - hledat podle zadanych kriterii
    /// - nastavit strankovani
    /// - nastavit razeni [lastName]
    /// <i>DS:</i> CustomerService/SearchCustomers
    /// </remarks>
    /// <returns>Seznam nalezenych klientu. BE sluzba neni strankovatelna, takze strankovani je jen jako fake na FE.</returns>
    [HttpPost("customer/search")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [SwaggerOperation(Tags = new [] { "UC: Identifikace klienta", "UC: Domacnost" })]
    [ProducesResponseType(typeof(Search.SearchResponse), StatusCodes.Status200OK)]
    public async Task<Search.SearchResponse> Search([FromBody] Search.SearchRequest resquest, CancellationToken cancellationToken)
        => await _mediator.Send(resquest, cancellationToken);
    
    /// <summary>
    /// Detail klienta
    /// </summary>
    /// <remarks>
    /// <i>DS:</i> CustomerService/GetCustomer
    /// </remarks>
    /// <param name="identityId">ID klienta v danem schematu</param>
    /// <param name="identityScheme">Schema ve kterem je klient ulozeny - Kb | Mp</param>
    /// <returns>Kompletni detail klienta vraceny z KB CM nebo KonsDb.</returns>
    [HttpGet("customer/get")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new [] { "UC: Identifikace klienta", "UC: Domacnost" })]
    [ProducesResponseType(typeof(GetDetail.GetDetailResponse), StatusCodes.Status200OK)]
    public async Task<GetDetail.GetDetailResponse> GetDetail([FromQuery] long identityId, [FromQuery] CIS.Foms.Enums.IdentitySchemes identityScheme, CancellationToken cancellationToken)
        => await _mediator.Send(new GetDetail.GetDetailRequest(identityId, identityScheme), cancellationToken);

    /// <summary>
    /// Identifikace klienta
    /// </summary>
    /// <remarks>
    /// Slouzi pro idenfifikaci klienta.<br />Možné použití pro hlavního dlužníka i pro spoludlužníka, na Domácnosti, na Modelaci hypotéky.<br/>
    /// <a href="https://eacloud.ds.kb.cz/webea?m=1&amp;o=EF40D23F-A77A-4a04-AA79-38779970393E"><img src="https://eacloud.ds.kb.cz/webea/images/element64/diagramactivity.png" width="20" height="20" />Diagram v EA</a>
    /// </remarks>
    [HttpPost("customer-on-sa/{customerOnSAId:int}/identify")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [SwaggerOperation(Tags = new[] { "UC: Modelace Hypoteky", "Klient", "UC: Domacnost" })]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task Identify([FromRoute] int customerOnSAId, [FromBody] Identify.IdentifyRequest request, CancellationToken cancellationToken)
        => await _mediator.Send(request.InfuseId(customerOnSAId), cancellationToken);

    /// <summary>
    /// Profile check s profilem identifikovaný
    /// </summary>
    /// <remarks>
    /// Provolá <i>DS: CustomerService/profileCheck</i> s profilem 'Subjekt s identifikací' a vrátí informaci, zda profil splňuje.
    /// </remarks>
    [HttpPost("customer/profile-check")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [SwaggerOperation(Tags = new[] { "UC: Identifikace klienta", "UC: Modelace Hypoteky", "UC: Domacnost" })]
    [ProducesResponseType(typeof(ProfileCheck.ProfileCheckResponse), StatusCodes.Status200OK)]
    public async Task<ProfileCheck.ProfileCheckResponse> ProfileCheck([FromBody] ProfileCheck.ProfileCheckRequest request, CancellationToken cancellationToken)
        => await _mediator.Send(request, cancellationToken);

    private readonly IMediator _mediator;
    public CustomerController(IMediator mediator) =>  _mediator = mediator;
}