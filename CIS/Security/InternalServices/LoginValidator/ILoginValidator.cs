using System.Threading.Tasks;

namespace CIS.Security.InternalServices
{
    public interface ILoginValidator
    {
        Task<bool> Validate(string login, string password);
    }
}
