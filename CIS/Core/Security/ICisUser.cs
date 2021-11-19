using System;

namespace CIS.Core.Security
{
    public interface ICisUser
    {
        int Id { get; }

        string Name { get; }
    }
}
