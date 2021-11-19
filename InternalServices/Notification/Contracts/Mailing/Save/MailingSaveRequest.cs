using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CIS.InternalServices.Notification.Contracts
{
    [DataContract]
    public class MailingSaveRequest
    {
        [DataMember(Order = 1)]
        public List<Email> Emails { get; set; }
    }
}
