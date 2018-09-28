
namespace Responses
{
    public class GenericResponse<T>
    {
        public GenericResponse()
        {
            Success = true;
        }
        public T Entity { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
        public int StatusCode { get; set; }
    }
}
