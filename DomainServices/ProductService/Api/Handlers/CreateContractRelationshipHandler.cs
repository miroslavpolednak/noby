using ExternalServices.MpHome.V1._1;
using ExternalServices.MpHome.V1._1.MpHomeWrapper;
using DomainServices.CodebookService.Abstraction;

namespace DomainServices.ProductService.Api.Handlers;

internal class CreateContractRelationshipHandler
    : BaseMortgageHandler, IRequestHandler<Dto.CreateContractRelationshipMediatrRequest, Google.Protobuf.WellKnownTypes.Empty>
{
    #region Construction

    public CreateContractRelationshipHandler(
        ICodebookServiceAbstraction codebookService,
        Repositories.LoanRepository repository,
        IMpHomeClient mpHomeClient,
        ILogger<CreateContractRelationshipHandler> logger) : base(codebookService, repository, mpHomeClient, logger)
    {}

    #endregion

    public async Task<Google.Protobuf.WellKnownTypes.Empty> Handle(Dto.CreateContractRelationshipMediatrRequest request, CancellationToken cancellation)
    {
        
        _logger.RequestHandlerStarted(nameof(CreateContractRelationshipHandler));

        // check if relationship not exists
        var relationshipExists = await _repository.ExistsRelationship(request.Request.ProductId, request.Request.Relationship.PartnerId, cancellation);
        if (relationshipExists)
        {
            throw new CisAlreadyExistsException(13015, nameof(Repositories.Entities.Relationship)); //TODO: error code
        }

        // TODO: check existension of KB entities (ProductId, PartnerId) 
        // --------------------------------------------------------------------------------------------------------------------------

        // check if loan exists
        var loanExists = await _repository.ExistsLoan(request.Request.ProductId, cancellation);
        if (!loanExists)
        {
            throw new CisNotFoundException(13014, nameof(Repositories.Entities.Loan), request.Request.ProductId); //TODO: error code
        }

        // check if partner exists
        var partnerExists = await _repository.ExistsPartner(request.Request.Relationship.PartnerId, cancellation);
        if (!partnerExists)
        {
            throw new CisNotFoundException(13014, nameof(Repositories.Entities.Partner), request.Request.Relationship.PartnerId); //TODO: error code
        }

        // --------------------------------------------------------------------------------------------------------------------------

        // get codebook ContractRelationshipType item
        // var = await GetContractRelationshipType(request.Request.Relationship.ContractRelationshipTypeId);

        // create request
        var loanLinkRequest = new LoanLinkRequest
        {
            Type = (ContractRelationshipType)request.Request.Relationship.ContractRelationshipTypeId    //TODO: convert codebook ContractRelationshipType item to request enum
        };

        // call endpoint
        var result = await _mpHomeClient.UpdateLoanPartnerLink(request.Request.ProductId, request.Request.Relationship.PartnerId, loanLinkRequest); //TODO: check result

        return new Google.Protobuf.WellKnownTypes.Empty();
    }

    ///// <summary>
    ///// Returns ContractRelationshipType codebook item by ID
    ///// </summary>
    //private async Task<ContractRelationshipTypeItem> GetContractRelationshipType(int contractRelationshipTypeId)
    //{
    //    var list = await _codebookService.ContractRelationshipTypes();
    //    var item = list.FirstOrDefault(t => t.Id == contractRelationshipTypeId);

    //    if (item == null)
    //    {
    //        throw new CisNotFoundException(13014, nameof(ContractRelationshipTypeItem), contractRelationshipTypeId);
    //    }

    //    return item;
    //}

}