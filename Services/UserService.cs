using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Module7API.Dal;
using Module7API.Services.Models;
using Responses;

namespace Module7API.Services
{
    public class UserService : IUserService
    {
        //private readonly IUserRepository _userRepository;
        private readonly IResponseBuilderFactory _responseBuilderFactory;

        //public UserService(IUserRepository userRepository, IResponseBuilderFactory responseBuilderFactory)
        //{
        //    _userRepository = userRepository;
        //    _responseBuilderFactory = responseBuilderFactory;
        //}

        //public async Task<GenericResponse<Guid>> Add(User user)
        //{
        //    user.RegistrationDate = DateTime.UtcNow;
        //    var response = await _userRepository.Add(AutoMapper.Mapper.Map<Dal.Model.User>(user));
        //    return response;
        //}

        //public async Task<GenericResponse<User>> Get(string userName)
        //{
        //    var responseBuilder = _responseBuilderFactory.GetResponseBuilder<User>();

        //    var response = await _userRepository.Get(userName);
        //    if (response.Success)
        //    {
        //        return responseBuilder
        //            .WithEntity(Mapper.Map<User>(response.Entity))
        //            .WithMessage("OK")
        //            .WithStatusCode(StatusCodes.Success)
        //            .WithSuccess(true)
        //            .GetObject(); 
        //    }
        //    return responseBuilder
        //        .WithEntity(null)
        //        .WithMessage("User not exist.")
        //        .WithStatusCode(StatusCodes.NotFound)
        //        .WithSuccess(false)
        //        .GetObject();
        //}

        //public Task<GenericResponse<bool>> Remove(long userId)
        //{
        //    throw new NotImplementedException();
        //}
        private readonly IRepository<Module7API.Dal.Model.User> _userRepository;

        public UserService(IResponseBuilderFactory responseBuilderFactory, IRepository<Dal.Model.User> userRepository)
        {
            _responseBuilderFactory = responseBuilderFactory;
            _userRepository = userRepository;
        }
        public async Task<GenericResponse<Guid>> Add(User user)
        {
            var responseBuilder = _responseBuilderFactory.GetResponseBuilder<Guid>();

            if (!(await _userRepository.Get(x => x.Username == user.Username)).Entity.Any())
            {
                return await _userRepository.Add(AutoMapper.Mapper.Map<Dal.Model.User>(user));
            }

            return responseBuilder.WithEntity(new Guid()).WithMessage("Username is taken").WithStatusCode(StatusCodes.Taken).WithSuccess(false)
                .GetObject();
        }

        public async Task<GenericResponse<User>> Get(string userName)
        {
            try
            {
                var result = _userRepository.Get(x => x.Username == userName);
                var user = (await result).Entity.SingleOrDefault();
                return _responseBuilderFactory.GetResponseBuilder<User>().WithEntity(Mapper.Map<User>(user))
                    .WithMessage("OK").WithSuccess(true).WithStatusCode(StatusCodes.Success).GetObject();
            }
            catch (Exception e)
            {
                return _responseBuilderFactory.GetResponseBuilder<User>().WithEntity(null)
                    .WithMessage(e.ToString()).WithSuccess(false).WithStatusCode(StatusCodes.Error).GetObject();
            }  
        }

        public Task<GenericResponse<bool>> Remove(long userId)
        {
            throw new NotImplementedException();
        }
    }
}
