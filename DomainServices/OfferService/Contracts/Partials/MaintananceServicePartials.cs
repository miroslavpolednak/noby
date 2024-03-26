using Google.Protobuf.WellKnownTypes;

namespace DomainServices.MaintananceService.Contracts;
public partial class ImportOfferFromDatamartRequest : MediatR.IRequest<Empty>, CIS.Core.Validation.IValidatableRequest { }
