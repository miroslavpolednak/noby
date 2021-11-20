using CIS.Infrastructure.gRPC;
using DomainServices.CustomerService.Api.Repositories;
using DomainServices.CustomerService.Contracts;
using DomainServices.CustomerService.Dto;
using Grpc.Core;
using System.Text.Json;

namespace DomainServices.CustomerService.Api.Handlers
{
    internal class GetBasicDataByFullIdentificationHandler : IRequestHandler<GetBasicDataByFullIdentificationMediatrRequest, Contracts.GetBasicDataResponse>
    {
        private readonly KonsDbRepository _repository;
        private readonly ILogger<GetBasicDataByFullIdentificationHandler> _logger;

        public GetBasicDataByFullIdentificationHandler(KonsDbRepository repository, ILogger<GetBasicDataByFullIdentificationHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<GetBasicDataResponse> Handle(GetBasicDataByFullIdentificationMediatrRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get detail instance Identifier #{id}", JsonSerializer.Serialize(request));

            var entities = await _repository.GetBasic(request.Request);

            if (entities == null || !entities.Any())
                throw GrpcExceptionHelpers.CreateRpcException(StatusCode.NotFound, $"Detail instance #{JsonSerializer.Serialize(request)} not found", 10007);

            //TODO: zjistit spravny exception status a code. Zajima nas vubec zjistovani duplicity?
            if (entities.Count() > 1)
                throw GrpcExceptionHelpers.CreateRpcException(StatusCode.Internal, $"BasicData instance returns duplicate records #{JsonSerializer.Serialize(request)}", 10007);

            return entities.First().ToBasicDataResponse();
        }
    }
}
