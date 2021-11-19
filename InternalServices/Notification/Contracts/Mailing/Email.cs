using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CIS.InternalServices.Notification.Contracts
{
    [DataContract]
    public class Email
    {
        [DataMember(Order = 1)]
        public string FromAddress { get; set; }

        [DataMember(Order = 2)]
        public List<string> Recipients { get; set; }

        [DataMember(Order = 3)]
        public List<string> Cc { get; set; }

        [DataMember(Order = 4)]
        public List<string> Bcc { get; set; }

        [DataMember(Order = 5)]
        public string Subject { get; set; }

        [DataMember(Order = 6)]
        public string HtmlBody { get; set; }

        [DataMember(Order = 10)]
        public List<byte[]> Attachments { get; set; }
    }
}
