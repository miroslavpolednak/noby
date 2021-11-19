using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CIS.InternalServices.Notification.Contracts
{
    [DataContract]
    public class MailingSaveStreamRequest
    {
        [DataMember(Order = 1)]
        public IAsyncEnumerable<Email> Emails { get; set; }
    }
}
