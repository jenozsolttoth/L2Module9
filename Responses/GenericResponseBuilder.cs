
namespace Responses
{
    public class GenericResponseBuilder<T> : IResponseBuilder<T>
    {
        private GenericResponse<T> _response = new GenericResponse<T>();
        public IResponseBuilder<T> WithStatusCode(int statusCode)
        {
            _response.StatusCode = statusCode;
            return this;
        }

        public IResponseBuilder<T> WithMessage(string message)
        {
            _response.Message = message;
            return this;
        }

        public IResponseBuilder<T> WithEntity(T entity)
        {
            _response.Entity = entity;
            return this;
        }

        public IResponseBuilder<T> WithSuccess(bool success)
        {
            _response.Success = success;
            return this;
        }

        public IResponseBuilder<T> Clear()
        {
            _response = new GenericResponse<T>();
            return this;
        }

        public GenericResponse<T> GetObject()
        {
            return _response;
        }
    }
}
