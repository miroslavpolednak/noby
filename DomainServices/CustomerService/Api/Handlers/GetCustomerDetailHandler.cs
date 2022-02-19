using CIS.Infrastructure.gRPC;
using DomainServices.CustomerService.Api.Repositories;
using DomainServices.CustomerService.Contracts;
using DomainServices.CustomerService.Dto;
using Grpc.Core;

namespace DomainServices.CustomerService.Api.Handlers
{
    internal class GetCustomerDetailHandler : IRequestHandler<GetCustomerDetailMediatrRequest, Contracts.CustomerResponse>
    {
        private readonly KonsDbRepository _repository;
        private readonly ILogger<GetCustomerDetailHandler> _logger;

        public GetCustomerDetailHandler(KonsDbRepository repository, ILogger<GetCustomerDetailHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<CustomerResponse> Handle(GetCustomerDetailMediatrRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get detail instance ID #{id}", request.Request);

            var entity = await _repository.GetDetail(0);

            if (entity == null)
                throw GrpcExceptionHelpers.CreateRpcException(StatusCode.NotFound, $"Detail instance #{request.Request} not found", 10007);

            //TODO: Jak poznam z konsdb jestli jde o FO/PO? Modulo asi nechceme, ne?
            return entity.ToDetailResponse();
        }
    }
}
