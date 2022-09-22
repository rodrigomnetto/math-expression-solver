using Grpc.Net.ClientFactory;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Math.Expression.Solver.Telegram.Bot
{
    public class MessageHandler : IUpdateHandler
    {
        private readonly GrpcClientFactory _grpcClientFactory;
        private readonly ILogger<MessageHandler> _logger;

        public MessageHandler(GrpcClientFactory grpcClientFactory, ILogger<MessageHandler> logger)
        {
            _grpcClientFactory = grpcClientFactory;
            _logger = logger;
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            var message = update.Message;

            if (message?.Text is not { } messageText)
                return;

            var botParameters = new BotParameters(botClient, message, cancellationToken);

            var action = messageText.Split(' ')[0] switch
            {
                "/solve" => Execute(botParameters, new SolveExpressionHandler(botParameters)),
                "/steps" => Execute(botParameters, new GetStepsHandler(botParameters)),
                _ => Usage(botClient, message, cancellationToken)
            };

            await action;
        }

        private async Task<Message> Execute(BotParameters botParameters, IExpressionHandler handler)
        {
            var (botClient, message, cancellationToken) = botParameters;
            var errorHandler = new ErrorMessageHandler(botParameters);

            await botClient.SendChatActionAsync(
                chatId: message.Chat.Id,
                chatAction: ChatAction.Typing,
                cancellationToken: cancellationToken);

            try
            {
                var client = _grpcClientFactory.CreateClient<Expression.ExpressionClient>("Expression");
                return await handler.Handle(client, errorHandler);
            }
            catch (IndexOutOfRangeException)
            {
                return await errorHandler.Handle("Incorrect syntax. Try: /solve (2+2)");
            }
            catch (Exception e)
            {
                _logger.LogError(e, message.Text);
                return await errorHandler.Handle("Sorry, we can't solve this...");
            }
        }

        static async Task<Message> Usage(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            const string usage = "Usage:\n" +
                                 "/solve - solve math expression\n" +
                                 "/memo  - return last expressions\n" +
                                 "/steps - return steps for solving expression\n";

            return await botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: usage,
                replyMarkup: new ReplyKeyboardRemove(),
                cancellationToken: cancellationToken);
        }

        public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            _logger.LogError(exception, ErrorMessage);

            if (exception is RequestException)
                await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
        }
    }
}
