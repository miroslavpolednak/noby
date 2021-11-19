using Microsoft.Extensions.Logging;
using Dapper;
using System.Threading.Tasks;
using CIS.Core;
using System.Collections.Generic;
using CIS.Infrastructure.Data;
using System.Linq;
using CIS.Core.Data;

namespace CIS.InternalServices.Notification.Mailing.Repositories
{
    public class MailingRepository : DapperBaseRepository<MailingRepository>
    {
        private readonly IDateTime _time;

        public MailingRepository(ILogger<MailingRepository> logger, IConnectionProvider factory, IDateTime time)
            : base(logger, factory)
        {
            this._time = time;
        }
    }
}
