using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Module7API.Dal.Model;
using Responses;

namespace Module7API.Dal
{
    public class UserRepository : IUserRepository
    {
        private static readonly List<User> UserDatabase = new List<User>();
        public static object Lockobject = new object();
        private readonly IResponseBuilderFactory _responseBuilderFactory;


        public UserRepository(IResponseBuilderFactory responseBuilderFactory)
        {
            _responseBuilderFactory = responseBuilderFactory;
        }

        public async Task<GenericResponse<Guid>> Add(User user)
        {
            var responseBuilder =_responseBuilderFactory.GetResponseBuilder<Guid>();
            lock (Lockobject)
            {
                if (UserDatabase.All(x => x.Username != user.Username))
                {
                    user.UserId = Guid.NewGuid();
                    user.DateOfModification = DateTime.UtcNow;
                    UserDatabase.Add(user);

                    responseBuilder.WithEntity(user.UserId)
                        .WithMessage("OK")
                        .WithStatusCode(StatusCodes.Success);
                }
                else
                {
                    responseBuilder.WithEntity(new Guid())
                        .WithSuccess(false)
                        .WithStatusCode(StatusCodes.Taken)
                        .WithMessage("Username is taken");
                }
            }
            return responseBuilder.GetObject();
        }

        public async Task<GenericResponse<bool>> Remove(string username)
        {
            var responseBuilder = _responseBuilderFactory.GetResponseBuilder<bool>();
            lock (Lockobject)
            {
                UserDatabase.Remove(UserDatabase.SingleOrDefault(x => x.Username == username));
            }

            return responseBuilder.WithEntity(true)
                .WithMessage("OK")
                .WithStatusCode(StatusCodes.Success)
                .WithSuccess(true).GetObject();
        }

        public async Task<GenericResponse<User>> Get(string username)
        {
            var responseBuilder = _responseBuilderFactory.GetResponseBuilder<User>();
            lock (Lockobject)
            {
                var user = UserDatabase.SingleOrDefault(x => x.Username == username);
                if (user != null)
                {
                    return responseBuilder.WithEntity(user)
                        .WithStatusCode(StatusCodes.Success)
                        .WithMessage("OK")
                        .GetObject();
                }
            }

            return responseBuilder.WithEntity(null)
                .WithMessage("Cannot find user")
                .WithStatusCode(StatusCodes.NotFound)
                .WithSuccess(false)
                .GetObject();
        }
    }
}
