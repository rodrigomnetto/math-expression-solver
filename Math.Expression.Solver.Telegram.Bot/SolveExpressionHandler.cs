using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using static Math.Expression.Solver.Telegram.Bot.Expression;

namespace Math.Expression.Solver.Telegram.Bot
{
    public class SolveExpressionHandler : IExpressionHandler
    {
        private readonly BotParameters _botParameters;

        public SolveExpressionHandler(BotParameters botParameters)
        {
            _botParameters = botParameters;
        }

        public async Task<Message> Handle(ExpressionClient client, ErrorMessageHandler errorHandler)
        {
            var (botClient, message, cancellationToken) = _botParameters;
            var expression = message.Text.Split(" ")[1];
            var solveRequest = new SolveRequest()
            {
                Expression = expression
            };

            var response = await client.SolveAsync(solveRequest, cancellationToken: cancellationToken);

            if (response.Succeeded is false)
                return await errorHandler.Handle(response.Message);

            return await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: response.Result.ToString(),
                replyMarkup: new ReplyKeyboardRemove(),
                cancellationToken: cancellationToken);
        }
    }
}
