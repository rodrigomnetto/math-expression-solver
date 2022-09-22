using Math.Expression.Solver.Api.Services;
using Math.Expression.Solver.Application.Commands;
using Math.Expression.Solver.Application.Database;
using Math.Expression.Solver.Application.Querys;
using Math.Expression.Solver.Application.Repositories;
using static Math.Expression.Solver.Application.Commands.SolveCommand;
using static Math.Expression.Solver.Application.Commands.StepsCommand;
using static Math.Expression.Solver.Application.Querys.GetUserExpressionsQuery;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddScoped<ISolveCommandHandler, SolveCommandHandler>();
builder.Services.AddScoped<IStepsCommandHandler, StepsCommandHandler>();
builder.Services.AddScoped<IUserExpressionsRepository, UserExpressionsRepository>();
builder.Services.AddScoped<DbSession>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<IUserExpressionsRepository, UserExpressionsRepository>();
builder.Services.AddScoped<IGetUserExpressionsQueryHandler, GetUserExpressionsQueryHandler>();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<ExpressionService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
