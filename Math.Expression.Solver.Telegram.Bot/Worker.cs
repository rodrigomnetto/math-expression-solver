using Grpc.Net.ClientFactory;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace Math.Expression.Solver.Telegram.Bot
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly GrpcClientFactory _grpcClientFactory;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public Worker(ILogger<Worker> logger, GrpcClientFactory grpcClientFactory, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _logger = logger;
            _grpcClientFactory = grpcClientFactory;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var receiverOptions = new ReceiverOptions()
            {
                AllowedUpdates = new UpdateType[] { UpdateType.Message },
                ThrowPendingUpdates = false,
            };

            var token = _configuration["BotConfiguration:BotToken"];

            while (!stoppingToken.IsCancellationRequested)
            {
                var httpClient = _httpClientFactory.CreateClient();
                var telegramClient = new TelegramBotClient(token, httpClient);
                var handler = new MessageHandler(_grpcClientFactory);
                await telegramClient.ReceiveAsync(handler, receiverOptions, stoppingToken);

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}