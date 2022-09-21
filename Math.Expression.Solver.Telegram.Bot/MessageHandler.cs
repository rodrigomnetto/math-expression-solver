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

            var action = messageText.Split(' ')[0] switch
            {
                "/solve" => SolveExpression(botClient, message, cancellationToken),
                _ => Usage(botClient, message, cancellationToken)
            };

            await action;
        }

        private async Task<Message> SolveExpression(ITelegramBotClient botClient, Message message, CancellationToken cancellationToken)
        {
            await botClient.SendChatActionAsync(
                chatId: message.Chat.Id,
                chatAction: ChatAction.Typing,
                cancellationToken: cancellationToken);

            string expression = string.Empty;

            try
            {
                var client = _grpcClientFactory.CreateClient<Expression.ExpressionClient>("Expression");
                expression = message.Text.Split(" ")[1];

                var solveRequest = new SolveRequest()
                {
                    Expression = expression
                };

                var response = await client.SolveAsync(solveRequest, cancellationToken: cancellationToken);

                if (response.Succeeded is false)
                    return await SendErrorMessage(botClient, message, response.Message, cancellationToken);

                return await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: response.Result.ToString(),
                    replyMarkup: new ReplyKeyboardRemove(),
                    cancellationToken: cancellationToken);
            }
            catch (IndexOutOfRangeException)
            {
                return await SendErrorMessage(botClient, message, "Incorrect syntax. Try: /solve (2+2)", cancellationToken);
            }
            catch (Exception e)
            {
                _logger.LogError(e, expression);
                return await SendErrorMessage(botClient, message, "Sorry, we can't solve this...", cancellationToken);
            }
        }

        private static Task<Message> SendErrorMessage(ITelegramBotClient botClient, Message message, string error, CancellationToken cancellationToken)
        {
            return botClient.SendTextMessageAsync(
                chatId: message.Chat.Id,
                text: error,
                replyMarkup: new ReplyKeyboardRemove(),
                cancellationToken: cancellationToken);
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
