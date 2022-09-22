using Telegram.Bot;
using Telegram.Bot.Types;
using static Math.Expression.Solver.Telegram.Bot.Expression;

namespace Math.Expression.Solver.Telegram.Bot
{
    public interface IExpressionHandler
    {
        Task<Message> Handle(ExpressionClient client, ErrorMessageHandler errorHandler);
    }
}
