using System;

namespace CIS.Core.Security
{
    public interface ICurrentUserProvider
    {
        ICisUser Get();
    }
}
