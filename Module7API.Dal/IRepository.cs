using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Responses;

namespace Module7API.Dal
{
    public interface IRepository<TEntity> where TEntity : IEntity
    {
        Task<GenericResponse<Guid>> Add(TEntity entity);
        Task<GenericResponse<bool>> Remove(Expression<Func<TEntity, bool>> query);
        Task<GenericResponse<IEnumerable<TEntity>>> Get(Expression<Func<TEntity, bool>> query);
        Task<GenericResponse<bool>> Update(TEntity entity);
    }
}
