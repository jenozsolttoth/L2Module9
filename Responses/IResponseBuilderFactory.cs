
namespace Responses
{
    public interface IResponseBuilderFactory
    {
        IResponseBuilder<T> GetResponseBuilder<T>();
    }
}
