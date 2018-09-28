using System;
using System.Linq;
using System.Threading.Tasks;
using Module7API.Dal.Model;
using MongoDB.Driver;
using Responses;

namespace Module7API.Dal
{
    public class MongoUserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _mongoUserCollection;
        private readonly IResponseBuilderFactory _responseBuilderFactory;
        public MongoUserRepository(IResponseBuilderFactory responseBuilderFactory)
        {
            _responseBuilderFactory = responseBuilderFactory;
            var mongoClient = new MongoClient("mongodb://localhost:32771");
            var mongoDatabase = mongoClient.GetDatabase("UserStore");
            _mongoUserCollection = mongoDatabase.GetCollection<User>("Users");
        }

        public async Task<GenericResponse<Guid>> Add(User user)
        {
            var responseBuilder = _responseBuilderFactory.GetResponseBuilder<Guid>();
            try
            {
                var userChecker = await _mongoUserCollection.FindAsync(Builders<User>.Filter.Eq("Username", user.Username));
                userChecker.MoveNext();
                var available = !userChecker.Current.Any();
                if ( available )
                {
                    user.UserId = Guid.NewGuid();
                    user.DateOfModification = DateTime.UtcNow;
                    await _mongoUserCollection.InsertOneAsync(user);
                    return responseBuilder.WithEntity(user.UserId).WithMessage("OK").WithStatusCode(StatusCodes.Success).WithSuccess(true)
                        .GetObject();
                }
                return responseBuilder.WithEntity(new Guid()).WithMessage("Username is taken").WithStatusCode(StatusCodes.Taken).WithSuccess(false)
                    .GetObject();
            }
            catch ( Exception e )
            {
                return responseBuilder.WithEntity(new Guid()).WithMessage(e.ToString()).WithStatusCode(StatusCodes.Error).WithSuccess(false)
                    .GetObject();
            }
        }

        public async Task<GenericResponse<bool>> Remove(string username)
        {
            var responseBuilder = _responseBuilderFactory.GetResponseBuilder<bool>();
            try
            {
                await _mongoUserCollection.FindOneAndDeleteAsync(Builders<User>.Filter.Eq("Username", username));
                return responseBuilder.WithEntity(true).WithMessage("OK").WithStatusCode(StatusCodes.Success).WithSuccess(true)
                    .GetObject();
            }
            catch ( Exception e )
            {
                return responseBuilder.WithEntity(false).WithMessage(e.ToString()).WithStatusCode(StatusCodes.Error).WithSuccess(false)
                    .GetObject();
            }

        }

        public async Task<GenericResponse<User>> Get(string username)
        {
            var responseBuilder = _responseBuilderFactory.GetResponseBuilder<User>();
            try
            {
                var userResponse = await _mongoUserCollection.FindAsync(Builders<User>.Filter.Eq("Username", username));
                if ( userResponse.MoveNext() )
                {
                    var user = userResponse.Current.First();
                    return responseBuilder.WithEntity(user).WithStatusCode(StatusCodes.Success).WithMessage("OK").WithSuccess(true)
                        .GetObject();
                }
                return responseBuilder.WithEntity(null).WithMessage("Cannot find user").WithStatusCode(StatusCodes.NotFound).WithSuccess(false)
                    .GetObject();
            }
            catch (Exception e)
            {
                return responseBuilder.WithEntity(null).WithMessage(e.ToString()).WithStatusCode(StatusCodes.Error).WithSuccess(false)
                    .GetObject();
            }
        }
    }
}
