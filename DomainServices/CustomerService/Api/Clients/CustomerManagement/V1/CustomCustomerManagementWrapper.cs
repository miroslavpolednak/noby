using System.Text;

namespace DomainServices.CustomerService.Api.Clients.CustomerManagement.V1;

internal partial class CustomerManagementWrapper
{
    partial void PrepareRequest(HttpClient client, HttpRequestMessage request, StringBuilder urlBuilder)
    {
        var uri = client.BaseAddress!;

        client.BaseAddress = null;

        urlBuilder.Insert(0, uri.ToString());
    }
}