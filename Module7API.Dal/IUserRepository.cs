
using System;
using System.Threading.Tasks;
using Module7API.Dal.Model;
using Responses;

namespace Module7API.Dal
{
    public interface IUserRepository
    {
        Task<GenericResponse<Guid>> Add(User user);
        Task<GenericResponse<bool>> Remove(string username);
        Task<GenericResponse<User>> Get(string username);
    }
}
