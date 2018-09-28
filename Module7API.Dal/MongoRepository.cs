using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Module7API.Dal.Helpers;
using Module7API.Dal.Model;
using MongoDB.Driver;
using Responses;

namespace Module7API.Dal
{
    public class MongoRepository<TEntity> : IRepository<TEntity> where TEntity : IEntity
    {
        private readonly IMongoCollection<TEntity> _mongoCollection;
        private readonly IResponseBuilderFactory _responseBuilderFactory;
        private readonly IDateTimeWrapper _dateTimeWrapper;
        public MongoRepository(IResponseBuilderFactory responseBuilderFactory, IDateTimeWrapper dateTimeWrapper)
        {
            _responseBuilderFactory = responseBuilderFactory;
            _dateTimeWrapper = dateTimeWrapper;

            var mongoClient = new MongoClient("mongodb://localhost:32771");
            var mongoDatabase = mongoClient.GetDatabase(typeof(TEntity).Name + "Store");

            _mongoCollection = mongoDatabase.GetCollection<TEntity>(typeof(TEntity).Name + "Collection");
        }

        public async Task<GenericResponse<Guid>> Add(TEntity entity)
        {
            var builder = _responseBuilderFactory.GetResponseBuilder<Guid>();

            try
            {
                entity.Id = Guid.NewGuid();
                entity.CreateDate = _dateTimeWrapper.Now();

                await _mongoCollection.InsertOneAsync(entity);

                return builder.WithEntity(entity.Id).WithStatusCode(StatusCodes.Success).WithMessage("OK").WithSuccess(true)
                    .GetObject();
            }
            catch (Exception e)
            {
                return builder.WithEntity(new Guid()).WithStatusCode(StatusCodes.Error).WithMessage(e.ToString()).WithSuccess(false)
                    .GetObject();
            }
        }

        public async Task<GenericResponse<bool>> Remove(Expression<Func<TEntity, bool>> query)
        {
            var builder = _responseBuilderFactory.GetResponseBuilder<bool>();
            try
            {
                await _mongoCollection.DeleteManyAsync(query);

                return builder.WithEntity(true).WithMessage("OK").WithStatusCode(StatusCodes.Success).WithSuccess(true)
                    .GetObject();
            }
            catch (Exception e)
            {
                return builder.WithEntity(false).WithMessage(e.ToString()).WithStatusCode(StatusCodes.Error)
                    .WithSuccess(false).GetObject();
            }
        }

        public async Task<GenericResponse<IEnumerable<TEntity>>> Get(Expression<Func<TEntity,bool>> query)
        {
            var builder = _responseBuilderFactory.GetResponseBuilder<IEnumerable<TEntity>>();

            try
            {
                var result = await _mongoCollection.FindAsync(query);

                return builder.WithEntity(result.ToList()).WithStatusCode(StatusCodes.Success).WithMessage("OK").WithSuccess(true)
                    .GetObject();
            }
            catch (Exception e)
            {
                var result = await _mongoCollection.FindAsync(query);

                return builder.WithEntity(null).WithStatusCode(StatusCodes.Error).WithMessage(e.ToString()).WithSuccess(false)
                    .GetObject();
            }
        }

        public async Task<GenericResponse<bool>> Update(TEntity entity)
        {
            var builder = _responseBuilderFactory.GetResponseBuilder<bool>();
            try
            {
                var toupdate = (await Get(x => x.Id == entity.Id)).Entity.SingleOrDefault();
                if ( toupdate != null )
                {
                    foreach ( var propertyInfo in toupdate.GetType().GetProperties().Where(x=>Attribute.IsDefined(x,typeof(ModifiableAttribute))) )
                    {
                        var propertyValueToUpdate = propertyInfo.GetValue(toupdate).ToString();
                        var propertyValueToUpdateWith = propertyInfo.GetValue(entity).ToString();
                        if ( propertyValueToUpdate != propertyValueToUpdateWith )
                        {
                            propertyInfo.SetValue(toupdate,
                                entity.GetType().GetProperty(propertyInfo.Name).GetValue(entity, null));
                        }
                    }
                }
                _mongoCollection.ReplaceOne(x => x.Id == toupdate.Id, toupdate);
                return builder.WithStatusCode(StatusCodes.Success).WithEntity(true).WithMessage("OK").WithSuccess(true)
                    .GetObject();
            }
            catch (Exception e)
            {
                return builder.WithStatusCode(StatusCodes.Error).WithEntity(false).WithMessage(e.ToString()).WithSuccess(false)
                    .GetObject();
            }
        }
    }
}
