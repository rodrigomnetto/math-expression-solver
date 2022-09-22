using Grpc.Core;
using Math.Expression.Solver.Application.Commands;
using Math.Expression.Solver.Application.Querys;

namespace Math.Expression.Solver.Api.Services
{
    public class ExpressionService : Expression.ExpressionBase
    {
        private readonly ISolveCommandHandler _solveCommandHandler;
        private readonly IStepsCommandHandler _stepsCommandHandler;
        private readonly IGetUserExpressionsQueryHandler _getUserExpressionsQueryHandler;

        public ExpressionService(ISolveCommandHandler solveCommandHandler
            , IStepsCommandHandler stepsCommandHandler
            , IGetUserExpressionsQueryHandler getUserExpressionsQueryHandler)
        {
            _solveCommandHandler = solveCommandHandler;
            _stepsCommandHandler = stepsCommandHandler;
            _getUserExpressionsQueryHandler = getUserExpressionsQueryHandler;
        }

        public override async Task<SolveReply> Solve(SolveRequest request, ServerCallContext context)
        {
            var command = new SolveCommand(request.Expression, request.UserId);
            var response = await _solveCommandHandler.Handle(command);

            return new SolveReply() { 
                Result = response.Result
              , Message = response.Message
              , Succeeded = response.Succeeded };
        }

        public override async Task<GetStepsReply> GetSteps(GetStepsRequest request, ServerCallContext context)
        {
            var command = new StepsCommand(request.Expression, request.UserId);
            var response = await _stepsCommandHandler.Handle(command);

            return new GetStepsReply()
            {
                Message = response.Message,
                Steps = response.Steps,
                Succeeded = response.Succeeded,
                Result = response.Result
            };
        }

        public override async Task<GetUserExpressionsReply> GetUserExpressions(GetUserExpressionsRequest request, ServerCallContext context)
        {
            var query = new GetUserExpressionsQuery(request.UserId);
            var response = await _getUserExpressionsQueryHandler.Handle(query);

            var reply = new GetUserExpressionsReply();
            reply.Expressions.AddRange(response.Select(s => s.Expression));
            return reply;
        }
    }
}
