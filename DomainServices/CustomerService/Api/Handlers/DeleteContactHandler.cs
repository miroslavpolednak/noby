using Grpc.Core;
using CIS.Infrastructure.gRPC;
using Google.Protobuf.WellKnownTypes;
using DomainServices.CustomerService.Dto;

namespace DomainServices.CustomerService.Api.Handlers
{
    internal class DeleteContactHandler : IRequestHandler<DeleteContactMediatrRequest, Empty>
    {
        private readonly ILogger<DeleteContactHandler> _logger;
        private readonly ExternalServices.MpHome.IMpHomeClient _mpHome;

        public DeleteContactHandler(ILogger<DeleteContactHandler> logger, ExternalServices.MpHome.IMpHomeClient mpHome)
        {
            _logger = logger;
            _mpHome = mpHome;
        }

        public async Task<Empty> Handle(DeleteContactMediatrRequest request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Run DeleteContact with {inputs}", request);

            await _mpHome.DeleteContact(request.Request.ContactId, request.Request.Identity);

            return new Empty();
        }
    }
}
