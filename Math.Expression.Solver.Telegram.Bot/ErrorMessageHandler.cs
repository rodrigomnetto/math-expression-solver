using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Math.Expression.Solver.Telegram.Bot
{
    public class ErrorMessageHandler
    {
        private readonly BotParameters _botParameters;

        public ErrorMessageHandler(BotParameters botParameters)
        {
            _botParameters = botParameters;
        }

        public Task<Message> Handle(string error)
        {
            var (botClient, message, cancellationToken) = _botParameters;

            return botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: error,
                replyMarkup: new ReplyKeyboardRemove(),
                cancellationToken: cancellationToken);
        }
    }
}
