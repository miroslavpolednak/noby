using DomainServices.CustomerService.Contracts;
using DomainServices.CustomerService.Dto;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;

namespace DomainServices.CustomerService.Api.Services
{
    [Authorize]
    public class CustomerService : Contracts.CustomerService.CustomerServiceBase
    {
        private readonly IMediator _mediator;

        public CustomerService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task<CreateResponse> Create(CreateRequest request, ServerCallContext context)
            => await _mediator.Send(new CreateMediatrRequest(request));

        public override async Task<CreateContactResponse> CreateContact(CreateContactRequest request, ServerCallContext context)
            => await _mediator.Send(new CreateContactMediatrRequest(request));

        public override async Task<Empty> DeleteContact(DeleteContactRequest request, ServerCallContext context)
            => await _mediator.Send(new DeleteContactMediatrRequest(request));

        public override async Task<GetBasicDataResponse> GetBasicDataByFullIdentification(GetBasicDataByFullIdentificationRequest request, ServerCallContext context)
            => await _mediator.Send(new GetBasicDataByFullIdentificationMediatrRequest(request));

        public override async Task<GetBasicDataResponse> GetBasicDataByIdentifier(GetBasicDataByIdentifierRequest request, ServerCallContext context)
            => await _mediator.Send(new GetBasicDataByIdentifierMediatrRequest(request));

        public override async Task<GetDetailResponse> GetDetail(GetDetailRequest request, ServerCallContext context)
            => await _mediator.Send(new GetDetailMediatrRequest(request));

        public override async Task<GetListResponse> GetList(GetListRequest request, ServerCallContext context)
            => await _mediator.Send(new GetListMediatrRequest(request));

        public override async Task<Empty> UpdateAdress(UpdateAdressRequest request, ServerCallContext context)
            => await _mediator.Send(new UpdateAdressMediatrRequest(request));

        public override async Task<Empty> UpdateBasicData(UpdateBasicDataRequest request, ServerCallContext context)
            => await _mediator.Send(new UpdateBasicDataMediatrRequest(request));

        public override Task<Empty> Test(TestRequest request, ServerCallContext context)
        {
            //var x = request.a
            return base.Test(request, context);
        }

    }
}
