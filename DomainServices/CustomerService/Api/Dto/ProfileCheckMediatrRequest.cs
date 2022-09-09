using CIS.Core.Validation;

namespace DomainServices.CustomerService.Api.Dto;

internal record ProfileCheckMediatrRequest(ProfileCheckRequest Request) 
    : IRequest<ProfileCheckResponse>, IValidatableRequest;