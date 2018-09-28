using Module7API.Services.Models;
using Responses;

namespace Module7API.Security.Services
{
    public interface ITokenBuilderService
    {
        GenericResponse<string> BuildToken(User user);
    }
}
