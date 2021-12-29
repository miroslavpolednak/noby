using System;

namespace CIS.Core.Security
{
    public interface ICurrentUser
    {
        int Id { get; }

        string Name { get; }

        string Login { get; }
    }
}
