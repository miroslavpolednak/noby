using System;

namespace CIS.Core
{
    public interface IDateTime
    {
        /// <summary>
        /// Current time
        /// </summary>
        DateTime Now { get; }

        /// <summary>
        /// Current time
        /// </summary>
        DateTimeOffset OffsetNow { get; }
    }
}