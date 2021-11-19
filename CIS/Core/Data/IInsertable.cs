using System;

namespace CIS.Core.Data
{
    public interface IInsertable
    {
        int? InsertUserId { get; set; }

        DateTime? InsertTime { get; set; }
    }

    public class BaseInsertable : BaseIsActual, IInsertable
    {
        public int? InsertUserId { get; set; }

        public DateTime? InsertTime { get; set; }
    }

    public class BaseInsertableWithoutActual : IInsertable
    {
        public int? InsertUserId { get; set; }

        public DateTime? InsertTime { get; set; }
    }
}
