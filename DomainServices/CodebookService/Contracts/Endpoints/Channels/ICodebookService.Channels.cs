using ProtoBuf.Grpc;

namespace DomainServices.CodebookService.Contracts
{
    public partial interface ICodebookService
    {
        [OperationContract]
        Task<List<Endpoints.Channels.ChannelItem>> Channels(Endpoints.Channels.ChannelsRequest request, CallContext context = default);
    }
}