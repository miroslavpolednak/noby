using System.ComponentModel;
using DomainServices.CustomerService.Api.Services;
using DomainServices.CustomerService.Api.Services.CustomerManagement;
using ExternalServices.MpHome.V1;

namespace DomainServices.CustomerService.Api.Endpoints.v1.UpdateCustomer;

internal sealed class UpdateCustomerHandler(
    IdentifiedSubjectService _identifiedSubjectService,
    MpHomeUpdateMapper _mpHomeMapper,
    IMpHomeClient _mpHomeClient)
        : IRequestHandler<UpdateCustomerRequest, UpdateCustomerResponse>
{
    public async Task<UpdateCustomerResponse> Handle(UpdateCustomerRequest request, CancellationToken cancellationToken)
    {
        if (request.Mandant == SharedTypes.GrpcTypes.Mandants.Kb)
        {
            await _identifiedSubjectService.UpdateSubject(request, cancellationToken);
        }
        else if (request.Mandant == SharedTypes.GrpcTypes.Mandants.Mp)
        {
            var mpIdentity = request.Identities.First(i => i.IdentityScheme == SharedTypes.GrpcTypes.Identity.Types.IdentitySchemes.Mp);

            if (!await _mpHomeClient.PartnerExists(mpIdentity.IdentityId, cancellationToken))
            {
                throw CIS.Core.ErrorCodes.ErrorCodeMapperBase.CreateNotFoundException(ErrorCodeMapper.PartnerNotInKonsDb, mpIdentity.IdentityId);
            }

            var partnerRequest = await _mpHomeMapper.MapUpdateRequest(request.NaturalPerson, request.Identities, request.IdentificationDocument, request.Addresses, request.Contacts, cancellationToken);

            await _mpHomeClient.UpdatePartner(mpIdentity.IdentityId, partnerRequest, cancellationToken);
        }
        else
        {
            throw new InvalidEnumArgumentException();
        }

        return new UpdateCustomerResponse();
    }
}