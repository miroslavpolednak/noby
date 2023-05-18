using CIS.InternalServices.NotificationService.Messaging.Partials;

namespace cz.kb.osbs.mcs.notificationreport.eventapi.v3.report
{
    public partial class NotificationReport : IMcsResultTopic
    {
    }
}

namespace cz.kb.osbs.mcs.sender.sendapi.v4.email
{
    public partial class SendEmail : IMcsSenderTopic
    {
    }
}

namespace cz.kb.osbs.mcs.sender.sendapi.v4.sms
{
    public partial class SendSMS: IMcsSenderTopic
    {
    }
}

namespace cz.mpss.api.noby.notification.sendapi.v1.email
{
    public partial class SendEmail : IMpssSendEmailTopic
    {
    }
}