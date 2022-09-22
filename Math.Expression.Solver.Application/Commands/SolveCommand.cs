using Math.Expression.Solver.Application.DTOs;
using Math.Expression.Solver.Application.Parser;

namespace Math.Expression.Solver.Application.Commands
{
    public interface ISolveCommandHandler
    {
        SolveResult Handle(SolveCommand command);
    }

    public class SolveCommand
    {
        string Expression { get; }

        public SolveCommand(string expression)
        {
            Expression = expression;
        }

        public class SolveCommandHandler : ISolveCommandHandler
        {
            public SolveResult Handle(SolveCommand command)
            {
                var succeeded = true;
                var message = string.Empty;
                double result = 0;

                try
                {
                    var parser = new ExpressionParser();
                    result = parser.Parse(command.Expression);
                }
                catch
                {
                    succeeded = false;
                    message = "Math expression contains incorrect syntax.";
                }

                return new SolveResult(succeeded, message, result);
            }
        }
    }
}
