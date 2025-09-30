using ExpenseTrackerV2.Infrastructure;
using ExpenseTrackerV2.Infrastructure.Persistence.Dbcontext;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);
var appSettings = builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables()
    .Build();

var connectionString = builder.Configuration.GetConnectionString("ExpenseTrackerDbContext")
    ?? throw new InvalidOperationException("Connection string 'ExpenseTrackerDbContext' not found.");

builder.Services.AddInfrastructureWebApi(connectionString);
// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

// Configure the HTTP request pipeline.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

