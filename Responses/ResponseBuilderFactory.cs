
namespace Responses
{
    public class ResponseBuilderFactory : IResponseBuilderFactory
    {
        public IResponseBuilder<T> GetResponseBuilder<T>()
        {
            return new GenericResponseBuilder<T>();
        }
    }
}
