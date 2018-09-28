
using System.Threading.Tasks;
using Module7API.Security.Model;
using Responses;

namespace Module7API.Security.Services
{
    public interface ILoginService
    {
        Task<GenericResponse<string>> Login(UserLoginModel loginDetails);
    }
}
