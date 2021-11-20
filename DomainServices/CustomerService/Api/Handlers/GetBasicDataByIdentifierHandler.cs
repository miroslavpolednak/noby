using CIS.Infrastructure.gRPC;
using DomainServices.CustomerService.Api.Repositories;
using DomainServices.CustomerService.Contracts;
using DomainServices.CustomerService.Dto;
using Grpc.Core;

namespace DomainServices.CustomerService.Api.Handlers
{
    internal class GetBasicDataByIdentifierHandler : IRequestHandler<GetBasicDataByIdentifierMediatrRequest, Contracts.GetBasicDataResponse>
    {
        private readonly KonsDbRepository _repository;
        private readonly ILogger<GetBasicDataByIdentifierHandler> _logger;

        public GetBasicDataByIdentifierHandler(KonsDbRepository repository, ILogger<GetBasicDataByIdentifierHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<GetBasicDataResponse> Handle(GetBasicDataByIdentifierMediatrRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get detail instance Identifier #{id}", request.Request.Identifier);

            var entities = await _repository.GetBasic(request.Request.Identifier);

            if (entities == null || !entities.Any())
                throw GrpcExceptionHelpers.CreateRpcException(StatusCode.NotFound, $"Detail instance #{request.Request.Identifier} not found", 10007);

            //TODO: zjistit spravny exception status a code. Zajima nas vubec zjistovani duplicity?
            if (entities.Count() > 1)
                throw GrpcExceptionHelpers.CreateRpcException(StatusCode.Internal, $"BasicData instance returns duplicate records #{request.Request.Identifier}", 10007);

            return entities.First().ToBasicDataResponse();
        }
    }
}
