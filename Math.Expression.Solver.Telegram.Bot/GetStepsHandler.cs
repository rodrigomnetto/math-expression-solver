using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Math.Expression.Solver.Telegram.Bot
{
    public class GetStepsHandler : IExpressionHandler
    {
        private readonly BotParameters _botParameters;

        public GetStepsHandler(BotParameters botParameters)
        {
            _botParameters = botParameters;
        }

        public async Task<Message> Handle(Expression.ExpressionClient client, ErrorMessageHandler errorHandler)
        {
            var (botClient, message, cancellationToken) = _botParameters;
            var expression = message.Text.Split(" ")[1];
            var getStepsRequest = new GetStepsRequest()
            {
                Expression = expression
            };

            var response = await client.GetStepsAsync(getStepsRequest, cancellationToken: cancellationToken);

            if (response.Succeeded is false)
                return await errorHandler.Handle(response.Message);

            return await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: $"{response.Steps}\nResult: {response.Result}",
                replyMarkup: new ReplyKeyboardRemove(),
                cancellationToken: cancellationToken);
        }
    }
}
