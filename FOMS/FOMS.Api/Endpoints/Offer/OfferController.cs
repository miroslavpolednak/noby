using Swashbuckle.AspNetCore.Annotations;

namespace FOMS.Api.Endpoints.Offer;

[ApiController]
[Route("api/offer")]
public class OfferController : ControllerBase
{
    private readonly IMediator _mediator;
    public OfferController(IMediator mediator) =>  _mediator = mediator;
    
    /// <summary>
    /// Simulace KB hypoteky.
    /// </summary>
    /// <remarks>
    /// DS: OfferService/SimulateMortgage
    /// </remarks>
    /// <param name="request">Nastaveni simulace.</param>
    /// <returns>ID vytvorene simulace a jejich vysledky.</returns>
    [HttpPost("mortgage")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [SwaggerOperation(Tags = new [] { "UC: Modelace Hypoteky" })]
    [ProducesResponseType(typeof(SimulateMortgage.SimulateMortgageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<SimulateMortgage.SimulateMortgageResponse> SimulateMortgage([FromBody] SimulateMortgage.SimulateMortgageRequest request, CancellationToken cancellationToken)
        => await _mediator.Send(request, cancellationToken);
    
    /// <summary>
    /// Detail provedene simulace dle ID simulace.
    /// </summary>
    /// <remarks>
    /// DS: OfferService/GetMortgageData
    /// </remarks>
    /// <returns>Vstupy a vystupy ulozene simulace.</returns>
    [HttpGet("mortgage/{offerId:int}")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new [] { "UC: Modelace Hypoteky" })]
    [ProducesResponseType(typeof(Dto.GetMortgageResponse), StatusCodes.Status200OK)]
    public async Task<Dto.GetMortgageResponse> GetMortgageByOfferId([FromRoute] int offerId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetMortgageByOfferId.GetMortgageByOfferIdRequest(offerId), cancellationToken);
    
    /// <summary>
    /// Detail provedene simulace dle ID Sales Arrangement.
    /// </summary>
    /// <remarks>
    /// Stejny endpoint jako GetMortgageByOfferId, jen podle jineho ID.
    /// DS: SalesArrangementService/GetSalesArrangement (to get OfferId)
    /// DS: OfferService/GetMortgageData
    /// </remarks>
    /// <returns>Vstupy a vystupy ulozene simulace.</returns>
    [HttpGet("mortgage/sales-arrangement/{salesArrangementId:int}")]
    [Produces("application/json")]
    [SwaggerOperation(Tags = new [] { "UC: Modelace Hypoteky" })]
    [ProducesResponseType(typeof(Dto.GetMortgageResponse), StatusCodes.Status200OK)]
    public async Task<Dto.GetMortgageResponse> GetMortgageBySalesArrangementId([FromRoute] int salesArrangementId, CancellationToken cancellationToken)
        => await _mediator.Send(new GetMortgageBySalesArrangement.GetMortgageBySalesArrangementRequest(salesArrangementId), cancellationToken);
    
    /// <summary>
    /// Vytvoreni noveho pripadu (hypoteky) ze simulace.
    /// </summary>
    /// <remarks>
    /// Vytvori novy Case a Sales Arrangement.
    /// DS:
    /// </remarks>
    /// <param name="request">Identifikace klienta a ID simulace.</param>
    /// <returns>ID nove vytvoreneho Case</returns>
    [HttpPost("mortgage/create-case")]
    [Produces("application/json")]
    [Consumes("application/json")]
    [SwaggerOperation(Tags = new [] { "UC: Modelace Hypoteky" })]
    [ProducesResponseType(typeof(CreateMortgageCase.CreateMortgageCaseResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<CreateMortgageCase.CreateMortgageCaseResponse> CreateMortgageCase([FromBody] CreateMortgageCase.CreateMortgageCaseRequest request, CancellationToken cancellationToken)
        => await _mediator.Send(request, cancellationToken);
}