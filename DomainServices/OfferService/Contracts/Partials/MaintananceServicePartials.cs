using Google.Protobuf.WellKnownTypes;

namespace DomainServices.OfferService.Contracts;
public partial class ImportOfferFromDatamartRequest : MediatR.IRequest<Empty>, CIS.Core.Validation.IValidatableRequest { }

public partial class DeleteRefixationOfferOlderThanRequest : MediatR.IRequest<Empty>, CIS.Core.Validation.IValidatableRequest { }
