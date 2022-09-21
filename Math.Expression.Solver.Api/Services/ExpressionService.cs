using Grpc.Core;
using Math.Expression.Solver.Application.Commands;
using static Math.Expression.Solver.Application.Commands.SolveCommand;

namespace Math.Expression.Solver.Api.Services
{
    public class ExpressionService : Expression.ExpressionBase
    {
        private readonly ISolveCommandHandler _solveCommandHandler;

        public ExpressionService(ISolveCommandHandler solveCommandHandler)
        {
            _solveCommandHandler = solveCommandHandler;
        }

        public override Task<SolveReply> Solve(SolveRequest request, ServerCallContext context)
        {
            var command = new SolveCommand(request.Expression);
            var response = _solveCommandHandler.Handle(command);

            return Task.FromResult(new SolveReply() { 
                Result = response.Result
              , Message = response.Message
              , Succeeded = response.Succeeded });
        }
    }
}
