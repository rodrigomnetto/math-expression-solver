using Dapper;
using Math.Expression.Solver.Application.Database;
using Math.Expression.Solver.Application.Models;

namespace Math.Expression.Solver.Application.Repositories
{
    public interface IUserExpressionsRepository
    {
        Task<int> Save(UserExpression userExpression);
    }

    public class UserExpressionsRepository : IUserExpressionsRepository
    {
        private readonly DbSession _session;

        public UserExpressionsRepository(DbSession session)
        {
            _session = session;
        }

        public Task<int> Save(UserExpression userExpression)
        {
            return _session.Connection.ExecuteAsync("INSERT INTO \"UserExpressions\" VALUES (@userId, @expression)", userExpression);
        }
    }
}
