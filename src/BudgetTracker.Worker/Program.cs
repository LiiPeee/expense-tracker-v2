using BudgetTracker.Worker;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<AtualizacaoDeSaldoWorker>();

var host = builder.Build();
host.Run();

