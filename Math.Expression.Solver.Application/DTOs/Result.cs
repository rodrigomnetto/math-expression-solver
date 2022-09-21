
namespace Math.Expression.Solver.Application.DTOs
{
    public class Result
    {
        public bool Succeeded { get; }

        public string Message { get; }

        public Result(bool succeeded, string message)
        {
            Succeeded = succeeded;
            Message = message;
        }

        public T As<T>() where T : Result => (T)this;
    }
}
