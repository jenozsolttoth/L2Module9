
namespace Responses
{
    public interface IResponseBuilder<T>
    {
        IResponseBuilder<T> WithStatusCode(int statusCode);
        IResponseBuilder<T> WithMessage(string message);
        IResponseBuilder<T> WithEntity(T entity);
        IResponseBuilder<T> WithSuccess(bool success);
        IResponseBuilder<T> Clear();
        GenericResponse<T> GetObject();
    }
}
