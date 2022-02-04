using CIS.Core.Exceptions;
using DomainServices.CodebookService.Abstraction;
using DomainServices.CodebookService.Contracts.Endpoints.ProductInstanceTypes;
using DomainServices.OfferService.Api.Repositories.Entities;
using DomainServices.OfferService.Contracts;

namespace DomainServices.OfferService.Api.Handlers;

internal class BaseHandler
{

    #region Construction

    protected readonly Repositories.OfferInstanceRepository _repository;

    private readonly ICodebookServiceAbstraction _codebookService;

    public BaseHandler(
        Repositories.OfferInstanceRepository repository,
        ICodebookServiceAbstraction codebookService)
    {
        _repository = repository;
        _codebookService = codebookService;
    }

    #endregion


    /// <summary>
    /// Loads OfferInstance entity. 
    /// Throws exception if not found and param throwExceptionIfNotFound = true
    /// </summary>
    protected async Task<OfferInstance> LoadOfferInstance(int offerInstanceId, bool throwExceptionIfNotFound = true)
    {
        var entity = await _repository.Get(offerInstanceId);

        if (entity == null && throwExceptionIfNotFound)
        {
            throw new CisNotFoundException(0, $"OfferInstance #{offerInstanceId} not found");
        }

        return entity;
    }
        
        
    /// <summary>
    /// Checks if ProductInstanceTypeId matches ProductInstanceTypeCategory (Hypo, SS, ...)
    /// </summary>
    protected async Task checkProductInstanceTypeCategory(long id, ProductInstanceTypeCategory category)
    {
        var list = await _codebookService.ProductInstanceTypes();
        var item = list.FirstOrDefault(t => t.Id == id);

        if (item == null)
        {
            throw new CisNotFoundException(13014, nameof(ProductInstanceTypeItem), id);
        }

        if (item.ProductCategory != category)
        {
            throw new CisArgumentException(1, $"ProductInstanceTypeId '{id}' doesn't match ProductInstanceTypeCategory '{category}'.", "ProductInstanceTypeId");
        }
    }


    /// <summary>
    /// Checks if ResourceProcessId exists or generates new one.
    /// </summary>
    protected async Task<Guid> CheckResourceProcessId(string resourceProcessId)
    {
        if (String.IsNullOrWhiteSpace(resourceProcessId))
        {
            return Guid.NewGuid();
        }

        // check if provided ResourceProcessId already exists
        var id = Guid.Parse(resourceProcessId);

        var exists = await _repository.AnyOfResourceProcessId(id);

        if (!exists)
        {
            throw new CisArgumentException(1, $"ResourceProcessId '{resourceProcessId}' not found.", "ResourceProcessId");
        }

        return id;
    }


    /// <summary>
    /// Converts entity created data to contract DTO.
    /// </summary>
    protected OfferInstanceCreated ToCreated(OfferInstance entity)
    {
        return new OfferInstanceCreated
        {
            UserId = entity.CreatedUserId,
            Name = entity.CreatedUserName,
            CreatedOn = entity.CreatedTime
        };
    }

}
