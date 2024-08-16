﻿using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace DomainServices.CustomerService.Api.Endpoints.v1;

[Authorize]
internal sealed class CustomerService(IMediator _mediator)
    : Contracts.V1.CustomerService.CustomerServiceBase
{
    public override Task<ProfileCheckResponse> ProfileCheck(ProfileCheckRequest request, ServerCallContext context)
        => _mediator.Send(request, context.CancellationToken);

    public override Task<CreateCustomerResponse> CreateCustomer(CreateCustomerRequest request, ServerCallContext context)
        => _mediator.Send(request, context.CancellationToken);

    public override Task<UpdateCustomerResponse> UpdateCustomer(UpdateCustomerRequest request, ServerCallContext context)
        => _mediator.Send(request, context.CancellationToken);

    public override async Task<Empty> UpdateCustomerIdentifiers(UpdateCustomerIdentifiersRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override Task<CustomerDetailResponse> GetCustomerDetail(CustomerDetailRequest request, ServerCallContext context)
        => _mediator.Send(request, context.CancellationToken);

    public override async Task<CustomerListResponse> GetCustomerList(CustomerListRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<SearchCustomersResponse> SearchCustomers(SearchCustomersRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<ValidateContactResponse> ValidateContact(ValidateContactRequest request, ServerCallContext context)
        => await _mediator.Send(request, context.CancellationToken);

    public override async Task<FormatAddressResponse> FormatAddress(FormatAddressRequest request, ServerCallContext context) =>
        await _mediator.Send(request, context.CancellationToken);

    public override async Task<Empty> UpdateContacts(UpdateContactsRequest request, ServerCallContext context) => 
        await _mediator.Send(request, context.CancellationToken);
}
