using Grpc.Core;
using Math.Expression.Solver.Application.Commands;

namespace Math.Expression.Solver.Api.Services
{
    public class ExpressionService : Expression.ExpressionBase
    {
        private readonly ISolveCommandHandler _solveCommandHandler;
        private readonly IStepsCommandHandler _stepsCommandHandler;

        public ExpressionService(ISolveCommandHandler solveCommandHandler, IStepsCommandHandler stepsCommandHandler)
        {
            _solveCommandHandler = solveCommandHandler;
            _stepsCommandHandler = stepsCommandHandler;
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

        public override Task<GetStepsReply> GetSteps(GetStepsRequest request, ServerCallContext context)
        {
            var command = new StepsCommand(request.Expression);
            var response = _stepsCommandHandler.Handle(command);

            return Task.FromResult(new GetStepsReply()
            {
                Message = response.Message,
                Steps = response.Steps,
                Succeeded = response.Succeeded,
                Result = response.Result
            });
        }
    }
}
