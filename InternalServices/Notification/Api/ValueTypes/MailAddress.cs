using System;

namespace CIS.InternalServices.Notification
{
    public sealed class MailAddress : System.Net.Mail.MailAddress
    {
        public MailAddress(string address) : base(address) { }
    }
}
