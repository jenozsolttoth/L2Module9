using System;
using System.Threading.Tasks;
using Module7API.Services.Models;
using Responses;

namespace Module7API.Services
{
    public interface IUserService
    {
        Task<GenericResponse<Guid>> Add(User user);
        Task<GenericResponse<User>> Get(string userName);
        Task<GenericResponse<bool>> Remove(Int64 userId);
    }
}
