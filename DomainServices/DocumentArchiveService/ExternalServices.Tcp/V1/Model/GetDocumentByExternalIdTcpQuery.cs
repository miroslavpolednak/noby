using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainServices.DocumentArchiveService.ExternalServices.Tcp.V1.Model;
public class GetDocumentByExternalIdTcpQuery
{
    public string DocumentId { get; set; } = null!;

    public bool WithContent { get; set; }
}
