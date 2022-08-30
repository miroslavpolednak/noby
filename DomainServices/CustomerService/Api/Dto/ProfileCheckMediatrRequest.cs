using CIS.Core.Validation;
using DomainServices.CustomerService.Contracts;

namespace DomainServices.CustomerService.Api.Dto;

internal record ProfileCheckMediatrRequest(ProfileCheckRequest Request) 
    : IRequest<ProfileCheckResponse>, IValidatableRequest;