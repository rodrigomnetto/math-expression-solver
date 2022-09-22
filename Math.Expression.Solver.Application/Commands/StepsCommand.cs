using Math.Expression.Solver.Application.DTOs;
using Math.Expression.Solver.Application.Parser;

namespace Math.Expression.Solver.Application.Commands
{
    public interface IStepsCommandHandler
    {
        StepsResult Handle(StepsCommand command);
    }

    public class StepsCommand
    {
        string Expression { get; }

        public StepsCommand(string expression)
        {
            Expression = expression;
        }

        public class StepsCommandHandler : IStepsCommandHandler
        {
            public StepsResult Handle(StepsCommand command)
            {
                var succeeded = true;
                var message = string.Empty;
                var stepsWriter = new StepsWriter();
                double result = 0;

                try
                {
                    var parser = new ExpressionParser(stepsWriter);
                    result = parser.Parse(command.Expression);
                }
                catch
                {
                    succeeded = false;
                    message = "Math expression contains incorrect syntax.";
                }

                return new StepsResult(succeeded, message, stepsWriter, result);
            }
        }
    }
}
