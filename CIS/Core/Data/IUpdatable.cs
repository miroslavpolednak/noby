using System;

namespace CIS.Core.Data
{
    public interface IUpdatable
    {
        int? UpdateUserId { get; set; }

        DateTime? UpdateTime { get; set; }
    }

    public class BaseUpdatableOnly : IUpdatable
    {
        public int? UpdateUserId { get; set; }

        public DateTime? UpdateTime { get; set; }
    }

    public class BaseUpdatable : BaseInsertable, IUpdatable
    {
        public int? UpdateUserId { get; set; }

        public DateTime? UpdateTime { get; set; }
    }

    public class BaseUpdatableWithoutIsActual : IUpdatable, IInsertable
    {
        public int? InsertUserId { get; set; }

        public DateTime? InsertTime { get; set; }

        public int? UpdateUserId { get; set; }

        public DateTime? UpdateTime { get; set; }
    }
}
