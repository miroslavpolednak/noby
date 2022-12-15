using System.ComponentModel;
using System.Runtime.Intrinsics.Arm;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using System.Xml.Linq;

namespace DomainServices.CodebookService.Contracts.Endpoints.NetMonthEarnings
{
    [DataContract]
    public class NetMonthEarningItem
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }


        [DataMember(Order = 2)]
        public string Name { get; set; }


        [DataMember(Order = 3)]
        public string RdmCode { get; set; }
    }
}
