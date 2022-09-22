
namespace Math.Expression.Solver.Application.Models
{
    public sealed class UserExpression
    {
        public UserExpression(string userId, string expression)
        {
            UserId = userId;
            Expression = expression;
        }

        public string UserId { get; }

        public string Expression { get; }
    }
}
