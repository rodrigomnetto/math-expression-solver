
namespace Math.Expression.Solver.Application.DTOs
{
    public class StepsResult : Result
    {
        public string Steps { get; }

        public double Result { get; }

        public StepsResult(bool succeeded, string message, string steps, double result) : base(succeeded, message)
        {
            Steps = steps;
            Result = result;
        }
    }
}
