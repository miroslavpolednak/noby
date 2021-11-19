using System.ServiceModel;
using System.Threading.Tasks;
using ProtoBuf.Grpc;

namespace CIS.InternalServices.Notification.Contracts
{
    [ServiceContract(Name = "CIS.InternalServices.Notification.Mailing")]
    public interface IMailingService
    {
        [OperationContract]
        Task Save(MailingSaveRequest request, CallContext context = default);

        [OperationContract]
        Task SaveStream(MailingSaveStreamRequest request, CallContext context = default);
    }
}
