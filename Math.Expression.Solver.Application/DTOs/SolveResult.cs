
namespace Math.Expression.Solver.Application.DTOs
{
    public class SolveResult : Result
    {
        public double Result { get; }

        public SolveResult(bool succeeded, string message, double result) : base(succeeded, message)
        {
            Result = result;
        }
    }
}
