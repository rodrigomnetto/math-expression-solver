using Math.Expression.Solver.Telegram.Bot;
using Serilog;
using Serilog.Events;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddGrpcClient<Expression.ExpressionClient>("Expression", o =>
        {
            o.Address = new Uri("http://localhost:5123");
        });
        services.AddHttpClient();
    }).UseSerilog((ctx, lc) => lc
    .WriteTo.File("logs/log.txt")
    .MinimumLevel.Is(LogEventLevel.Warning))
    .Build();

await host.RunAsync();
