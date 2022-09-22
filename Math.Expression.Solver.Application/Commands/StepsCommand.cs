using Math.Expression.Solver.Application.Database;
using Math.Expression.Solver.Application.DTOs;
using Math.Expression.Solver.Application.Models;
using Math.Expression.Solver.Application.Parser;
using Math.Expression.Solver.Application.Repositories;

namespace Math.Expression.Solver.Application.Commands
{
    public interface IStepsCommandHandler
    {
        Task<StepsResult> Handle(StepsCommand command);
    }

    public class StepsCommand
    {
        string Expression { get; }

        string UserId { get; }

        public StepsCommand(string expression, string userId)
        {
            Expression = expression;
            UserId = userId;    
        }

        public class StepsCommandHandler : IStepsCommandHandler
        {
            private readonly IUserExpressionsRepository _userExpressionsRepository;
            private readonly IUnitOfWork unitOfWork;

            public StepsCommandHandler(IUserExpressionsRepository userExpressionsRepository, IUnitOfWork unitOfWork)
            {
                _userExpressionsRepository = userExpressionsRepository;
                this.unitOfWork = unitOfWork;
            }

            public async Task<StepsResult> Handle(StepsCommand command)
            {
                var succeeded = true;
                var message = string.Empty;
                var stepsWriter = new StepsWriter();
                double result = 0;

                try
                {
                    var parser = new ExpressionParser(stepsWriter);
                    result = parser.Parse(command.Expression);

                    unitOfWork.BeginTransaction();
                    await _userExpressionsRepository.Save(new UserExpression(command.UserId, command.Expression));
                    unitOfWork.Commit();
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
