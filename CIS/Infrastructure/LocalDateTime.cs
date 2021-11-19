using System;
using CIS.Core;

namespace CIS.Infrastructure
{
    /// <summary>
    /// Vrací aktuální lokální čas
    /// </summary>
    public class LocalDateTime : IDateTime
    {
        public DateTime Now => DateTime.Now;
        public DateTimeOffset OffsetNow => DateTimeOffset.Now;
    }
}