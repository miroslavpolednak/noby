namespace Mpss.Rip.Infrastructure.RemoteServices.IC4M.Models.RiskBusinessCase
{
    public class ResourceIdentifier
    {
        /// <summary>
        /// The resource instance code, eg. 'KBCZ'
        /// </summary>
        /// <value>The resource instance code, eg. 'KBCZ'</value>
        public string Instance { get; set; }

        /// <summary>
        /// The resource domain code, eg. 'CFLM'
        /// </summary>
        /// <value>The resource domain code, eg. 'CFLM'</value>
        public string Domain { get; set; }

        /// <summary>
        /// The resource code (the in-domain resource code, eg. 'LimitModel')
        /// </summary>
        /// <value>The resource code (the in-domain resource code, eg. 'LimitModel')</value>
        public string Resource { get; set; }

        /// <summary>
        /// ID
        /// </summary>
        /// <value>ID</value>
        public string Id { get; set; }

        /// <summary>
        /// The variant of the resource, eg. distinguishing the origin of the source
        /// </summary>
        /// <value>The variant of the resource, eg. distinguishing the origin of the source</value>
        public string Variant { get; set; }
    }
}
