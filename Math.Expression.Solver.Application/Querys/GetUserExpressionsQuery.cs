using Dapper;
using Math.Expression.Solver.Application.Database;
using Math.Expression.Solver.Application.Models;

namespace Math.Expression.Solver.Application.Querys
{
    public interface IGetUserExpressionsQueryHandler
    {
        Task<IEnumerable<UserExpression>> Handle(GetUserExpressionsQuery getUserExpressionsQuery);
    }

    public class GetUserExpressionsQuery
    {
        public string UserId { get; }

        public GetUserExpressionsQuery(string userId)
        {
            UserId = userId;
        }

        public class GetUserExpressionsQueryHandler : IGetUserExpressionsQueryHandler
        {
            private readonly DbSession _dbSession;

            public GetUserExpressionsQueryHandler(DbSession dbSession)
            {
                _dbSession = dbSession;
            }

            public async Task<IEnumerable<UserExpression>> Handle(GetUserExpressionsQuery getUserExpressionsQuery)
            {
                return await _dbSession.Connection.QueryAsync<UserExpression>("SELECT * FROM \"UserExpressions\" WHERE \"UserId\" = @UserId", getUserExpressionsQuery);
            }
        }
    }
}
