using MediatR;
using System.Collections.Generic;

namespace CIS.InternalServices.Notification.Mailing.Dto
{
    public class SaveRequest : IRequest
    {
        public SaveRequest(List<Contracts.Email> emails)
        {
            Emails = emails;
        }

        public List<Contracts.Email> Emails { get; init; }
    }
}
