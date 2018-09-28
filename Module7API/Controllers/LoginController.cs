using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Module7API.Models;
using Module7API.Security.Model;
using Module7API.Security.Services;
using Module7API.Services;
using Module7API.Services.Models;
using Newtonsoft.Json;
using Responses;

namespace Module7API.Controllers
{
    [Produces("application/json")]
    [Route("api/Login")]
    [EnableCors("MyPolicy")]
    public class LoginController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILoginService _loginService;
        private readonly IResponseBuilderFactory _responseBuilderFactory;
        public LoginController(IUserService userService, ILoginService loginService, IResponseBuilderFactory responseBuilderFactory)
        {
            _userService = userService;
            _loginService = loginService;
            _responseBuilderFactory = responseBuilderFactory;
        }

        // GET: api/Login/5
        [HttpGet("{loginDetails}"), AllowAnonymous]
        public async Task<GenericResponse<string>> Get(string loginDetails)
        {
            var loginData = JsonConvert.DeserializeObject<UserLoginViewModel>(loginDetails);
            var loginDetail = AutoMapper.Mapper.Map<UserLoginModel>(loginData);
            return  await _loginService.Login(loginDetail);
        }
        
        // POST: api/Login
        [HttpPost, AllowAnonymous]
        public async Task<GenericResponse<Guid>> Post([FromBody]UserCreateViewModel value)
        {
            var responseBuilder = _responseBuilderFactory.GetResponseBuilder<Guid>();
            if (ModelState.IsValid)
            {
                var response = await _userService.Add(AutoMapper.Mapper.Map<User>(value));
                return response;
            }

            return responseBuilder.WithEntity(new Guid())
                .WithMessage("OK")
                .WithStatusCode(StatusCodes.InvalidModel)
                .WithSuccess(false)
                .GetObject();
        }   
        
        // PUT: api/Login/5
        [HttpPut("{id}")]
        public GenericResponse<bool> Put(Guid id, [FromBody]UserCreateViewModel value)
        {
            throw new NotImplementedException();
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public GenericResponse<bool> Delete(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
