namespace CIS.Core.Configuration
{
    public sealed class KestrelConfiguration
    {
        public List<EndpointInfo>? Endpoints { get; set; }

        public CertificateInfo? Certificate {  get; set; }

        public sealed class EndpointInfo
        {
            public int Port {  get; set; }

            /// <summary>
            /// 1 = HTTP1.1
            /// 2 = HTTP2
            /// </summary>
            public int Protocol {  get; set; }
        }

        public sealed class CertificateInfo
        {
            public string? Path { get; set; }
            public string? Password {  get; set; }
        }
    }
}
