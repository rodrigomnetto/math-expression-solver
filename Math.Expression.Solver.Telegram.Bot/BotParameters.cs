using Telegram.Bot;
using Telegram.Bot.Types;

namespace Math.Expression.Solver.Telegram.Bot
{
    public class BotParameters
    {
        public BotParameters(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            BotClient = botClient;
            Message = message;
            CancellationToken = cancellationToken;
        }

        public void Deconstruct(out ITelegramBotClient botClient, out Message message, out CancellationToken cancellationToken)
        {
            botClient = BotClient;
            message = Message;
            cancellationToken = CancellationToken;
        }

        public ITelegramBotClient BotClient { get; }
        public Message Message { get; }
        public CancellationToken CancellationToken { get; }
    }
}
