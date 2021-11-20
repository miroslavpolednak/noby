using CIS.Infrastructure.gRPC;
using DomainServices.CustomerService.Api.Repositories;
using DomainServices.CustomerService.Contracts;
using DomainServices.CustomerService.Dto;
using Grpc.Core;

namespace DomainServices.CustomerService.Api.Handlers
{
    internal class GetDetailHandler : IRequestHandler<GetDetailMediatrRequest, Contracts.GetDetailResponse>
    {
        private readonly KonsDbRepository _repository;
        private readonly ILogger<GetDetailHandler> _logger;

        public GetDetailHandler(KonsDbRepository repository, ILogger<GetDetailHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<GetDetailResponse> Handle(GetDetailMediatrRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get detail instance ID #{id}", request.Request.Identity);

            var entity = await _repository.GetDetail(request.Request.Identity);

            if (entity == null)
                throw GrpcExceptionHelpers.CreateRpcException(StatusCode.NotFound, $"Detail instance #{request.Request.Identity} not found", 10007);

            //TODO: Jak poznam z konsdb jestli jde o FO/PO? Modulo asi nechceme, ne?
            return entity.ToDetailResponse();
        }
    }
}
