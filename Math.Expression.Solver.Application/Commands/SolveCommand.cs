using Math.Expression.Solver.Application.Database;
using Math.Expression.Solver.Application.DTOs;
using Math.Expression.Solver.Application.Models;
using Math.Expression.Solver.Application.Parser;
using Math.Expression.Solver.Application.Repositories;

namespace Math.Expression.Solver.Application.Commands
{
    public interface ISolveCommandHandler
    {
        Task<SolveResult> Handle(SolveCommand command);
    }

    public class SolveCommand
    {
        string Expression { get; }

        string UserId { get; }

        public SolveCommand(string expression, string userId)
        {
            Expression = expression;
            UserId = userId;
        }

        public class SolveCommandHandler : ISolveCommandHandler
        {
            private readonly IUserExpressionsRepository _userExpressionsRepository;
            private readonly IUnitOfWork unitOfWork;

            public SolveCommandHandler(IUserExpressionsRepository userExpressionsRepository, IUnitOfWork unitOfWork)
            {
                _userExpressionsRepository = userExpressionsRepository;
                this.unitOfWork = unitOfWork;
            }

            public async Task<SolveResult> Handle(SolveCommand command)
            {
                var succeeded = true;
                var message = string.Empty;
                double result = 0;

                try
                {
                    var parser = new ExpressionParser();
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

                return new SolveResult(succeeded, message, result);
            }
        }
    }
}
