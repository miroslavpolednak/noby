using CIS.Infrastructure.gRPC;
using DomainServices.CustomerService.Api.Repositories;
using DomainServices.CustomerService.Contracts;
using DomainServices.CustomerService.Dto;
using Grpc.Core;

namespace DomainServices.CustomerService.Api.Handlers
{
    internal class GetCustomerListHandler : IRequestHandler<GetCustomerListMediatrRequest, Contracts.CustomerListResponse>
    {
        private readonly KonsDbRepository _repository;
        private readonly ILogger<GetCustomerListHandler> _logger;
        private readonly CustomerManagement.ICMClient _cm;

        public GetCustomerListHandler(KonsDbRepository repository, ILogger<GetCustomerListHandler> logger, CustomerManagement.ICMClient cm)
        {
            _repository = repository;
            _logger = logger;
            _cm = cm;
        }

        public async Task<CustomerListResponse> Handle(GetCustomerListMediatrRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get list instance Identities #{id}", string.Join(",", request.Request.Identities));

            //var entities = await _repository.GetList(request.Request.Identities.ToList());

            //TODO kontrolovat i pocet na vstupu/vystupu?
            //if (entities == null || !entities.Any())
            //    throw GrpcExceptionHelpers.CreateRpcException(StatusCode.NotFound, $"Detail instance #{string.Join(",", request.Request.Identities)} not found", 10007);

            var response = new CustomerListResponse();

            //foreach (var entity in entities)
            //    response.Customers.Add(entity.ToBasicDataResponse());

            return response;
        }
    }
}
