
using System.Threading.Tasks;
using Module7API.Security.Model;
using Module7API.Services;
using Responses;

namespace Module7API.Security.Services
{
    public class LoginService : ILoginService
    {
        private readonly IUserService _userService;
        private readonly ITokenBuilderService _tokenBuilderService;

        public LoginService(IUserService userService, ITokenBuilderService tokenBuilderService)
        {
            _userService = userService;
            _tokenBuilderService = tokenBuilderService;
        }

        public async Task<GenericResponse<string>> Login(UserLoginModel loginDetails)
        {
            var result = await _userService.Get(loginDetails.Username);
            if (result.Success)
            {
                if (result.Entity.Password == loginDetails.Password)
                {
                    return _tokenBuilderService.BuildToken(result.Entity);
                }
            }
            return new GenericResponse<string>(){Entity = null, Message = "Failed to login", StatusCode = -1, Success = false};
        }
    }
}
